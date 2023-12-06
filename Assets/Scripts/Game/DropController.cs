/* 
 * Name: Harnish Patel
 * Student Number: 3632126
 * Course: COMP 486
 * 
 * Script Name: DropController
 * Description: this script controls drops that are spawned within the game. This script checks if this object collides with
 *              other object colliders. If so and if the other object has tag = "Player", this means the drop has collided
 *              with the Player. It then destroys the drop and activates its effect. Currently there are 4 different types of
 *              drops: Shield, Overdrive, Frenzy and Health.
 * 
 * Functions   
 *      OnTriggerEnter2D(Collider2D other)
 *          Description:    This script is applied to the Shield drop prefab that has a Collider2D. This function
 *                          is a Message of the MonoBehaviour class (built into Unity). When another Collider2D collides
 *                          with this drop, it checks if the tag of the other object is "Player" which signifies that the
 *                          Player collided with the drop. If so, it destroys the drop and activates the effect based on the flag
 *                          which specifics which specific drop it is. 
 *          Expected Input:     The other Collider2D that is involved in collision
 *          Expected Result:    other.tag = "Player" - check which drop based on flag and run logic from GameController script
 *                              other.tag != "Player" - do nothing
 *          Called By:  Unity
 *          Will Call:  AudioManager's PlaySFX, Destroy(), GameController's ShieldActive(), OverdriveActive(), FrenzyActive()
 *                      and HealthBoost()
 *          
  *      OnBecameInvisible()
 *          Description:    This function is a Message of the MonoBehaviour class (built into Unity). When the
 *                          drop becomes invisible (move off screen), it is destroyed
 *          Expected Input:     Unity
 *          Expected Result:    When the drop is off the screen, destroy the object to save memory
 *          Called By:  Update()
 *          Will Call:  None
 *          
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropController : MonoBehaviour
{
    //flags for which type of boost the drop is
    public bool isShield, isOverdrive, isFrenzy, isHealth;

    //if object collider collides with another object collider
    private void OnTriggerEnter2D(Collider2D other)
    {
        //check if other object collider has tag "Player"
        if (other.tag == "Player")
        {
            //play drop pickup
            AudioManager.instance.PlaySFX(8);

            //Destroy this object
            Destroy(this.gameObject);

            //check if drop is shield
            if (isShield)
            {
                //Run shieldActive function from the GameController script
                GameController.Instance.ShieldActive();
            }

            //check if drop is overdrive
            else if (isOverdrive)
            {
                //run OverdriveActive from GameController script
                GameController.Instance.OverdriveActive();
            }

            //check if drop is frenzy
            else if (isFrenzy)
            {
                //run FrenzyActive from GameController script
                GameController.Instance.FrenzyActive();
            }

            //check if drop is health
            else if (isHealth)
            {
                //run HealthBoost from GameController script
                GameController.Instance.HealthBoost();
            }
        }
    }

    //is drop moves off screen, destroy it to save memory
    void OnBecameInvisible()
    {
        Destroy(this.gameObject);
    }
}
