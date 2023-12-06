/*
 * Name: Harnish Patel
 * Student Number: 3632126
 * Course: COMP 486
 * 
 * Script Name:     ScoreboardSaveData
 * Description:     This script defines a data structure that represents the saved scoreboard data
 *                  for saving high-score entries. This means the data in this structure is a
 *                  list of ScoreboardEntryData objects.
 * 
 * Serializable Variables:
 *   - highscores: A list of ScoreboardEntryData objects representing high-score entries.
 */


using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Serializable data structure that represents the saved scoreboard data
[Serializable]
public class ScoreboardSaveData
{
    // List of high score entries
    public List<ScoreboardEntryData> highscores = new List<ScoreboardEntryData>();
}
