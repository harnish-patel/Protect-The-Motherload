/* 
 * Name: Harnish Patel
 * Student Number: 3632126
 * Course: COMP 486
 * 
 * Script Name: BackgroundScroller
 * Description: This script is used to scroll the 2 background imgs used for parallax effect down. 
 *              The rate it is moved down by is determined by the set parallaxSpeed parameter and this is
 *              framerate independent
 *              
 *              Additionally, once an img moves down where the duplicate img is in the original img's spot
 *              the img is moved up twice it's height so it is now seamlessly above the duplicate img.
 *              This is repeated indefinately to create the parallax effect
 * 
 * Functions
 *      Start()
 *          Description:    Retrieves the height of the background img from the BackgroundScaler script
 *                          This will not update as the code runs so only needs to be called once
 *          Expected Input:     None
 *          Expected Result:    get spriteHeight
 *          Called By:  Unity
 *          Will Call:  None
 *      
 *      Update()
 *          Description:    Called Every frame. This updates position of the background imgs used for
 *                          the parallax effect. Additionally if an img moves outside its bounds, its
 *                          position is updated (Explained in BackgroundScroller Description)
 *          Expected Input:     None
 *          Expected Output:    Each time update is run, imgs for the parallax effect are moved down & are repositioned
 *                              if moved outside bound
 *          Called By:  Unity
 *          Will Call:  ScrollBackground(), CheckBounds()
 *      
 *      ScrollBackground()
 *          Description:    This function will scroll the img down by a rate set by parallaxSpeed. The scrolling
 *                          is framerate independent
 *          Expected Input:     the Transform for an img (in our implementation it will be the original and duplicate
 *                              background imgs used in the parallax effect)
 *          Expected Result:    imgs scroll down based on parallaxSpeed that was set
 *          Called By:  Update()
 *          Will Call:  None
 *          
 *      CheckBounds()
 *          Description:    This function checks if the img has moved down its entire height. If so, it will
 *                          move twice its height above its current position. This new position will be seamlessly
 *                          above the other img used in the parallax effect
 *          Expected Input:     the Transform for an img (in our implementation it will be the original and duplicate
 *                              background imgs used in the parallax effect)
 *          Expected Result:    When img moves down its height, it will be moved twice its height above making it seamlessly
 *                              above the img that is above it
 *          Called By:  Update()
 *          Will Call:  None
 */


using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BackgroundScroller : MonoBehaviour
{
    //references to transforms for background images
    public Transform BG, BG_Duplicate;
    
    //speed of parallax effect
    public float parallaxSpeed;
    
    //height of sprite
    private float spriteHeight; 


    void Start()
    {
        //get height of sprite from BackgroundScaler script
        spriteHeight = BackgroundScaler.spriteHeight;
    }

    // Update is called once per frame
    void Update()
    {
        //Scroll both imgs
        ScrollBackground(BG);
        ScrollBackground(BG_Duplicate);

        //check if imgs outside bounds
        CheckBounds(BG);
        CheckBounds(BG_Duplicate);
    }

    //Move Background image down at a particular rate for parallax effect
    void ScrollBackground(Transform img)
    {
        //Time.deltaTime used to make movement framerate independent
        img.position += new Vector3(0f, -parallaxSpeed * Time.deltaTime, 0f);
    }

    //Checks if the background img has moved off screen and if so, moves it above the other background img
    void CheckBounds(Transform img)
    {
        //check if img has moved down the entirety of its height. -1 used as a buffer just to ensure img is fully off screen
        if (img.position.y < -spriteHeight - 1) 
        {
            //move the sprite up twice the height so it is positioned seamlessly above the other background img
            img.position += new Vector3(0f, spriteHeight * 2f, 0f);
        }
    }
}
