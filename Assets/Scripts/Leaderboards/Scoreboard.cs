/*
 * Name: Harnish Patel
 * Student Number: 3632126
 * Course: COMP 486
 * 
 * Script Name: Scoreboard
 * Description: This script handles the leaderboard system in the game. The high scores are saved in JSON format
 *              and will allow new scores to be inputted. If there are less than 5 scores in the leaderboard, it
 *              is added, but will then be displaced when higher scores are recorded.
 *
 * Functions:
 *   Start():
 *     Description: Initializes the script by loading saved high scores and updating the UI.
 *     Expected Input: None
 *     Expected Output: The high-score data is loaded and the UI is updated.
 *     Called By: Unity
 *     Will Call: GetSavedScores(), UpdateUI(), AddEntry(), SaveScores()
 *
 *   AddEntry():
 *     Description: Adds a new high-score entry to the leaderboard.
 *     Expected Input: ScoreboardEntryData scoreboardEntryData - The data for the new high-score entry.
 *     Expected Output: The new entry is added to the leaderboard, and excess entries are removed if needed.
 *     Called By: Start()
 *     Will Call: GetSavedScores(), SaveScores(), UpdateUI()
 *
 *   UpdateUI():
 *     Description: Updates the UI to display the high-score entries.
 *     Expected Input: ScoreboardSaveData savedScores - The high-score data to display.
 *     Expected Output: The UI is updated with the high-score entries.
 *     Called By: Start(), AddEntry()
 *     Will Call: None
 *
 *   GetSavedScores():
 *     Description: Retrieves the saved high-score data from PlayerPrefs.
 *     Expected Input: None
 *     Expected Output: The saved high-score data is returned.
 *     Called By: Start(), AddEntry()
 *     Will Call: None
 *
 *   SaveScores(ScoreboardSaveData scoreboardSaveData):
 *     Description: Saves the high-score data to PlayerPrefs in JSON format.
 *     Expected Input: ScoreboardSaveData scoreboardSaveData - The high-score data to save.
 *     Expected Output: The high-score data is saved in PlayerPrefs.
 *     Called By: AddEntry()
 *     Will Call: None
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.SceneManagement;

public class Scoreboard : MonoBehaviour
{
    [SerializeField] private int maxScoreboardEntries = 5;
    [SerializeField] private Transform highscoresHolderTransform;
    [SerializeField] private GameObject scoreboardEntryObject;

    private void Start()
    {
        // Load saved scores and update the UI
        ScoreboardSaveData savedScores = GetSavedScores();
        UpdateUI(savedScores);

        // Retrieve and log player's saved name and score
        Debug.Log("Player Name Saved: " + PlayerPrefs.GetString("PlayerName"));
        Debug.Log("Player Score Saved: " + PlayerPrefs.GetInt("PlayerScore"));

        // Create a scoreboard entry for the player and add it if a valid score is saved
        ScoreboardEntryData playerEntryData = new ScoreboardEntryData
        {
            entryName = PlayerPrefs.GetString("PlayerName"),
            entryScore = PlayerPrefs.GetInt("PlayerScore")
        };

        if (!(PlayerPrefs.GetInt("PlayerScore") == 0))
        {
            AddEntry(playerEntryData);
            Debug.Log("Updated UI");

            // Clear saved player name and score
            Debug.Log("Player Name Saved: " + PlayerPrefs.GetString("PlayerName"));
            Debug.Log("Player Score Saved: " + PlayerPrefs.GetInt("PlayerScore"));
        }

        PlayerPrefs.DeleteKey("PlayerName");
        PlayerPrefs.DeleteKey("PlayerScore");
        PlayerPrefs.Save();
    }

    public void AddEntry(ScoreboardEntryData scoreboardEntryData)
    {
        // Load saved scores
        ScoreboardSaveData savedScores = GetSavedScores();

        bool scoreAdded = false;

        // Insert the new entry into the correct position based on the score
        for (int i = 0; i < savedScores.highscores.Count; i++)
        {
            if (scoreboardEntryData.entryScore > savedScores.highscores[i].entryScore)
            {
                savedScores.highscores.Insert(i, scoreboardEntryData);
                scoreAdded = true;
                break;
            }
        }

        // If the score couldn't be inserted and the maximum number of entries isn't reached, add it at the end
        if (!scoreAdded && savedScores.highscores.Count < maxScoreboardEntries)
        {
            savedScores.highscores.Add(scoreboardEntryData);
        }

        // Remove excess entries if the maximum number is exceeded
        if (savedScores.highscores.Count > maxScoreboardEntries)
        {
            savedScores.highscores.RemoveRange(maxScoreboardEntries, savedScores.highscores.Count - maxScoreboardEntries);
        }

        // Save the updated scores and update the UI
        SaveScores(savedScores);
        UpdateUI(savedScores);
    }

    private void UpdateUI(ScoreboardSaveData savedScores)
    {
        Debug.Log("Starting Update UI");

        // Clear existing UI entries
        foreach (Transform child in highscoresHolderTransform)
        {
            Destroy(child.gameObject);
        }

        // Instantiate new UI entries for each highscore
        foreach (ScoreboardEntryData highscore in savedScores.highscores)
        {
            Instantiate(scoreboardEntryObject, highscoresHolderTransform).GetComponent<ScoreboardEntryUI>().Initialize(highscore);
        }
    }

    private ScoreboardSaveData GetSavedScores()
    {
        // Check if there are saved highscores and load them if available
        if (PlayerPrefs.HasKey("Highscores"))
        {
            string json = PlayerPrefs.GetString("Highscores");
            Debug.Log("json from PlayerPref: " +json);
            Debug.Log("return val: " + JsonUtility.FromJson<ScoreboardSaveData>(json));
            return JsonUtility.FromJson<ScoreboardSaveData>(json);
        }
        else
        {
            // Return an empty scoreboard data if no saved data is found
            return new ScoreboardSaveData();
        }
    }

    private void SaveScores(ScoreboardSaveData scoreboardSaveData)
    {
        // Serialize the scoreboard data to JSON and save it to PlayerPrefs
        string json = JsonUtility.ToJson(scoreboardSaveData);
        PlayerPrefs.SetString("Highscores", json);
        PlayerPrefs.Save();
    }
}