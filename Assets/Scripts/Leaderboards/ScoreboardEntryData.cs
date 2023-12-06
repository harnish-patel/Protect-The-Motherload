/* Name: Harnish Patel
 * Student Number: 3632126
 * Course: COMP 486
 * 
 * 
 * Script Name:     ScoreboardEntryData
 * Description:     This script defines a data structure representing a single high-score entry.
 *                  The Fields include the name and score of the entry.
 * 
 * Serializable Variables:
 *   - entryName: string for name used for scoreboard entry
 *   - entryScore: int for score used for scoreboard entry
 */

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Serializable struct to represent a single entry in the scoreboard
[Serializable]
public struct ScoreboardEntryData
{
    // String for entry name
    public string entryName;

    // Int for entry score
    public int entryScore;
}