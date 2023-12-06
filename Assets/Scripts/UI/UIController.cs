/* 
 * Name: Harnish Patel
 * Student Number: 3632126
 * Course: COMP 486
 * 
 * Script Name: UIController
 * Description: This script manages various UI elements within the game
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
 *          Description:    SFX is active when game starts so isMuted flag is set accordingly
 *          Expected Input:     None
 *          Expected Result:    isMuted flag set to false to indicate SFX is active
 *          Called By:  Unity
 *          Will Call:  None
 *          
 *      StartGame()
 *          Description:    This function handles when the Start button is pressed in the Main Menu and is used to start
 *                          gameplay. It does this by resetting the current score to 0 and changing scenes to the game.
 *          Expected Input:     None
 *          Expected Result:    Current score is set to 0 and scene is changed to game screen
 *          Called By:  Start Button in Main Menu
 *          Will Call:  ChangeScene(), ButtonPressSFX().
 *          
 *      Rules(), Leaderboards(), Home()
 *          Description:    Change scenes to Rules, Leaderboards, or Home. These functions are used by buttons in various
 *                          UI elements
 *          Expected Input:     None
 *          Expected Result:    Scene changes to Rules, Leaderboards, or Home
 *          Called By:  Rules() & Leaderboards() - Main Menu buttons, Home() - Home Button in Rules, Leaderboard and Pause Menu
 *          Will Call:  ChangeScene(), ButtonPressSFX().
 *          
 *      NextLevel()
 *          Description:    This function is used by the Continue Button in the Level Complete sceen to continue the game
 *                          and move to the next level It does this by activating Player's ability to shoot, resetting
 *                          gameplay music to max volume, adding the level complete points to the current score, calling
 *                          the next wave, turning off the Level Complete UI elements and turning on the Gameplay UI elements.
 *                          Finally the flag for ready to spawn is set to true to allow enemy spawning and player invinsibility
 *                          reset to false so Player can take damage again
 *          Expected Input:     None
 *          Expected Result:    Game Music reset to max, level complete score added to current score (and displayed visually),
 *                              Next Wave spawned and UI elements are adjusted accordingly.
 *          Called By: UI button.
 *          Will Call: ButtonPressSFX(), GameController's AddScore(), WaveController's NextWave(), GameplayUIOff.
 *          
 *      PauseGame(), UnPauseGame()
 *          Description:    The PauseGame() function is called by the Pause Button in the main game screen and the
 *                          UnPauseGame() function is called by the Play button in the Pause menu UI. They work by
 *                          activating or deactivating the pauseButton in the main UI based on what is clicked. If
 *                          the game is paused, the gameplay and boss music is reduced, the game elements are paused
 *                          by setting the timeScale to 0 and the pause screen UI is set to active. When the game is
 *                          unpaused, the gameplay and boss music is set back to max volume, the game elements are 
 *                          resumed by setting timeScale back to 1 and the pause screen UI is set to inactive. 
 *          Expected Input:     None.
 *          Expected Result:    Game is paused (gameplay paused, music lowered and pause menu UI activated), 
 *                              unpaused (gameplay resumed, music returned to normal and pause menu UI deactivated)
 *          Called By:      PauseGame() - Pause Button in game UI, UnPauseGame() - Play Button in Pause Menu UI
 *          Will Call:  ButtonPressSFX()
 *          
 *      ToggleSFX()
 *          Description:    This function is used to toggle to either mute or unmute all SFX within gameplay loop.
 *                          It is called by the Button used for the volume in the Pause Menu UI. It does this by 
 *                          inverting the status when pressed (if currently muted, make unmuted and vice versa).
 *                          If SFX is muted, Mute all SFX and adjust the text for the UI Button to show the sprite for
 *                          sound off. If SFX is unmuted, unmute all SFX and adjust the UI button to show the sprite for
 *                          sound on
 *          Expected Input:     None
 *          Expected Result:    If isMuted is currently false prior to function running, invert isMuted value, mute all 
 *                              SFX and adjust icon for button to show sound is muted. If isMuted is true prior to running
 *                              function, invert isMuted value, unmute all SFX and adjust button to show sound is unmuted
 *          Called By: UI buttons.
 *          Will Call: ButtonPressSFX(), AudioManager's MuteSounds(), AudioManager's UnMuteSounds().
 *          
 *      LevelComplete()
 *          Description:    This function is used to show the UI elements for when a level is completed and is called
 *                          By WaveStatus() in the WaveController. It does this by turning off the Player's ability to shoot,
 *                          reducing the gameplay music, playing the level complete SFX, updating the text to show the exact
 *                          level complete, turning on the level complete UI elements and turning off the gameplay UI elements
 *          Expected Input:     None
 *          Expected Result:    Player cannot shoot, gameplay music lowered, level win SFX played, Level complete UI is 
 *                              displayed (with exact level which is completed), and gameplay UI elements disabled.
 *          Called By:  WaveController's WaveStatus().
 *          Will Call:  GameplayUIOff(), AudioManager's PlaySFX().
 *          
 *      GameComplete()
 *          Description:    This function handles when a player either wins the game or loses the game. It does this by
 *                          turning off the player's ability to shoot, deactivating the gameplay ui elements, and activating
 *                          the Game Complete UI elements. Then, if the current health is 0 (aka player lost game), the
 *                          death text is activated and victory text deactivated, the gameplay and boss music is muted and
 *                          the game lose SFX is played. Otherwise, the death text is deactivated and victory text activated,
 *                          gameplay and boss music is muted, and the game win SFX is played. Finally the high score is used
 *                          to fill the high score text and the current score is used to fill the game score text                       
 *          Expected Input:     None.
 *          Expected Result:    Game complete UI is displayed and based on if game is won or lost, specific UI elements 
 *                              are displayed and specific SFX is played
 *          Called By:  BossController's HurtBoss(), HealthManager's DamagePlayer().
 *          Will Call: GameplayUIOff(), AudioManager's PlaySFX().
 *          
 *      SubmitScore()
 *          Description:    This function is used by the Leaderboards Button in the GameComplete UI to go to the
 *                          leaderboards scene after checking if a player name has been inputted into the input field
 *                          and saved
 *          Expected Input:     None
 *          Expected Result:    Scene changes to Leaderboards if player name saved. Otherwise error message text is 
 *                              displayed to prompt user to input name.
 *          Called By:  Leaderboard Button in Complete Complete UI
 *          Will Call:  ChangeScene(), ButtonPressSFX().
 *          
 *      SaveName()
 *          Description:    This function is used to save the player name typed into the input field. It does this by
 *                          setting a string playerName to whatever was typed into the name input field. Then the PlayerPref
 *                          PlayerName is set to the playerName string and the PlayerPrefs are saved
 *          Expected Input:     None
 *          Expected Result:    Player name is saved into playerprefs to be used by leaderboards script
 *          Called By:  Submit button in Game Complete UI 
 *          Will Call:  PlayerPrefs.SetString(), PlayerPrefs.Save().
 *          
 *      GameplayUIOff()
 *          Description:    This function is used to activate/inactivate the gameplay UI elements based on the status
 *                          inputted
 *          Expected Input:     bool status - status if you want GamplayUI off or on
 *          Expected Result: In-game UI elements are turned on or off.
 *          Called By:  LevelComplete(), GameComplete().
 *          Will Call:  None
 *          
 *      ButtonPressSFX()
 *          Description:    This function is used to play a button pressed SFX
 *          Expected Input:     None
 *          Expected Result:    Button press sound effect played
 *          Called By:  various functions which control button presses
 *          Will Call:  AudioManager's PlaySFX()
 *          
 *      ChangeScene()
 *          Description:    This function changes the scene based on the inputted scene name
 *          Expected Input:     String sceneName - indicating the name of the scene to change to.
 *          Expected Result:    Scene changes to the specified scene.
 *          Called By:  StartGame(), Rules(), Leaderboards(), Home(), SubmitScore
 *          Will Call:  SceneManager's LoadScene()
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;
using JetBrains.Annotations;

public class UIController : MonoBehaviour
{
    public static UIController Instance;

    //UI elements for UI Game Elements
    public Slider healthBar, bossHealthBar;
    public TMP_Text scoreText;

    //public string that is filled in to determine where scene will go next
    public string nextLevel;

    //current score
    public int currentScore;
    
    //1000 points provided when level completed successfully
    private int levelCompleteScore = 1000;

    //GameObjects that hold UI elements
    public GameObject levelComplete;
    public GameObject gameElements;
    public GameObject gameOver;
    public GameObject joystick;
    public GameObject pauseButton;

    //TMP_Text UI elements
    public TMP_Text hiScore;
    public TMP_Text gameScore;

    //GameObjects for UI Pause
    public GameObject pauseScreen;

    //icon for sound is a sprite asset (Assets/TextMeshPro/Examples & Extras/Resources/SpriteAssets/Icons)
    public TMP_Text soundIcon;
    //flag when 
    public bool isMuted;

    //Level Complete elements
    public TMP_Text levelCompleteText;

    //GameComplete elements
    public TextMeshProUGUI deathText, victoryText;
    public TextMeshProUGUI errorMessage;
    public TMP_InputField nameInputField;
    public GameObject gameComplete;

    //This is initialized before the game starts
    public void Awake()
    {
        Instance = this;
    }
    // Start is called before the first frame update
    void Start()
    {
        //music on when starting game so flag is set accordingly
        isMuted = false;

        //delete all playerprefs (used by dev)
        //PlayerPrefs.DeleteAll();
    }

    //Start game function called by the Start function in the Main Menu UI
    public void StartGame()
    {
        ButtonPressSFX();

        //reset current score back to 0
        PlayerPrefs.SetInt("CurrentScore", 0);

        //change scene to level1
        ChangeScene("Level1");
    }

    //used by Rules Button on Main Menu to change scene to Rules scene
    public void Rules()
    {
        ButtonPressSFX();

        //change scene to rules
        ChangeScene("Rules2");
    }

    //used by Leaderboards Button on Main Menu to go to Leaderboards scene
    public void Leaderboards()
    {
        ButtonPressSFX();

        //change scene to rules
        ChangeScene("Leaderboards");
    }

    //used by buttons on Pause Menu, Leaderboard and Rules scenes to go back to Home scene
    public void Home()
    {
        //ButtonPressSFX();
        Time.timeScale = 1f;

        //load startingscreen scene
        ChangeScene("StartingScreen2");
    }

    //function used by Continue Button in the LevelComplete screen to continue the game and move to the next level
    public void NextLevel()
    {
        //turn on player's ability to shoot
        PlayerController.Instance.canShoot = true;

        //reset gameplay music to full volume
        AudioManager.instance.audioSource[15].volume = 1f;
        ButtonPressSFX();

        //Add score awarded for successfully completing level
        GameController.Instance.AddScore(levelCompleteScore);
        
        //call next wave
        WaveController.instance.NextWave();

        //turn off LevelComplete UI element
        levelComplete.SetActive(false);

        //turn on all gameplay UI elements
        GameplayUIOff(true);

        //set flag for wave ready to spawn
        WaveController.instance.readyToSpawn = true;

        //turn off player invinsibility
        HealthManger.instance.invinsible = false;
    }

    //pause gameplay. Called by Pause button UI element
    public void PauseGame()
    {
        //ButtonPressSFX();
        
        //turn off Pause Button UI element from main game screen
        pauseButton.SetActive(false);
        //reduce gameplay and boss music
        AudioManager.instance.audioSource[15].volume = 0.3f;
        AudioManager.instance.audioSource[14].volume = 0.3f;

        //turn on pause screen UI elements
        pauseScreen.SetActive(true);

        //pauses all in game elements
        Time.timeScale = 0f;
    }

    //Unpause gameplay. Called by play button in Pause Menu UI
    public void UnPauseGame()
    {
        ButtonPressSFX();

        //turn on Pause Button UI element from main game screen
        pauseButton.SetActive(true);

        //return gameplay and boss music to full volume
        AudioManager.instance.audioSource[15].volume = 1f;
        AudioManager.instance.audioSource[14].volume = 1f;

        //turn off pause screen UI elements
        pauseScreen.SetActive(false);

        //unpause all in game elements
        Time.timeScale = 1f;
    }

    //Used to toggle SFX in game. Called by mute/unmute button in Pause Menu UI
    public void ToggleSFX()
    {
        ButtonPressSFX();

        //change status (if currently muted, make it unmuted. if currently unmuted, make it muted)
        isMuted = !isMuted;

        //if volume muted
        if (isMuted)
        {
            //mute all SFX
            AudioManager.instance.MuteSounds();

            //change sprite icon for Mute/UnMute button to SoundOff sprite
            soundIcon.text = "<sprite name=\"SoundOff\">";
        }

        else
        {
            //unmute all sounds
            AudioManager.instance.UnMuteSounds();

            //change sprite icon for Mute/UnMute button to SoundOn sprite
            soundIcon.text = "<sprite name=\"SoundOn\">";
        }
    }

    //function called if Level Complete (all enemies in wave defeated + even wave num). Called by WaveController's WaveStatus()
    public void LevelComplete()
    {
        //turn off player's ability to shoot within the LevelComplete UI 
        PlayerController.Instance.canShoot = false;

        //reduce volume of game music
        AudioManager.instance.audioSource[15].volume = 0.1f;

        //play level complete SFX
        AudioManager.instance.PlaySFX(11);

        //update text to show which wave has been complete
        levelCompleteText.text = "WAVE " + WaveController.instance.waveNum / 2 + " COMPLETE";

        //turn on the Level Complete UI elements
        levelComplete.SetActive(true);
       
        //turn off gameplay UI elements
        GameplayUIOff(false);
    }

    //function when game is complete (both win or lose)
    public void GameComplete()
    {
        //turn off ability for player to shoot
        PlayerController.Instance.canShoot = false;

        //turn off gameplay ui elements
        GameplayUIOff(false);

        //activate Game Complete UI
        gameComplete.SetActive(true);

        //if player lost game
        if (HealthManger.instance.currHealth == 0)
        {
            //activate death text and disable victory text
            deathText.enabled = true;
            victoryText.enabled = false;

            //turn off gameplay and boss music
            AudioManager.instance.audioSource[14].volume = 0f;
            AudioManager.instance.audioSource[15].volume = 0f;

            //play game lose SFX
            AudioManager.instance.PlaySFX(10);
        }

        //if play won game
        else
        {
            //Add score awarded for successfully completing level
            GameController.Instance.AddScore(levelCompleteScore);

            //turn off death text and activate victory text
            deathText.enabled = false;
            victoryText.enabled = true;

            //tuun off gameplay and boss music
            AudioManager.instance.audioSource[14].volume = 0f;
            AudioManager.instance.audioSource[15].volume = 0f;

            //play game win SFX
            AudioManager.instance.PlaySFX(16);
        }
        
        //set high score text based on PlayerPref that stores the highest score achieved
        hiScore.text = PlayerPrefs.GetInt("HighScore").ToString(); ;

        //set the game score based on the current score achieved this playthrough
        gameScore.text = GameController.Instance.currScore.ToString();
    }

    //used by the Leaderboard button in the GameComplete UI to go to the leaderboards scene after PlayerName is saved
    public void SubmitScore()
    {
        ButtonPressSFX();

        //if PlayerPref for PlayerName is empty (this is filled by typing into the input field and clicking submit)
        if (string.IsNullOrEmpty(PlayerPrefs.GetString("PlayerName")))
        {
            //activate error message prompting player to input name
            errorMessage.gameObject.SetActive(true);
            return;
        }

        //Add the current score to the PlayerScore PlayerPrefs used in the leaderboard function
        PlayerPrefs.SetInt("PlayerScore", GameController.Instance.currScore);

        //go to leaderboards scene
        ChangeScene("Leaderboards");
    }

    //called by the Submit button in the Game Complete UI to save the player name typed into the input field
    public void SaveName()
    {
        ButtonPressSFX();

        //set playername to whatever was inputted inot the input field
        string playerName = nameInputField.text;

        //Add the player name string into playerName PlayerPrefs which is used in the leaderboard function
        PlayerPrefs.SetString("PlayerName", playerName);

        //save the PlayerPrefs
        PlayerPrefs.Save();
    }

    //function that toggles UI elements used during gameplay based on the inputted status
    public void GameplayUIOff(bool status)
    {
        //set the joystick movement to 0 so the player stops moving
        PlayerController.Instance.joystickMovement.joystickVec = Vector2.zero;

        //display joystick UI elements based on inputted status
        joystick.SetActive(status);

        //display game UI elements based on inputted status
        gameElements.SetActive(status);

        //display pause button UI element based on inputted status
        pauseButton.SetActive(status);
    }

    //used to play the button press SFX when a UI button has been pressed
    public void ButtonPressSFX()
    {
        AudioManager.instance.PlaySFX(0);
    }

    //used to change scenes based on an inputted scene name 
    public void ChangeScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }
}
