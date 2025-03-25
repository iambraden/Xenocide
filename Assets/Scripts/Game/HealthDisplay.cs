using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class HealthDisplay : MonoBehaviour
{
    public int health;
    public int maxHealth;

    public Sprite emptyHeart;
    public Sprite fullHeart;

    public Image[] hearts;

    public PlayerHealth playerHealth;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // Initialize health to max at start
        health = maxHealth;
    }

    // Update is called once per frame
    void Update()
    {
        //Sync max health with Player object
        health = playerHealth.health;
        maxHealth = playerHealth.maxHealth;
        UpdateHeartDisplay();
    }
    

    //Update UI when you change values in inspector (for testing)
    void OnValidate()
    {
        // Only run in edit mode or if the component is enabled ingame
        if (!Application.isPlaying || (Application.isPlaying && enabled))
        {
            // Make sure current health isn't greater than maxHealth
            if (health > maxHealth)
            {
                health = maxHealth;
            }
            
            // Update the heart display for each heart
            if (hearts != null && hearts.Length > 0)
            {
                UpdateHeartDisplay();
            }
        }
    }
    
    //Update sprites for each heart
    public void UpdateHeartDisplay()
    {
        for(int i = 0; i < hearts.Length; i++)
        {
            if(i < health)
            {
                hearts[i].sprite = fullHeart;
            }
            else
            {
                hearts[i].sprite = emptyHeart;
            }
            
            if (i < maxHealth)
            {
                hearts[i].enabled = true;
            }
            else
            {
                hearts[i].enabled = false;
            }
        }
    }
}
