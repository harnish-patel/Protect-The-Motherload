/* Name: Harnish Patel
 * Student Number: 3632126
 * Course: COMP 486
 * 
 * Script Name:     ScoreboardEntryUI
 * Description:     This script is used by the HighScore Prefab to update the Text for the name
 *                  and score of the entry. 
 *
 * Functions:
 *   Initialize():
 *     Description: Sets the UI elements with the provided data for score and name.
 *     Expected Input: ScoreboardEntryData scoreboardEntryData - The data for the high-score entry.
 *     Expected Output: The UI elements are updated with the entry's name and score.
 *     Called By: Scoreboard script
 *     Will Call: None
 */

using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

// UI component responsible for displaying a single entry in the scoreboard
public class ScoreboardEntryUI : MonoBehaviour
{
    // Reference to the UI element displaying the entry name
    [SerializeField] private TextMeshProUGUI entryNameText = null;
    // Reference to the UI element displaying the entry score
    [SerializeField] private TextMeshProUGUI entryScoreText = null;

    // Initializes the UI elements with data from a scoreboard entry
    public void Initialize(ScoreboardEntryData scoreboardEntryData)
    {
        // Set the displayed name to the entry's name
        entryNameText.text = scoreboardEntryData.entryName;
        // Set the displayed score to the entry's score
        entryScoreText.text = scoreboardEntryData.entryScore.ToString();
    }
}