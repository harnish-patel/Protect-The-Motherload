/* 
 * Name: Harnish Patel
 * Student Number: 3632126
 * Course: COMP 486
 * 
 * Script Name: MovingObjects
 * Description: this script controls the movement of objects. Currently it is setup to only go down at a rate of moveSpeed
 *              and is used for the shield drop. The code also includes a rotation feature that was being tested out. The
 *              functionality works but the feature may be used for the next version of the game.
 * 
 * Functions   
 *      Update()
 *          Description:    This function runs each time frame is refreshed. It calls the MoveObject function to handle
 *                          object movement. Additionally, there is a feature for rotating the object that is implemented
 *                          but this feature was removed for this current version of the game.
 *          Expected Input:     None
 *          Expected Result:    Move object down by a rate of moveSpeed while being framerate independent
 *          Called By:      Unity
 *          Will Call:      MoveObject()
 *          
 *      OnBecameInvisible()
 *          Description:    This function is a Message of the MonoBehaviour class (built into Unity). When Object is
 *                          off the screen, the object is destroyed to save memory
 *          Expected Input:     None
 *          Expected result:    When object is off screen, destroy it to save memory
 *          Called By:  Unity
 *          Will Call:  Destroy()
 *          
 *      MoveObject()
 *          Description:    This function is used to move the object down by a particular speed set by moveSpeed. This
 *                          movement is framerate independent.
 *          Expected Input:     None
 *          Expected Result:    move object down particular speed
 *          Called By:      Update()
 *          Will Call:      None
 *          
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingObjects : MonoBehaviour
{
    //feature removed for current version
    public bool isRotating;
    public float rotateSpeed;
    
    //speed to set how fast object will move
    public float moveSpeed;

    // Update is called once per frame
    void Update()
    {
        //move object using MoveObject() function
        MoveObject();

        //feature removed for this version but functionality still exists
        if (isRotating){
            transform.eulerAngles -= new Vector3(0f, 0f, rotateSpeed * Time.deltaTime);
        }
    }

    //Destroy object when moved off screen to save memory
    private void OnBecameInvisible(){
        Destroy(this.gameObject);
    }

    //function to handle object movement (current set to always go down)
    //is framerate independent
    private void MoveObject()
    {
        transform.position -= new Vector3(0f, moveSpeed * Time.deltaTime, 0f);
    }
}
