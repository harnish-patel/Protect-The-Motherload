/* 
 * Name: Harnish Patel
 * Student Number: 3632126
 * Course: COMP 486
 * 
 * Script Name: BossController
 * Description: This script controls the behavior of the final boss in the game, including health management, 
 *              shooting, shielding, and phase transitions.

 * Functions
 *      Start()
 *          Description:    Initializes boss parameters, health, and UI elements at the start of the boss encounter.
 *          Expected Input:     None
 *          Expected Result:    Boss setup and UI display.
 *          Called By:  Unity's Start() method.
 *          Will Call:  ShootingMechanic(), Shielding().

 *      Update()
 *          Description:    Manages the shooting and shielding aspect of the boss
 *          Expected Input:     None
 *          Expected Result:    Boss shooting running as long as canShoot flag is true and active shield 
 *                              when isShielded flag is true
 *          Called By:  Unity's
 *          Will Call:  ShootingMechanic(), Shielding().

 *      HurtBoss()
 *          Description:    This function is used to handle when the boss takes damage. it first checks to make
 *                          sure the boss is not invinsible (based on flag), and if not, decrement health and 
 *                          show the change visually to the UI. This function also handles when the boss is
 *                          defeated (0 health) along with setting up the different boss phases based on health
 *                          remaining. 
 *          Expected Input:     None.
 *          Expected Result:    Decrease boss health if not invinsible. Adjust boss phase based on health remaining
 *                              and handle death sequence when boss health falls to 0.
 *          Called By:      Other game events.
 *          Will Call:      ExplosionPlayed(), BossInvinsible(), CannotShoot(), CanShoot().

 *      ShootingMechanic()
 *          Description:    This function handles the shooting mechanic for the boss. It uses a shot counter
 *                          to time when to shoot as well as alternate betweent wo laser spawn points by using
 *                          a ternary operator.
 *          Expected Input:     Two Transform parameters for the laser spawn locations.
 *          Expected Result:    Boss alternates shooting weapon from the two spawn locations
 *          Called By:  Update().
 *          Will Call:  Instantiate().

 *      Shielding()
 *          Description:    This function handles the shielding for the boss by using a flag. If isShielded is true
 *                          the bossShield game object is set to active, and when the flag is false, it is set to 
 *                          inactive.
 *          Expected Input:     None
 *          Expected Result:    Boss shield's is actived/inactivated based on flag status.
 *          Called By:  Update()
 *          Will Call:  None

 *      ShieldActive()
 *          Description:    This function sets the flag isShielded to true. This is mainly used in the boss
 *                          animation as an event. This means this function is run at a certain frame in the
 *                          animation
 *          Expected Input:     None
 *          Expected Result:    Boss shield becomes active.
 *          Called By:  Unity's Animation events
 *          Will Call:  None

 *      ShieldInactive()
 *          Description:    This function sets the flag isShielded to false. This is mainly used in the boss
 *                          animation as an event. This means this function is run at a certain frame in the
 *                          animation
 *          Expected Input:     None
 *          Expected Result:    Boss shield becomes inactive.
 *          Called By:  Unity's Animation events.
 *          Will Call:  None

 *      CanShoot()
 *          Description:    This function sets the flag canShoot to true. This is mainly used in the boss
 *                          animation as an event. This means this function is run at a certain frame in the
 *                          animation
 *          Expected Input:     None.
 *          Expected Result:    Sets flag for boss to shoot true, resulting in boss being able to shoot
 *          Called By:  Unity's Animation events
 *          Will Call:  None

 *      CannotShoot()
 *          Description:    This function sets the flag canShoot to false. This is mainly used in the boss
 *                          animation as an event. This means this function is run at a certain frame in the
 *                          animation
 *          Expected Input:     None.
 *          Expected Result:    Sets the flag for boss to shoot to false, resulting in boss being unable to
 *                              shoot
 *          Called By:  Unity's Animation events
 *          Will Call:  None

 *      BossInvinsible()
 *          Description:    This function sets the flag isInvinsible to true and shifts the color to be darker
 *                          to visually show the player the boss cannot take damage at this point. This is mainly 
 *                          used in the boss animation as an event. This means this function is run at a certain 
 *                          frame in the animation
 *          Expected Input:     None
 *          Expected Result:    The boss becomes invinsible (unable to take damage) and the sprite is darkened
 *          Called By:  Unity's Animation events
 *          Will Call:  None

 *      BossNotInvinsible()
 *          Description:    This function sets the flag isInvinsible to false and shifts the color back to normal
 *                          to visually show the player the boss can take damage at this point. This is mainly 
 *                          used in the boss animation as an event. This means this function is run at a certain 
 *                          frame in the animation
 *          Expected Input:     None.
 *          Expected Result:    The boss is no longer invinsible (can take damage) and the sprite is back to normal
 *          Called By:  Unity's Animation events
 *          Will Call:  None

 *      ExplosionPlayed()
 *          Description:    This function sets the flag explosionPlayed to true. This is used to make sure the
 *                          explosion is only played once. 
 *          Expected Input:     None.
 *          Expected Result:    explosionPlayed flag set to true.
 *          Called By:  HurtBoss()
 *          Will Call:  None.

 *      ExplosionNotPlayed()
 *          Description:    This function sets the flag explosionPlayed to false. This is used to make sure the
 *                          explosion is only played once.This is mainly used in the boss animation as an event. 
 *                          This means this function is run at a certain frame in the animation
 *          Expected Input:     None
 *          Expected Result:    explosionPlayed flag set to false.
 *          Called By:  Unity's Animation events.
 *          Will Call:  None.
 */

using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static System.TimeZoneInfo;


public class BossController : MonoBehaviour
{
    //enemy max health
    public int maxHealth = 100;

    //current health
    public int currHealth;

    //flags for if invinsible (cannot be hurt), shield active, and can shoot
    public bool isInvinsible, isShielded, canShoot;

    //the time delay set between shots
    public float shotDelay;

    //a counter to track when to shoot
    private float shotCounter;

    //Game object for different lasers and boss shield
    public GameObject shotUsed, shot1, shot2, bossShield;

    //Transform for where laser spawn points
    public Transform laserLocationLeft, laserLocationRight;

    //flag for alternating between laser spawn locations
    private bool useLaserLocation1 = true;

    //animator reference
    private Animator animator;

    //game objects for different explosions. mini explosion between boss phases, and final explosion when boss killed
    public GameObject MiniExplosion, FinalExplosion;
    //flag to make sure explosion doesn't spam repeat
    public bool explosionPlayed;

    //boss point value to add to score
    public int bossScore = 3000;

    //reference to sprite renderer
    public SpriteRenderer sr;

    //only 1 instance of BossController in project therefore we use this to always reference the same BossController
    public static BossController instance;

    //This is initialized before the game starts
    private void Awake()
    {
        instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        //initialize variables
        explosionPlayed = false;
        canShoot = true;
        isShielded = false;
        animator = GetComponent<Animator>();
        currHealth = maxHealth;
        shotCounter = shotDelay;
        shotUsed = shot1;

        // Handle Boss UI health bar elements
        UIController.Instance.bossHealthBar.maxValue = currHealth;
        UIController.Instance.bossHealthBar.value = currHealth;
        UIController.Instance.bossHealthBar.gameObject.SetActive(true);
        WaveController.instance.canSpawnAsteroids = true;
    }

    // Update is called once per frame
    void Update()
    {
        //if can shoot, run ShootingMechanic function
        if (canShoot)
        {
            ShootingMechanic(laserLocationLeft, laserLocationRight);
        }
        //run Shielding function
        Shielding();
    }

    //handles damage boss takes
    public void HurtBoss()
    {
        //check if boss is not invinsible (can be hurt)
        if (!isInvinsible)
        {
            //decrement health
            currHealth--;
            //update UI with health change
            UIController.Instance.bossHealthBar.value = currHealth;

            //if health is 0
            if (currHealth <= 0)
            {
                //add score for killing final boss
                GameController.Instance.AddScore(bossScore);
                //set player to invinsible so remaining game objects don't hurt player
                HealthManger.instance.invinsible = true;
                //instantiate final explosion based on boss's curent position
                Instantiate(FinalExplosion, transform.GetChild(0).position, transform.GetChild(0).rotation);
                //destroy boss game object
                Destroy(this.gameObject);
                //remove boss health bar UI
                UIController.Instance.bossHealthBar.gameObject.SetActive(false);
                //run GameComplete logic
                UIController.Instance.GameComplete();
            }
        }

        // Part 2 of Boss Battle start with 50% health left
        if (currHealth == maxHealth / 2)
        {
            //logic to ensure mini explosion plays only once
            if (!explosionPlayed)
            {
                Instantiate(MiniExplosion, transform.GetChild(0).position, transform.GetChild(0).rotation);
                ExplosionPlayed();
            }

            //Run Boss Invinsible logic
            BossInvinsible();
            //Stop ability to shoot
            CannotShoot();
            //set shot delay
            shotDelay = 0.5f;
            //set bool in animator to change to boss phase 2 animation
            animator.SetBool("Part1Done", true);
        }
        
        // Part 3 of Boss Battle start with 20% health left
        if (currHealth == maxHealth / 5)
        {
            //logic to ensure mini explosion plays only once
            if (!explosionPlayed)
            {
                Instantiate(MiniExplosion, transform.GetChild(0).position, transform.GetChild(0).rotation);
                ExplosionPlayed();
            }
            //Run Boss Invinsible Logic
            BossInvinsible();
            //Allow ability to shoot
            CanShoot();
            //change shot to 2nd type
            shotUsed = shot2;
            //decrease shot delay so enemy shoots faster
            shotDelay = 0.2f;
            //set bool in animator to change to boss phase 3 animation
            animator.SetBool("Part2Done", true);
        }
    }

    //Logic for shooting
    private void ShootingMechanic(Transform laserLocation1, Transform laserLocation2)
    {
        // Decrement the shot counter
        shotCounter -= Time.deltaTime;

        // When shot counter is <= 0 (time delay passes)
        if (shotCounter <= 0)
        {
            // Determine which laser blaster to use
            Transform selectedLocation = useLaserLocation1 ? laserLocation1 : laserLocation2;

            // Spawn enemy laser in the selected location
            Instantiate(shotUsed, selectedLocation.position, selectedLocation.rotation);

            // Reset shot counter
            shotCounter = shotDelay;

            // Toggle the flag to switch to the other laser blaster for the next shot
            useLaserLocation1 = !useLaserLocation1;
        }
    }

    //logic for boss shield (used as event in boss animation)
    private void Shielding()
    {
        if (isShielded)
        {
            bossShield.SetActive(true);
        }
        else
        {
            bossShield.SetActive(false);
        }
    }

    //logic for turning shield on (used primarily as event in boss animation)
    public void ShieldActive()
    {
        isShielded = true;
    }

    //logic for turning shield off (used primarily as event in boss animation)
    public void ShieldInactive()
    {
        isShielded = false;
    }

    //logic to allow shooting (used primarily as event in boss animation)
    public void CanShoot()
    {
        canShoot = true;
    }

    //logic to not allow shooting (used primarily as event in boss animation)
    public void CannotShoot()
    {
        canShoot = false;
    }

    //logic to set invinsible (used primarily as event in boss animation)
    public void BossInvinsible()
    {
        isInvinsible = true;
        sr.color = new Color(0.494f, 0.494f, 0.494f);
    }

    //logic to set not invinsible (used primarily as event in boss animation)
    public void BossNotInvinsible()
    {
        isInvinsible = false;
        sr.color = new Color(1f, 1f, 1f);
    }

    //logic to set explosion has played
    public void ExplosionPlayed()
    {
        explosionPlayed = true;
    }

    //logic to set explosion not played
    public void ExplosionNotPlayed()
    {
        explosionPlayed = false;
    }
}
