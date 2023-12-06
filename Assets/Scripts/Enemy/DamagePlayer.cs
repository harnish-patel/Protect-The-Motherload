/* 
 * Name: Harnish Patel
 * Student Number: 3632126
 * Course: COMP 486
 * 
 * Script Name: EnemyController
 * Description: This script is attached to the Boss Game Object and handles collision damage with Player 
 * 
 * Functions   
 *      OnCollisionEnter2D
 *          Description:    The OnCollisionEnter2D is a parameter of MonoBehaviour from Unity. This is called when the object's
 *                          collider makes contact with another object's collider. In this implementation, we check if the other
 *                          object has tag "Player", if so, we call DamagePlayer from HealthManager script to damage the player.
 *          Expected Input:     The other Collider2D that is involved in collision
 *          Expected result:    If the other collided object is the Player (tag = "Player"), damge the Player. If it's not the
 *                              player, do nothing. 
 *          Called By:  Unity
 *          Will Call: HealthManager's DamagePlayer().
 */


using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AdaptivePerformance.VisualScripting;

public class DamagePlayer : MonoBehaviour
{
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            HealthManger.instance.DamagePlayer();
        }
    }
}
