/* 
 * Name: Harnish Patel
 * Student Number: 3632126
 * Course: COMP 486
 * 
 * Script Name: ShootLaser
 * Description: This script is used to control all laser shots within the game. it will move the laser based on shotSpeed
 *              provided. This also has a flag to set if the specific laser this script is applied to is a Player or not. 
 *              The main difference:
 *                  Player Laser - move up & check for tag "Enemy" or "Boss" when collision detected and apply damage to 
 *                                 enemy if so
 *                  Enemy Laser - move down & check for tag "Player when collision detected and apply damage to player if so      
 * 
 * Functions   
 *      Update()
 *          Description:    Called Every frame. This updates the position of the laser using MoveLaser()
 *          Expected Input:     None
 *          Expected Result:    moves the laser down based on the shotSpeed provided
 *          Called By:  Unity
 *          Will Call:  MoveLaser()
 *      
 *      OnTriggerEnter2D()
 *          Description:    This script is applied to a Player and Enemy laser Prefab that has a Collider2D.
 *                          This function is a Message of the MonoBehaviour class (built into Unity). 
 *                          When another Collider2D collides with this laser it checks the flag on isPlayer
 *                          to check if this is a player or enemy laser. If it is an enemy laser, it checks 
 *                          if other collided object has tag "Player" to signify an enemy laser hit the Player.
 *                          If so, it calls the DamagePlayer() function from the HealthManager script. 
 *                          If the flag on isPlayer is true to signify it is a player laser, it checks if the
 *                          other collided object has tag "enemy" to signify player laser hit enemy. If so, it
 *                          calls the DamageEnemy() function from EnemyController script. If the other collided object
 *                          has tag "boss" to signifying player laser hitting boss, it calls HurtBoss() in BossController
 *                          script. Regardless of which runs, the laser is destroyed.
 *          Expected Input:     The other Collider2D that is involved in collision
 *          Expected Result:    isPlayer = true && other.tag = "Enemy" - this means player laser hit enemy and enemy
 *                              is damaged
 *                              isPlayer = true && other.tag = "Boss" - this means player laser hit boss and boss is
 *                              damaged
 *                              isPlayer = true && other.tag != "Enemy" - this means player laser but did not hit
 *                              enemy 
 *                              isPlayer = false && other.tag = "Player" - this means enemy laser hit player and playr
 *                              is damaged
 *                              isPlayer = false && other.tag != "Player" - this means enemy laser but didn't hit player
 *          Called By:  Unity
 *          Will Call:  HealthManager's DamagePlayer(), EnemyController's DamageEnemy(), BossController's HurtBoss(), 
 *                      Destroy()
 *          
 *      OnBecameInvisible()
 *          Description:    This function is a Message of the MonoBehaviour class (built into Unity). When the
 *                          laser becomes invisible (move off screen), it is destroyed
 *          Expected Input:     Unity
 *          Expected Result:    When the laser is off the screen, destroy the object to save memory
 *          Called By:  Update()
 *          Will Call:  None
 *
 *      MoveLaser()
 *          Description:    This function is used to move the laser. It uses a ternary operator to set y to +ve or -ve
 *                          based on if flag isPlayer is true or false. This direction is then multiplied by the speed
 *                          to get movement (framerate independent).
 *          Expected Input:     None
 *          Expected Result:    isPlayer = true - move shot up
 *                              isPlayer = false - move shot down
 *          Called By:  Update()
 *          Will Call:  None
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootLaser : MonoBehaviour
{
    //flag to check if attached to Player or Enemy
    public bool isPlayer;
    
    //shotSpeed default set to 3f
    public float shotSpeed = 3f;

    // Update is called once per frame
    void Update()
    {
        MoveLaser();
    }

    //check if EnemyLaser collider collides with another gameobject collider.
    private void OnTriggerEnter2D(Collider2D other)
    {

        //if not Player (aka enemy)
        if (!isPlayer)
        {
            //tag system setup in Unity. Only want to check for player so the laser doesn't interact with other enemies
            if (other.tag == "Player")
            {
                //call DamagePlayer function from HealthManager script
                HealthManger.instance.DamagePlayer();
            }
            //Destroy Enemylaser
            Destroy(this.gameObject);
        }
        //is Player
        else
        {
            //tag system setup in Unity. Only want to check for enemy so laser only interacts with enemies
            if (other.tag == "Enemy")
            {
                //call DamageEnemy from EnemyController script
                other.GetComponent<EnemyController>().DamageEnemy();
            }
            
            //check for boss so laser only interacts with boss
            if (other.tag == "Boss")
            {
                BossController.instance.HurtBoss();
            }

            //destroys laser
            Destroy(this.gameObject);
        }
    }

    //is enemy laser moves off screen, destroy it to save memory
    void OnBecameInvisible()
    {
        Destroy(this.gameObject);
    }

    //moves the laser down based on a rate deteremined by shotSpeed
    private void MoveLaser()
    {
        //set direction by using a ternary operator. If isPlayer, y is +ve (go up), and if !isPlayer, y is -ve (go down)
        Vector3 direction = isPlayer ? Vector3.up : Vector3.down;

        //move laser by getting direction of movement and speed
        transform.position += direction * shotSpeed * Time.deltaTime;
    }

}