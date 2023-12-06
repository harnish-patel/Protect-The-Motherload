/* 
 * Name: Harnish Patel
 * Student Number: 3632126
 * Course: COMP 486
 * 
 * Script Name: AudioManager
 * Description: This script manages audio within the game, including playing specific sound effects (SFX), muting, 
 *              and unmuting all SFX.
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
 *      PlaySFX()
 *          Description:    This function plays a sound clip based on the inputted array index
 *          Expected Input:     Index of the sound effect to play.
 *          Expected Result:    The specified sound effect is played.
 *          Called By:  Various functions in EnemyController, DropController, GameController, HealthManager, WaveController,
 *                      PlayerController, and UIController
 *          Will Call:  AudioSource.Play().
 *
 *      MuteSounds()
 *          Description:    This function mutes all sound effects.
 *          Expected Input:     None
 *          Expected Result:    All sound effects are muted
 *          Called By:  UIController's ToggleSFX
 *          Will Call:  None
 *
 *      UnMuteSounds()
 *          Description:    This function unmutes all sound effects.
 *          Expected Input:     None
 *          Expected Result:    All sound effects are unmuted
 *          Called By:  UIController's ToggleSFX
 *          Will Call:  None
 */

using JetBrains.Annotations;
using Mono.CompilerServices.SymbolWriter;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    //only 1 instance of AudioManager in Scene therefore we use this to always reference the same AudioManager
    public static AudioManager instance;

    //array of audio clips
    public AudioSource[] audioSource;

    //This is initialized before the game starts
    private void Awake()
    {
        instance = this;
    }

    //handles playing specific SFX based on array index
    public void PlaySFX(int sound)
    {
        audioSource[sound].Play();
    }

    //handles muting all SFX 
    public void MuteSounds()
    {
        foreach (AudioSource source in audioSource)
        {
            source.mute = true;
            Debug.Log("souce muted");
        }
    }

    //handles unmuting all SFX
    public void UnMuteSounds()
    {
        foreach (AudioSource source in audioSource)
        {
            source.mute = false;
            Debug.Log("souce unmuted");
        }
    }
}
