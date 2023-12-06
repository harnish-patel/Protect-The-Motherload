/* 
 * Name: Harnish Patel
 * Student Number: 3632126
 * Course: COMP 486
 * 
 * Script Name: HealthManager
 * Description: this script handles the player health system. This includes taking damage, being killed, activating
 *              shields, etc.
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
 *      Start()
 *          Description:    This runs before the first frame update. In this function, it initializes variables that
 *                          will be used and sets the initial value of the UI health bar
 *          Expected Input: None
 *          Expected Result:    Variables setup and the UI for the health bar is maxed at startup
 *          Called By:  Unity
 *          Will Call: None
 *  
 *      DamagePlayer()
 *          Description:    This function handles when the player takes damage. It checks to ensure the shield and
 *                          invinsible flag is false. If so, it plays the player hit SFX and decrements the current
 *                          health. If the current health falls to 0 (meaning player dead), the player dead SFX plays,
 *                          the death animation is triggered, the Player gameobject is removed, and the GameComplete UI
 *                          function is called. 
 *          Expected Input:     None
 *          Expected Result:    Player health decreases and health bar UI updates. If health drops to 0, death effect is played
 *          Called By:  DamagePlayer's OnCollisionEnter2D(), EnemyController's OnCollisionEnter2D(), ShootLaser's 
 *                      OnCollisionEnter2D(),    
 *          Will Call:  AudioManager's PlaySFX(), UIController's GameComplete(), Instantiate()
 *
 *      HealPlayer()
 *          Description:    This function handles healing the player. It does this by incrementing the current health
 *                          by 1 and updating the health bar UI element to show these changes
 *          Expected Input:     None
 *          Expected Result:    Player health increases, healthbar UI element updated
 *          Called By:  GameController's HealthBoost
 *          Will Call:  None
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthManger : MonoBehaviour
{
    public static HealthManger instance;

    //declaration for current and max health
    public int currHealth, maxHealth;

    //particle effect made using Unity particle effect
    public GameObject deathEffect;

    //if level is over, make player invinsible (makes sure left over shots don't kill player and bug game)
    public bool invinsible;

    //This is initialized before the game starts
    private void Awake()
    {
        instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        //set invinsible flag to false when starting
        invinsible = false;

        //set inital value of health
        currHealth = maxHealth;

        //set max and current value of the UI healthbar
        UIController.Instance.healthBar.maxValue = maxHealth;
        UIController.Instance.healthBar.value = currHealth;
    }

    //handle Player taking damage
    public void DamagePlayer()
    {
        //if shield is not active and invinsible flag is false
        if (!GameController.Instance.playerShield.activeInHierarchy && !invinsible)
        {
            //play SFX for player hit
            AudioManager.instance.PlaySFX(4);

            //reduce health and update UI to visually show change
            currHealth--;
            UIController.Instance.healthBar.value = currHealth;

            //if current health is 0 (aka dead)
            if (currHealth <= 0)
            {   
                //play SFX for player dead
                AudioManager.instance.PlaySFX(5);

                //spawn the death particle effect
                Instantiate(deathEffect, transform.position, transform.rotation);
                
                //turn off Player gameObject
                gameObject.SetActive(false);

                //start GameOver() function from UI Controller
                UIController.Instance.GameComplete();
            }
        }
    }

    //handles healing player
    public void HealPlayer()
    {
        //increment current health and update UI to visually show change
        currHealth++;
        UIController.Instance.healthBar.value = currHealth;
    }
}
