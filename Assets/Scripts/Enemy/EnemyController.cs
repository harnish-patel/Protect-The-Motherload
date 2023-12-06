/* 
 * Name: Harnish Patel
 * Student Number: 3632126
 * Course: COMP 486
 * 
 * Script Name: EnemyController
 * Description: this script controls the enemy behaviour within the game. It controls enemy movement, spawning
 *              shooting, taking damage, adding score, etc. 
 * 
 * Functions   
 *      Start()
 *          Description:    Initializes shot counter variable and set enemy spawn position using EnemySpawn() function
 *          Expected Input:     None
 *          Expected Result:    initialize shot counter variable and spawn the enemy in a random position
 *          Called By:  Unity
 *          Will Call:  EnemySpawn()
 *      
 *      Update()
 *          Description:    Updates the enemy position during each frame and checks if enemy is allowed to shoot
 *          Expected Input:     None
 *          Expected Result:    The enemy position updated to either move down or move diagonally (all framrate
 *                              independent). If enemy allowed to shoot, start shooting
 *          Called By:  Unity
 *          Will Call:  DiagonalMovement(), ShootingMechanic()
 *          
 *      OnBecameInvisible()
 *          Description:    This function is a Message of the MonoBehaviour class (built into Unity). When Object is
 *                          off the screen, this set flag allowShooting to be false (don't want enemy to shoot if off
 *                          screen) and isInvinsible to be true (don't want enemy to take damage as it's off screen).
 *                          Furthermore, it will call EnemySpawn() to respawn back at the top of the screen
 *          Expected Input:     None
 *          Expected Result:    When object is off screen, don't take damage, stop ability to shoot & respawn back to 
 *                              top of screen
 *          Called By:  Unity
 *          Will Call:  EnemySpawn()
 *          
 *      OnBecameVisible()
 *          Description:    This function is a Message of the MonoBehaviour class (built into Unity). When object is
 *                          on the screen, set flag isInvisible to false (enemy can take damage) & check if enemy can 
 *                          shoot. If so, set allowShooting flag to be true (start shooting again)
 *          Expected Input:     None
 *          Expected Result:    When object is within screen, allow it to take damage and check if it can shoot, and 
 *                              if so, then set allowShooting to be true so it can begin shooting
 *          Called By:  Unity
 *          Will Call:  None
 *          
 *      EnemySpawn()
 *          Description:    This function is used to randomly spawn enemies using the ranges set. Since transform.position
 *                          works in world space, these values will translate correctly regardless of the screen size. 
 *                          The purpose of the random spawn is to keep the game fresh so it's not the same each time the
 *                          game is run. 
 *          Expected Input:     None
 *          Expected Result:    Spawn object randomly within the set locataions. because this is in world space, it will
 *                              always be spawned outside of screen
 *          Called By:  Start(), OnBecameInvisible()
 *          Will Call:  None
 *          
 *      ShootingMechanic()
 *          Description:    This function is used control the enemy shooting. It uses a counter to ensure a certain time
 *                          delay has passed between shots and if so, spawns the new laser. When the new laser is spawned,
 *                          a laser SFX is played and the counter is reset.
 *          Expected Input:    Shoot lasers at the specific time increments set by shotDelay
 *          Expected Result:    None
 *          Called By:  Update()
 *          Will Call:  AudioManager's PlaySFX(), Instantiate()
 *          
 *      DamageEnemy()
 *          Description:    This function is used to control what happens when an enemy is damaged. When called, it checks
 *                          to ensure isInvinsible flag is false (can be hit) and if so, an enemy hit SFX is played and the
 *                          enemy health is decremented. If the enemy reaches 0 health, a death SFX is played and it will 
 *                          call a function to update the player score and spawn a drop based on a set probability. If the
 *                          enemy object is not an asteroid (flag set in Unity), the numEnemies is decremented and WaveStatus
 *                          is checked in the WaveController script. Finally, the game object is destroyed and the death 
 *                          animation is instantiated in the exact position.
 *          Expected Input:     None
 *          Expected result:    Removes 1 health when called and play hit SFX. if enemy has 0 health, play death SFX, update 
 *                              player score based on type of enemy killed, do probability check to see if drop will be spawned,
 *                              decrement numEnemies in WaveController and check WaveStatus in WaveController if not asteroid,
 *                              and finally remove object and play death animation prefab
 *          Called By:  OnTriggerEnter2D in ShootLaser script
 *          Will Call:  AudioManager's PlaySFX(), EnemyScore(), SpawnDrop(), Destroy(), WaveController's WaveStatus(), Instantiate()
 *          
  *      EnemyScore()
 *          Description:    This function is used to retrieve the value of the enemy killed by checking the gameObject
 *                          name. Then it adds this score to the Player score by calling AddScore() from the GameController
 *                          script
 *          Expected Input:     None
 *          Expected result:    Adjust score of player based on enemy killed 
 *          Called By:  DamageEnemy()
 *          Will Call:  GameController's AddScore(
 *          
 *      SpawnDrop()
 *          Description:    This function is used to determine if a drop will spawn and which drop will spawn. It does this by 
 *                          randomly picking a value between 0 and 1. If the value is larger than the dropProbability (set in Unity), 
 *                          then a drop will spawn. Now a random drop will be selected by randomly picking a number between 1 and 5
 *                          (1 inclusive, 5 exclusive). Each number correlates to a particular drop. The drop is instantiated based on
 *                          the enemy's current position and a 0 rotation.
 *          Expected Input:     None
 *          Expected result:    Check if drop will be spawned. If so, randomly select a drop and spawn it in the set location
 *          Called By:  DamageEnemy()
 *          Will Call: None
 *          
 *      DiagonalMovement()
 *          Description:    This function is used to move the object down diagonally (always starts moving right first). It does this by 
 *                          checking if a flag to see which direction it should currently be moving, when it reaches a boundary, the
 *                          flag is changed to be the opposite direction, and the object moves diagonally in the opposite direction
 *          Expected Input:     None
 *          Expected result:    Start moving diagonally right first until it hits boundary, then set flag to false and move diagonally
 *                              left. Repeat.
 *          Called By:  Update()
 *          Will Call: None         
 *          
 *      OnCollisionEnter2D
 *          Description:    The OnCollisionEnter2D is a parameter of MonoBehaviour from Unity. This is called when the object's
 *                          collider makes contact with another object's collider. In this implementation, we check if the other
 *                          object has tag "Player", if so, we call DamagePlayer from HealthManager script to damage the player.
 *          Expected Input:     The other Collider2D that is involved in collision
 *          Expected result:    If the other collided object is the Player (tag = "Player"), damge the Player. If it's not the
 *                              player, do nothing. 
 *          Called By:  Unity
 *          Will Call: HealthManager's DamagePlayer().
 */


using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Dependencies.NCalc;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    //speed enemy can move
    public float moveSpeed;

    //flag set if enemy can shoot & when enemy is allowed to shoot (when on screen)
    public bool canShoot, allowShooting;
    
    //Enemy laser prefab is set to this
    public GameObject shot;
    
    //Transform for where laser will spawn
    public Transform laserLocation;

    //the time delay set between shots
    public float shotDelay;

    //a counter to track when to shoot
    private float shotCounter;

    
    //values used for random spawn (based on world space, so these values will always work even if using device with taller screen size)
    private float xMin = -1.9f;
    private float xMax = 1.9f;
    private float yMin = 7f;
    private float yMax = 15f;

    //enemyhealth
    public int health;

    //death animation prefab is set to this
    public GameObject deathAnim;

    //score for enemy
    private int scoreValue;

    //for drops
    public GameObject shieldDrop, overdriveDrop, frenzyDrop, healthDrop;
    public float dropProbability;

    //flag if enemy invinsible (cannot damage)
    public bool isInvinsible;

    //flag for if enemy moves diagonally & flag for current direction moving
    public bool moveDiagonal, movingRight = true;
    
    //flag if enemy is an asteroid
    public bool isAsteroid;


    // Start is called before the first frame update
    void Start()
    {
        isInvinsible = true;
        shotCounter = shotDelay;
        EnemySpawn();
    }

    // Update is called once per frame
    void Update()
    {
        //move enemy diagonally
        if (moveDiagonal)
        {
            DiagonalMovement();
        }

        //move enemy down based on move speed (framerate independent)
        else
        {
            transform.position -= new Vector3(0f, moveSpeed * Time.deltaTime, 0f);
        }

        //if flag for allowShooting active, run ShootingMechanic function
        if (allowShooting)
        {
            ShootingMechanic();
        }
    }

    // respawns if enemy makes it to bottom of the screen without dying
    private void OnBecameInvisible()
    {
        //cannot damage
        isInvinsible = true;

        //turn off ability to shoot when object leaves screen
        allowShooting = false;

        //respawn enemy back to top of screen using EnemySpawn()
        EnemySpawn();
    }

    //when enemy is visible on screen, if flag for canShoot is true, set flag for allowShooting to also be true
    private void OnBecameVisible()
    {
        //can damage
        isInvinsible = false;

        //turn on ability to shoot if the enemy can shoot
        if (canShoot)
        {
            allowShooting = true;
        }
    }

    //Spawns enemy in random spawn positions (both horizontally and vertically) to add variance to gameplay
    private void EnemySpawn()
    {
        transform.position = new Vector3(Random.Range(xMin, xMax), Random.Range(yMin, yMax), 0f);
    }

    //logic for shooting
    private void ShootingMechanic()
    {
        //decrement the shot counter
        shotCounter -= Time.deltaTime;

        //when shot counter is <=0 (time delay passes)
        if (shotCounter <= 0)
        {
            //Play laser SFX
            AudioManager.instance.PlaySFX(3);

            //spawn enemy laser in the the position of the laserLocation Transform
            Instantiate(shot, laserLocation.position, laserLocation.rotation);
            
            //reset shot counter
            shotCounter = shotDelay;
        }
    }

    //Logic for when enemy is damaged
    public void DamageEnemy()
    {
        //make sure enemy isn't invinsible
        if (!isInvinsible)
        {
            //play SFX for when enemy hit
            AudioManager.instance.PlaySFX(2);

            //decrement enemy health
            health--;
        }

        //if enemy has 0 health
        if(health <= 0)
        {
            //play SFX for enemy death
            AudioManager.instance.PlaySFX(1);

            //call function to set score based on the enemy type killed
            EnemyScore();

            //call function for drops
            SpawnDrop();

            //destroy enemy gameobject
            Destroy(this.gameObject);

            //decrement numEnemies in the waveController script if enemy is not an asteroid
            if (!isAsteroid)
            {
                WaveController.instance.numEnemies--;
                WaveController.instance.WaveStatus();
            }

            //spawn the death animation for the enemy (animation set in prefab)
            Instantiate(deathAnim, transform.position, transform.rotation);
        }
    }

    //function to add enemy score to current score
    private void EnemyScore()
    {
        //if enemy called "Enemy1" its score value is 100
        if (gameObject.name == "Enemy1(Clone)")
        {
            scoreValue = 100;
        }

        //if enemy called "Enemy2" its score value is 300
        else if (gameObject.name == "Enemy2(Clone)")
        {
            scoreValue = 300;
        }

        //if enemy called "Enemy3" its score value is 500
        else if (gameObject.name == "Enemy3(Clone)")
        {
            scoreValue = 500;
        }

        //call AddScore function from GameController to increase player score
        GameController.Instance.AddScore(scoreValue);
    }

    //function to spawn drop
    private void SpawnDrop()
    {
        //Random.value picks a random value between 0-1
        //dropProbability set as a prefab value for a type of enemy (ex. Enemy1 dropProbability = 0.3 so 30% chance)
        if(Random.value < dropProbability)
        {
            //randomly select 1 of 4 types of drops (Random.Range(x,y): x inclusive, y exclusive
            int randomDropType = Random.Range(1, 5);
            switch (randomDropType)
            {
                //spawn shield
                case 1:
                    //quaternion represents rotation and identity corresponds to no rotation
                    //explictly set Quaternion.identity for asteroid since that rotates
                    Instantiate(shieldDrop, transform.position, Quaternion.identity);
                    break;
                //spawn overdrive
                case 2:
                    Instantiate(overdriveDrop, transform.position, Quaternion.identity);
                    break;
                //spawn frenzy
                case 3:
                    Instantiate(frenzyDrop, transform.position, Quaternion.identity);
                    break;
                //spawn heart
                case 4:
                    Instantiate(healthDrop, transform.position, Quaternion.identity);
                    break;
            }
        }
    }

    //lotic for diagonal movement
    public void DiagonalMovement()
    {
        //check flag if should be moving right
        if (movingRight)
        {
            // Move diagonally to the right
            transform.position += new Vector3(moveSpeed * Time.deltaTime, -moveSpeed * Time.deltaTime, 0f);

            // Check if we've reached the right limit
            if (transform.position.x >= GameController.Instance.rightLimit.position.x)
            {
                //set moving right flag to false to identify should now be moving left
                movingRight = false;
            }
        }
        //moving left logic
        else
        {
            // Move diagonally to the left
            transform.position -= new Vector3(moveSpeed * Time.deltaTime, moveSpeed * Time.deltaTime, 0f);

            // Check if we've reached the left limit
            if (transform.position.x <= GameController.Instance.leftLimit.position.x)
            {
                //set moving right flag to true to identify should now be moving right
                movingRight = true;
            }
        }
    }

    //If enemy Collider2d collides with Collider of Player, damage player
    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            HealthManger.instance.DamagePlayer();
        }
    }
}
