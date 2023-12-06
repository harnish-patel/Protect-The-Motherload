/* 
 * Name: Harnish Patel
 * Student Number: 3632126
 * Course: COMP 486
 * 
 * Script Name: GameController
 * Description: this script controls the movement of objects. Currently it is setup to only go down at a rate of moveSpeed
 *              and is used for the shield drop. The code also includes a rotation feature that was being tested out. The
 *              functionality works but the feature may be used for the next version of the game.
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
 *          Description:    This function begins on start and retrieves the current and high scores and stores them in
 *                          the variables declared in this script. Additionally, the score text UI is updated to match
 *                          the current score. This is important when the Level Changes, the current score needs to carry
 *                          over. 
 *          Expected Input:     None
 *          Expected Result:    currScore and highScore variables have correct values filled and the UI showing the 
 *                              current score is updated
 *          Called By:  Unity
 *          Will Call:  PlayerPrefs.GetInt()
 *          
 *      AddScore()
 *          Description:    This function adds a score to the current score and then updates the PlayerPref for current 
 *                          score. Additionally it will call the CompareScore() function to check if the high score is 
 *                          broken. Finally the UI score text in the GameElements UI Panel is updated to show the new
 *                          current score. PlayerPrefs is an API in unity that can save and load data between game sessions.
 *          Expected Input:     int score - the score that will be added to the current score
 *          Expected Result:    currentscore updated, UI shows new change to score, if current score greater than high score, 
 *                              update PlayerPrefs of highscore (this is done in CompareScore())
 *          Called By:      EnemyScore() in EnemyController script
 *          Will Call:      PlayerPrefs.SetInt(), CompareScore()
 *                    
 *      CompareScore()
 *          Description:    This function is used to compare and update high score if current score is greater 
 *          Expected Input:     None
 *          Expected Result:    if current score larger greater than highscore, update high score and PlayerPrefs for highscore
 *          Called By:      AddScore()
 *          Will Call:      PlayerPrefs.SetInt()
 *          
 *      ResetCurrentScore()
 *          Description:    This function is used to reset the currentscore in PlayerPrefs to 0. This is used when a new game
 *                          starts or when a game is reset
 *          Expected Input:     None
 *          Expected Result:    Set currentscore to 0
 *          Called By:      StartGame() - UIController, Reset() - UIController
 *          Will Call:      PlayerPrefs.SetInt()
 *          
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    public static GameController Instance;

    //declaration for current and high scores
    public int currScore;
    private int highScore;

    //declaration for shield
    public float shieldDuration = 5, overdriveDuration = 5, frenzyDuration = 5;
    private float shieldCounter, overdriveCounter, frenzyCounter;
    public GameObject playerShield, normalEngine, boostedEngine, frenzyWeapon, normalWeapon;

    public Transform leftLimit, rightLimit;
    public bool frenzyActive;

    //This is initialized before the game starts
    private void Awake()
    {
        Instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        //initialize shield counter
        shieldCounter = 0;
        overdriveCounter = 0;
        frenzyCounter = 0;
        frenzyActive = false;

        //store "CurrentScore" from PlayerPrefs to currScore
        //currScore = PlayerPrefs.GetInt("CurrentScore");
        AudioManager.instance.PlaySFX(15);
        currScore = 0;

        //store "HighScore" from PlayerPrefs to highScore
        highScore = PlayerPrefs.GetInt("HighScore");
        UIController.Instance.scoreText.text = "Score: " + currScore;
    }

    // Update is called once per frame
    void Update()
    {
        //call UpdateShield function
        UpdateShield();
        UpdateOverdrive();
        UpdateFrenzy();
    }

    public void AddScore(int score)
    {
        //add score to current game score
        currScore += score;
        
        //update current score in Playerprefs
        PlayerPrefs.SetInt("CurrentScore", currScore);

        //Update highscore if needed
        CompareScore();

        //update UI in game to show new current score
        UIController.Instance.scoreText.text = "Score: " + currScore;
    }


    //Update highscore if currentscore is greater than highscore
    public void CompareScore()
    {
        //current score greater than highscore
        if(currScore > highScore)
        {
            //update highscore
            highScore = currScore;

            //update highscore in PlayerPrefs
            PlayerPrefs.SetInt("HighScore", highScore);
        }
    }

    public void ResetCurrentScore()
    {
        PlayerPrefs.SetInt("CurrentScore", 0);
        currScore = 0;
    }

    //active the shield
    public void ShieldActive()
    {
        //set to 0 to reset timer even if currently active
        shieldCounter = 0;

        //spawn playerShield around player
        playerShield.SetActive(true);
    }

    //handle shield while active
    public void UpdateShield()
    {
        //if shield is active
        if (playerShield.activeInHierarchy)
        {
            //start incrementing shieldCounter (framerate independent
            shieldCounter += Time.deltaTime;

            //if counter equals or exceeds shieldDuration, it must be turned off
            if (shieldCounter >= shieldDuration)
            {
                //reset counter
                shieldCounter = 0;

                //remove shield around player
                playerShield.SetActive(false);
            }
        }
    }

    public void OverdriveActive()
    {
        //set to 0 to reset timer even if currently active
        overdriveCounter = 0;

        //spawn playerShield around player
        normalEngine.SetActive(false);
        boostedEngine.SetActive(true);

        PlayerController.Instance.speed = 5;
    }

    public void UpdateOverdrive()
    {
        if (boostedEngine.activeInHierarchy)
        {
            overdriveCounter += Time.deltaTime;

            if (overdriveCounter >= overdriveDuration)
            {
                overdriveCounter = 0;
                boostedEngine.SetActive(false);
                normalEngine.SetActive(true);
                PlayerController.Instance.speed = 3;
            }
        }
    }

    public void FrenzyActive()
    {
        frenzyCounter = 0;
        frenzyActive = true;
        frenzyWeapon.SetActive(true);
        normalWeapon.SetActive(false);
        //set boosted shooter sprite on
        //set normal shooter sprite off
    }

    public void UpdateFrenzy()
    {
        frenzyCounter += Time.deltaTime;

            if (frenzyCounter >= frenzyDuration)
            {
                frenzyCounter = 0;
                frenzyActive = false;
                frenzyWeapon.SetActive(false);
                normalWeapon.SetActive(true);
        }
    }

    public void HealthBoost()
    {
        HealthManger.instance.HealPlayer();

    }
}
