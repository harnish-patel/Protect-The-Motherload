/* 
 * Name: Harnish Patel
 * Student Number: 3632126
 * Course: COMP 486
 * 
 * Script Name: WaveController
 * Description: This script manages the spawning of enemies, asteroids, and final boss based on wave state. This 
 *              includes handling of wave completion, level transitions, steroid spawning, and final boss spawning.
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
 *          Description:    Initializes variables and settings at the start of the game.
 *          Expected Input:     None
 *          Expected Result:    variables setup for game
 *          Called By:  Unity
 *          Will Call:  None
 *
 *      Update()
 *          Description:    Manages enemy spawning behaviour based on wave state (flags used to ensure wave is defeated
 *                          and new wave is ready to spawn) as well as managing asteroid spawning
 *          Expected Input:     None
 *          Expected Result:    Enemies and asteroids spawned at the correct times
 *          Called By:  Unity
 *          Will Call:  SpawnEnemies(), SpawnAsteroids().
 *
 *      NextWave()
 *          Description:    This function increments the wave number.
 *          Expected Input:     None.
 *          Expected Result:    Wave number increases.
 *          Called By:  WaveStatus(), UIController's NextLevel()
 *          Will Call:  None
 *
 *      SpawnEnemies()
 *          Description:    This function handles spawning the enemies. When called it sets the flags waveDefeated and
 *                          readyToSpawn to false (since a new wave is about to be spawned), and then calls the
 *                          InstantiateEnemies() function (which instantiates enemies to the scene). The variable 
 *                          numEnemies is set to enemiesSpawned. This variable will be decremented in the EnemyController
 *                          to track when all enemeis within wave are defeated. Finally, the enemiesSpawned variable is
 *                          set to 0 so it's ready to be used for the next wave spawning.
 *          Expected Input:     None
 *          Expected Result:    Spawn enemies in the scene, adjust flags accordingly and set enemy tracking variables
 *          Called By:  Update()
 *          Will Call:  InstantiateEnemies()
 *
 *      InstantiateEnemies()
 *          Description:    This function handles the spawning of enemies based on wave number. It will change the
 *                          background color when a "new level" is reached (aka every 2 waves) as well as spawn a 
 *                          certain number of specific enemies for each level. Finally, when the 7th wave is reached, 
 *                          it will spawn the boss. 
 *          Expected Input:     None
 *          Expected Result:    Enemies and final boss are instantiated within the scene based on wave number
 *          Called By:  SpawnEnemies()
 *          Will Call:  SpawnNumberOfEnemies(), SpawnBoss()
 *
 *      SpawnNumberOfEnemies()
 *          Description:    This function is used to spawn enemies based on the inputted type and quantity
 *          Expected Input:     number of enemies "quantity" + type of enemy "enemyType"
 *          Expected Result:    Specified quantity of enemies are instantiated within the scene.
 *          Called By:  InstantiateEnemies()
 *          Will Call:  Instantiate()
 *
 *      SpawnBoss()
 *          Description:    This function handles spawning the boss. It turns off the regular gameplay music and starts
 *                          the boss music as well as activating the final boss game object within the scene
 *          Expected Input:     None
 *          Expected Result:    Boss enemy is activated within the scene and boss music plays
 *          Called By:  InstantiateEnemies()
 *          Will Call:  None
 *
 *      WaveStatus()
 *          Description:    This function handles what to do when enemies are killed (called by EnemyController when 
 *                          enemies die). If all enemies within wave are killed, check if waveNum is even (signifying
 *                          "Level complete". If so, set player to invinsible and call LevelComplete() function within
 *                          UIController and set flag for waveDefeated to true. If wavNum is odd (signifying first of
 *                          2 waves within level), call NextWave() function to spawn the next wave and set the flags
 *                          to spawn a wave to true.
 *          Expected Input:     None
 *          Expected Result:    When all enemies defeated within a wave, if Level complete, turn on level complete UI
 *                              elements, otherwise spawn the next wave in that level. 
 *          Called By:  EnemeyController's DamageEnemy()
 *          Will Call:  NextWave(), LevelComplete().
 *
 *      SpawnAsteroids()
 *          Description:    This function handles the spawning of asteroids. it uses a counter to ensure a set time
 *                          delay has passed between spawning of asteroids. The flag for canSpawnAsteroids is set in
 *                          BossController since we only want asteroids spawning in the boss scene. 
 *          Expected Input:     None
 *          Expected Result:    Asteroids are spawned within the scene at a specified interval
 *          Called By:  Update()
 *          Will Call:  Instantiate()
 */

using System.Collections;
using System.Collections.Generic;
//using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class WaveController : MonoBehaviour
{
    public static WaveController instance;

    //game objects for different enemy
    public GameObject enemy1, enemy2, enemy3, bossObject, asteroid;

    //number of enemies left in scene + number of enemies spawned + wave number 
    public int numEnemies, enemiesSpawned, waveNum;
    
    //flag if wave defeated + if wave ready to spawn
    public bool waveDefeated, readyToSpawn;

    //handling counter to spawn asteroids
    public float asteroidDelay = 5f;
    private float asteroidCounter;

    //flag when asteroids can spawn
    public bool canSpawnAsteroids;

    //camera object (used to change background color when changing levels)
    public Camera mainCamera;

    //This is initialized before the game starts
    public void Awake()
    {
        instance = this;
    }

    private void Start()
    {   
        //initialization
        waveNum = 1;
        waveDefeated = true;
        readyToSpawn = true;
        asteroidCounter = asteroidDelay;
        canSpawnAsteroids = false;
        mainCamera = Camera.main;
    }

    private void Update()
    {
        //if wave defeated and wave ready to spawn
        if (waveDefeated && readyToSpawn)
        {
            //spawn enemies
            SpawnEnemies();
        }

        //if asteroids ready to spawn
        if (canSpawnAsteroids)
        {
            //spawn asteroids
            SpawnAsteroids();
        }
    }

    //function to increment wave number
    public void NextWave()
    {
        waveNum++;
        Debug.Log("Incremented to: " + waveNum);
    }

    //function to spawn enemies
    private void SpawnEnemies()
    {
        //set flags
        waveDefeated = false;
        readyToSpawn = false;

        //function to handle instantiating enemies within scene
        InstantiateEnemies();

        // Set numEnemies to the total number of enemies within the wave (used later to determine if wave defeated)
        numEnemies = enemiesSpawned;
        //reset enemies spawned back to 0 (so enemiesSpawned will start counting at 0 for next wave)
        enemiesSpawned = 0;
    }

    //function to instantiate the enemies for each wave
    public void InstantiateEnemies()
    {
        //spawn based on wave number
        switch (waveNum)
        {
            //Level 1
            case 1:
                //change background color of scene
                mainCamera.backgroundColor = HexToColor("3C2A37");
                //spawn enemies
                SpawnNumberOfEnemies(3, enemy1);
                break;

            case 2:
                SpawnNumberOfEnemies(5, enemy1);
                break;

            //Level 2
            case 3:
                mainCamera.backgroundColor = HexToColor("2A2C3C");
                SpawnNumberOfEnemies(5, enemy1);
                SpawnNumberOfEnemies(2, enemy2);
                break;
            
            case 4:
                SpawnNumberOfEnemies(3, enemy1);
                SpawnNumberOfEnemies(5, enemy2);
                break;
            
            //Level 3
            case 5:
                mainCamera.backgroundColor = HexToColor("2A3C2C");
                SpawnNumberOfEnemies(3, enemy1);
                SpawnNumberOfEnemies(3, enemy2);
                SpawnNumberOfEnemies(1, enemy3);
                break;
            
            case 6:
                SpawnNumberOfEnemies(5, enemy1);
                SpawnNumberOfEnemies(3, enemy2);
                SpawnNumberOfEnemies(3, enemy3);
                break;

            //Level 4
            case 7:
                mainCamera.backgroundColor = HexToColor("3C3C2A");
                SpawnNumberOfEnemies(5, enemy1);
                SpawnNumberOfEnemies(4, enemy2);
                SpawnNumberOfEnemies(4, enemy3);
                break;

            case 8:
                SpawnNumberOfEnemies(8, enemy1);
                SpawnNumberOfEnemies(5, enemy2);
                SpawnNumberOfEnemies(5, enemy3);
                break;


            //Boss Battle
            case 9:
                mainCamera.backgroundColor = HexToColor("3C2D2A");
                SpawnBoss();
                break;
        }
    }

    //function to spawn specific type of enemy and quantity
    public void SpawnNumberOfEnemies(int quantity, GameObject enemyType)
    {
        //for loop to spawn enemies based on function input quantity
        for (int i = 0; i < quantity; i++)
        {
            //instantiate enemy based on input gameobject enemyTyper
            Instantiate(enemyType);
            //increment enemiesSpawned which is used to track when all enemies killed in wave
            enemiesSpawned++;
        }
    }

    //function to spawn the final boss
    public void SpawnBoss()
    {
        //turn off audio for regular gameplay music
        AudioManager.instance.audioSource[15].volume = 0f;
        
        //start boss music
        AudioManager.instance.PlaySFX(14);
        
        //activate boss object within scene
        bossObject.SetActive(true);
    }

    //function to handle what to do when enemies killed within scene
    public void WaveStatus()
    {
        //if all enemies within Wave are killed,
        if (numEnemies <= 0) 
        {   
            //2 "waves" within a level so if level is complete
            if (waveNum == 2 || waveNum == 4 || waveNum == 6 || waveNum == 8)
            {
                //set invinsible flag in HealthManager to be true
                HealthManger.instance.invinsible = true;

                //call the LevelComplete function within UIController
                UIController.Instance.LevelComplete();
                
                //set flag wave is defeated
                waveDefeated = true;
            }

            //spawn 2nd wave in level
            else
            {
                //call function which incrememnts wave number
                NextWave();

                //set flags
                waveDefeated = true;
                readyToSpawn = true;
            }
        }
    }

    //function to handle asteroid spawning
    public void SpawnAsteroids()
    {
        //decrement asteroid counter
        asteroidCounter -= Time.deltaTime;

        //when asteroid counter reaches 0
        if (asteroidCounter <= 0)
        {
            //instantiate asteroid game object within scene
            Instantiate(asteroid);
            
            //reset asteroid counter
            asteroidCounter = asteroidDelay;
        }
    }

    //function used to return color based on hexadecimal value
    Color HexToColor(string hex)
    {
        //create new color var to store result
        Color color = new Color();

        //Unity class that attempts to convert html color string. # added to make valid HTML color string
        ColorUtility.TryParseHtmlString("#" + hex, out color);
        return color;
    }
}
