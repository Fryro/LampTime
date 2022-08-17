using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
//using TMPro;

public class PlayerResources : MonoBehaviour
{
    public Image healthBar;
    public Text healthText;
    //public TextMeshPro healthText;

    public float maxHealth;
    public float health;
    public float guardMultiplier;
    public bool guarding;

    // Start is called before the first frame update
    void Start()
    {
        maxHealth = 100.0f;
        health = 100.0f;
        guardMultiplier = 0.2f;
        guarding = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (health <= 0)
        {
            SceneManager.LoadScene("Tutorial");
        }

        if (Input.GetKeyDown(KeyCode.E))
        {
            TakeDamage(20.0f);
        }
        
        if (Input.GetKeyDown(KeyCode.Q))
        {
            Heal(20.0f);
        }
    }

    public void TakeDamage(float incoming)
    {
        if (guarding)
        {
            incoming *= guardMultiplier;
        }
        health -= incoming;
        healthBar.fillAmount = health / 100.0f;
        healthText.text = ("Health: " + health);
        //healthText.SetText("Health: " + health);
    }

    public void Heal(float incoming)
    {
        health += incoming;
        health = Mathf.Clamp(health, 0.0f, maxHealth);
        healthBar.fillAmount = health / 100.0f;
        healthText.text = ("Health: " + health);
        //healthText.SetText("Health: " + health);
    }
}