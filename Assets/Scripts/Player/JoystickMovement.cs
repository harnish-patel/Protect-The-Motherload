/* 
 * Name: Harnish Patel
 * Student Number: 3632126
 * Course: COMP 486
 * 
 * Script Name: JoystickMovement
 * Description: This script is used to control the joystick that will be used for player movement. In this script, 
 *              we write the logic that changed the position of the joystick and joystick bg to the position 
 *              that the user presses on the screen. Then based on where the user drags their finger on the screen, 
 *              the joystick img will move to follow the finger while ensuring that it remains within the joystick
 *              background img. Finally, when the user lifts their finger, the joystick and joystick background imgs
 *              return to their original positions
 *              
 * 
 * Functions
 *      Start()
 *          Description:    This gets the joystick original position and size. It also sets the joystickRadius
 *                          which will be used later to ensure the joystick remains within the joystick background img    
 *          Expected Input:     None
 *          Expected result:    set original position of joystick and radius joystick can move within background img     
 *          Called By:  None         
 *          Will Call:  None
 *          
 *      PointerDown()
 *          Description:    This function is called whent he user presses their finger on the screen. When called,
 *                          it chagnes the joystick and joystick background position to the position that the user
 *                          touched
 *          Expected Input:     User pressing down on screen
 *          Expected result:    Joystick and joystick background move to location user press down 
 *          Called By: User input         
 *          Will Call: None
 *
 *      Drag()
 *          Description:    This moves the joystick and the joystick vector based on how the user drags their finger
 *                          across the screen. It ensures that the joystick stays within the joystick background.
 *          Expected Input:     A BaseEventData
 *          Expected result:    Moves the joystick based on where user drags finger, but if the vector is outside the bounds
 *                              of the calulcated joystickRadius (based on joystick background), the joystick will remain
 *                              at the edge of the radius. Additionally, the joystickVec is also calculated
 *          Called By: user Input       
 *          Will Call: None
 *
 *      PointerUp()
 *          Description:    This function is called when the user lifts their finger off the screen. When called, 
 *                          it changes the joystick and joystick background position to the original position
 *          Expected Input:     User pressing down on screen
 *          Expected result:    Set joystick and joystick background to original position        
 *          Called By: User Input         
 *          Will Call: None
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class JoystickMovement : MonoBehaviour
{
    //joystick visual assets
    public GameObject joystick; 
    public GameObject joystickBG;
    
    //variables used
    public Vector2 joystickVec;
    private Vector2 joystickTouchPos;
    private Vector2 joystickOriginalPos;
    private float joystickRadius;

    // Start is called before the first frame update
    void Start()
    {
        //get the joystick original position and size
        joystickOriginalPos = joystickBG.transform.position;
        //divided by 4 is arbitrary. This was found to work well but it's purpose is to make a smaller radius
        //that the joystick can move within the joystick background
        joystickRadius = joystickBG.GetComponent<RectTransform>().sizeDelta.y / 4;
    }

    //Moves joystick and joystick background to the where the user touches the screen
    public void PointerDown() {
        joystick.transform.position = Input.mousePosition;
        joystickBG.transform.position = Input.mousePosition;
        joystickTouchPos = Input.mousePosition;
    }

    //Moves joystick and the joystick vec based on user dragging input while 
    //ensuring it stays within background image
    public void Drag(BaseEventData baseEventData) 
    {
        //cast BaseEventData parameter to PointEventData object to get position of pointer
        PointerEventData pointerEventData = baseEventData as PointerEventData;
        
        //get position of the pointer
        Vector2 dragPos = pointerEventData.position;
        
        //check direction joystick is being moved and normalize it so only direction is important
        joystickVec = (dragPos - joystickTouchPos).normalized;

        //calculate distance between the 2 Vector2 dragPos and joystickTouchPos
        float joystickDist = Vector2.Distance(dragPos, joystickTouchPos);

        //this ensures joystick doesn't move outside of calculated joystick radius (based on the joystickbackground)
        //if joystick distance moved is less than the radius
        if(joystickDist < joystickRadius) 
        {
            //set the joystick position to be the current point position
            joystick.transform.position = joystickTouchPos + joystickVec * joystickDist;
        }
        else
        {
            //set joystick to be at the edge of the joystick background
            joystick.transform.position = joystickTouchPos + joystickVec * joystickRadius;
        }
    }

    //when user lifts finger up, reset position of joystick and joystick background to original position
    public void PointerUp(){
        joystickVec = Vector2.zero;
        joystick.transform.position = joystickOriginalPos;
        joystickBG.transform.position = joystickOriginalPos;
    }
}
