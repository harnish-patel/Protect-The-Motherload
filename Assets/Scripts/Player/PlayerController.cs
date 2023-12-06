/* 
 * Name: Harnish Patel
 * Student Number: 3632126
 * Course: COMP 486
 * 
 * Script Name: PlayerController
 * Description: this script controls the player behaviour within the game. It controls enemy movement (while ensuring
 *              it stays within the bounds of the screen), and allows auto shooting weapon at a set interval (from shotDelay)
 * 
 * Functions   
 *      Awake()
 *          Description:    This function is called immediately when game started prior to the Start(). It initializes
 *                          and instance of the object within the script so we refer to that instance always
 *          Expected Input:     None
 *          Expected Result:    Used for setup purposes
 *          Called By:  Unity
 *          Will Call:  None
 *      
 *      Start()
 *          Description:    This initializes the counter for the shooting and sets up parameters to be used for
 *                          player movement
 *          Expected Input:     None
 *          Expected Result:    Used for setup, no particular result expected other than having variables setup
 *          Called By:  Unity
 *          Will Call:  None
 *          
 *      Update()
 *          Description:    This is where the player control occurs. Here we control the player movement by checking
 *                          if joystickVec is non-zero. If so, this means user is dragging finger aka attempting to move.
 *                          The velocity of the rigidbody (Rigidbody is a class in Unity that uses its physics engine)  is
 *                          updated to the joystickVec * the set speed parameter, but if joystickVec is zero, the 
 *                          rigidbody velocity is set to zero too. Additionally, the InBounds() function is used to ensure
 *                          the player stays within the bounds of the screen and the AutoShoot() function is used to auto 
 *                          shoot the laser
 *          Expected Input:     None
 *          Expected Result:    Moving joystick moves player, but if joystick is trying to move player outside bounds of
 *                              screen, player will not move outside those bounds. Furthermore, player lasers are spawned 
 *                              at the rate specified (shotDelay)
 *          Called By:  Unity
 *          Will Call:  InBounds(), AutoShoot()
 *          
 *      AutoShoot()
 *          Description:    This function auto spawns a player laser based on the shotDelay specified. It does this by
 *                          using a shot counter. Additionally, if the Frenzy drop is active, the Player shoots 2 shots
 *                          from the frenzy locations
 *          Expected Input:     float delay - shotDelay provided (setup this way for future features)
 *          Expected Result:    Auto spawn (or "fire") laser at the specified intervals and at specific locations depedning
 *                              on if Frenzy boost is active
 *          Called By:  Update()
 *          Will Call:  None
 *          
 *      InBounds()
 *          Description:    This function ensures the player stays within the bounds of the screen. it does this by 
 *                          clamping the position of the player between the screenbounds - half the size of the Player.
 *                          Clamping works by returning the min/max value if the given float val is less than the min or
 *                          greater than the max respectively
 *          Expected Input:     None
 *          Expected Result:    Player unable to move outside bounds of screen
 *          Called By:  Update()
 *          Will Call:  None
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    //only 1 instance of PlayerController in project therefore we use this to always reference the same PlayerController
    public static PlayerController Instance;

    //reference JoystickMovement 
    public JoystickMovement joystickMovement;

    //player speed
    public float speed;

    //reference player rigidbody
    public Rigidbody2D rigidBody;

    // used to make limits to prevent player from leaving bounds
    private Vector2 screenBounds;
    private float playerHeight, playerWidth;

    //handles shooting 
    public Transform laserLocation, frenzyLaserLocation1, frenzyLaserLocation2;
    public GameObject normalShot, frenzyShot;
    public float shotDelay = .1f;
    private float shotCounter;
    public bool canShoot;

    //This is initialized before the game starts
    private void Awake()
    {
        Instance = this;
    }
    
    // Start is called before the first frame update
    void Start()
    {
        //initialize game variables
        canShoot = true;
        shotCounter = shotDelay;

        //Convert bounds of screen to world coordinates
        screenBounds = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, Camera.main.transform.position.z));
        
        //get player height and width. It is divided by 2 to get distance from centre
        playerHeight = transform.GetComponent<SpriteRenderer>().bounds.size.y / 2;
        playerWidth = transform.GetComponent<SpriteRenderer>().bounds.size.x / 2;
    }

    // Update is called once per frame
    void Update()
    {
        //checks joystickVec from te JoystickMovement script. If the joystickVec is not zero, the user is currently dragging
        if(joystickMovement.joystickVec.y !=0) 
        {
            //set the velocity of the rigidbody based on the joystickVec multiplied by the provided speed
            rigidBody.velocity = new Vector2(joystickMovement.joystickVec.x, joystickMovement.joystickVec.y) * speed;
        }
        else
        {
            //if not moving, set the velocity to zero
            rigidBody.velocity = Vector2.zero;
        }
        
        //run InBounds() to keep player ship within bounds of the screen
        InBounds();

        if (canShoot)
        {
            //run AutoShoot() to keep shooting at a constant rate (set by shotDelay) indefinately
            AutoShoot(shotDelay);
        }
    }

    //Autofire shots
    private void AutoShoot(float delay)
    {
        //decrement shot counter
        shotCounter -= Time.deltaTime;

        //when shot counter is 0 (time delay passes)
        if (shotCounter <= 0)
        {
            //check if Frenzy drop is active
            if (GameController.Instance.frenzyActive)
            {
                //instantiate shot in both frenzy locations
                Instantiate(frenzyShot, frenzyLaserLocation1.position, laserLocation.rotation);
                Instantiate(frenzyShot, frenzyLaserLocation2.position, laserLocation.rotation);
            }

            //regular shoooting
            else
            {
                //spawn laser in regular position
                Instantiate(normalShot, laserLocation.position, laserLocation.rotation);
            }

            //Player Laser SFX
            AudioManager.instance.PlaySFX(6);

            //reset counter
            shotCounter = delay;
        }
    }

    //keep player within bounds of screen
    private void InBounds()
    {
        //store current position of player
        Vector3 viewPos = transform.position;

        //Clamps current position (x and y) of player between the screenbounds - half the size of the Player
        viewPos.x = Mathf.Clamp(viewPos.x, screenBounds.x * -1 + playerWidth, screenBounds.x - playerWidth);
        viewPos.y = Mathf.Clamp(viewPos.y, screenBounds.y * -1 + playerHeight, screenBounds.y - playerHeight);

        //sets the transform to the new clamped position
        transform.position = viewPos;
    }
}
