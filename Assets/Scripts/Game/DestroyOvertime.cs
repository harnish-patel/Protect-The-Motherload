/* 
 * Name: Harnish Patel
 * Student Number: 3632126
 * Course: COMP 486
 * 
 * Script Name: DestroyOvertime
 * Description: this script controls how long a game object should be active before being destroyed. This is used for the
 *              death animation where the animation should only be active long enough to run once before being destroyed.
 *              To ensure this behaves consistently regardless of frame rate of the game, Time.deltaTime is used that
 *              calculates the amount of time passed between each compared frame. 
 * 
 * Functions   
 *      Update()
 *          Description:    This function runs once per frame. As mentioned in the script description, this subtracts 
 *                          Time.deltaTime from lifetime to get a consistent framerate independent timer for how long
 *                          before a game object is destroyed
 *          Expected Input:     None
 *          Expected Result:    game object lasts as long as the lifetime variable that is set in unity
 *          Called By:  Unity
 *          Will Call:  Destroy()
 *          
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyOvertime : MonoBehaviour
{
    //lifetime of the object
    public float lifetime;

    // Update is called once per frame
    void Update()
    {
        //subtract time from lifetime (framerate independent)
        lifetime -= Time.deltaTime;

        //check if lifetime is 0
        if (lifetime <= 0)
        {
            //destroy the game object
            Destroy(gameObject);
        }
    }
}
