using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class Health : MonoBehaviour
{
    public int maxHealth = 100;
    private int currentHealth;

    public Slider slider;
    private PlayerRagdoll playerRag;
    private ThirdPersonController playerController;

    void Start()
    {
        currentHealth = maxHealth;
        playerRag = GetComponent<PlayerRagdoll>();
        playerController = GetComponent<ThirdPersonController>();
        if(this.CompareTag("Player"))  slider.value = currentHealth;
           
        
        
    }

    public void TakeDamage(int amount)
    {
        currentHealth -= amount;
        if (currentHealth <= 0)
        {
            Die();
            if(this.CompareTag("Player")) UpdateHealthUI();
                
            
            
        }
        if(this.CompareTag("Player")) UpdateHealthUI();

    }

    void UpdateHealthUI()
    {
        slider.value = currentHealth;
    }

    void Die()
    {
        if(this.CompareTag("Player")){
            playerRag.RagdollON();
            playerController.enabled = false;
        } else{
            Destroy(gameObject);
        }
        
    }
}
