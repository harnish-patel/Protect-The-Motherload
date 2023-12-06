/* 
 * Name: Harnish Patel
 * Student Number: 3632126
 * Course: COMP 486
 * 
 * Script Name: BackgroundScaler
 * Description: This script is used to setup the background imgs that will be used for parallax scrolling.
 *              The program will scale the the img so that its width matches the screen width and then
 *              scales the img height to maintain the original ratio
 *              
 *              Additionally, it will move the duplicate background img so that it is seamlessly above
 *              the original img
 *              
 * 
 * Functions
 *      Start(): called before the first frame update (aka starts right away when scene opens)
 *          Description:    Scales the background img to the screensize. Here the img width is adjusted to 
 *                          match the screenwidth and the img height is scaled according to the new width
 *                          to ensure the original ratios are maintained
 *                      
 *                          Additionally, the duplicate img used in the parallax effect is positioned 
 *                          seamlessly above the original img.            
 *          Expected Input:     None
 *          Expected Output:    When program ran, both background imgs for parallax effect will be scaled according
 *                              to screen size and the duplicate will position seamlessly above original bg img          
 *          Called By: Unity         
 *          Will Call: None        
 *          Further Explanations:
 *                  screenWidth:    mainCamera.orthographicSize is half-height of camera view in world units 
 *                                  so 2f is used to convert to full-height
 *                                  mainCamera.aspect is width/heigh for camera view so this is used to 
 *                                  calculate the full width of view
 *                              
 *                  scaleFactor:    calculates the ratio between screen and sprite width which will be used later to
 *                                  transform the img to the correct width based on screen size
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class BackgroundScaler : MonoBehaviour
{

    static public float spriteHeight;

    //variables for the Camera and sprite renderer
    private Camera mainCamera;
    private SpriteRenderer spriteRenderer;

    //flag to indicate duplicate img
    public bool duplicate;

    // Start is called before the first frame update
    void Start()
    {
        //reference to main Camera
        mainCamera = Camera.main;

        //reference to sprite renderer
        spriteRenderer = GetComponent<SpriteRenderer>();
        
        //calculate width of screen
        float screenWidth = mainCamera.orthographicSize * 2f *mainCamera.aspect;
        
        //retrieve width of sprite from SpriteRenderer (component from Unity)
        float spriteWidth = spriteRenderer.sprite.bounds.size.x;

        //calculate the factor to which img has been scaled
        float scaleFactor = screenWidth / spriteWidth;
        
        //scales the transform of the img so the img width matches the screen width, but also maintains
        //the original ratios of the image
        transform.localScale = new Vector3(scaleFactor, scaleFactor, 1f); 

        //adjust the starting height of the duplicate transformed img so it is seamless
        if(duplicate)
        {
            //get the height of the transformed img
            spriteHeight = spriteRenderer.sprite.bounds.size.y * scaleFactor;

            //move the duplicate img up the exact amount as the height of the transformed img so it is seamless
            transform.position = new Vector3(transform.position.x, spriteHeight, transform.position.z);
        }
    }
}
