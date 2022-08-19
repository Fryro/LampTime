using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PlayerLantern : MonoBehaviour
{

    private float timeNow;
    private float timeAnchor;

    [SerializeField] private PlayerResources player;
    [SerializeField] private PlayerHitbox playerWeapon;
    [SerializeField] private UnityEngine.Rendering.Universal.Light2D lantern;
    [SerializeField] private GameObject lightParent;
    [SerializeField] private Image healthBar;
    [SerializeField] private Text healthText;
    [SerializeField] private PauseMenu pauseMenuController;
    private UnityEngine.Rendering.Universal.Light2D childLight;
    private UnityEngine.Rendering.Universal.Light2D ambientLight;

    private float healthNow;
    private float healthAnchor;
    private float maxHealth;
    private float oldHealthPercent;
    private float healthPercent;

    private float iFrames;
    private float updateTime = 0.1f; // Only use numbers that go evenly to 1; I.E; 0.5, 0.1, 0.05.
    private float timeStep;
    private float iterations;

    private float lanternBrightness;
    private float torchBrightness;
    private float ambientBrightness;

    private float brightnessDifference;

    //private float deathDarknessMultiplier;
    public bool dying;

    // Start is called before the first frame update
    void Start()
    {
        iFrames = 0.75f;
        timeNow = Time.time;
        timeAnchor = Time.time;

        healthNow = player.health;
        healthAnchor = player.health;
        maxHealth = player.maxHealth;
        healthPercent = healthNow / maxHealth;

        lanternBrightness = 1.0f;
        torchBrightness = 0.5f;
        ambientBrightness = 0.05f;

        //deathDarknessMultiplier = 0.0f;
        dying = false;

        ambientLight = GameObject.Find("Ambient Light").GetComponent<UnityEngine.Rendering.Universal.Light2D>();
    }

    // Update is called once per frame
    void Update()
    {
        timeNow = Time.time;
        healthNow = player.health;
        healthPercent = healthNow / maxHealth;
        oldHealthPercent = healthAnchor / maxHealth;

        if (playerWeapon.attacking)
        {
            return;
        }
        else if (dying)
        {
            // FadeLight(lantern, lanternBrightness*oldHealthPercent, lanternBrightness*0.0f, 10.0f, 0.0f);
            // FadeLight(ambientLight, ambientBrightness*oldHealthPercent, ambientBrightness*0.0f, 10.0f, 0.00f);
            // foreach (Transform item in lightParent.transform)
            // {
            //     childLight = item.GetComponent<UnityEngine.Rendering.Universal.Light2D>();
            //     FadeLight(childLight, torchBrightness*oldHealthPercent, torchBrightness*0.0f, 10.0f, 0.0f);
            // }
            StartCoroutine(DeathFade(lantern.intensity, childLight.intensity, ambientLight.intensity));
            StartCoroutine(player.EndGame());
            return;
        }
        else
        {
            // FadeLight(lantern, lanternBrightness*oldHealthPercent, lanternBrightness*healthPercent, iFrames, 0.5f);
            // FadeLight(ambientLight, ambientBrightness*oldHealthPercent, ambientBrightness*healthPercent, iFrames, 0.05f);
            // foreach (Transform item in lightParent.transform)
            // {
            //     childLight = item.GetComponent<UnityEngine.Rendering.Universal.Light2D>();
            //     FadeLight(childLight, torchBrightness*oldHealthPercent, torchBrightness*healthPercent, iFrames, 0.5f);
            // }
            lantern.intensity = lanternBrightness * healthPercent + 0.5f;
            ambientLight.intensity = ambientBrightness * healthPercent;
            foreach (Transform item in lightParent.transform)
            {
                childLight = item.GetComponent<UnityEngine.Rendering.Universal.Light2D>();
                childLight.intensity = torchBrightness * healthPercent + 0.5f;
            }
        }

        healthAnchor = healthNow;
        //print("Lantern: " + lantern.intensity + "\tTorches: " + childLight.intensity + "\tAmbient: " + ambientLight.intensity);

        if (healthNow <= 0)
        {
            dying = true;
        }

    }
    //     // If the player's health has changed between frames,
    //     if (healthNow != healthAnchor)
    //     {
    //         // Lower lantern brightness proportional to change in health percentage.
    //         StartCoroutine(ChangeBrightness(lanternBrightness * oldHealthPercent, lanternBrightness * healthPercent, iFrames, lantern));
    //         // Do the same for all objects that are children of the "Lights" GameObject.
    //         foreach (Transform item in lightParent.transform)
    //         {
    //             childLight = item.GetComponent<UnityEngine.Rendering.Universal.Light2D>();
    //             StartCoroutine(ChangeBrightness(torchBrightness * oldHealthPercent, torchBrightness * healthPercent, iFrames, childLight));
    //         }
    //         // Do the same for the Ambient Light object.
    //         StartCoroutine(ChangeBrightness(ambientBrightness * oldHealthPercent, ambientBrightness * healthPercent, iFrames, ambientLight));
    //         healthAnchor = healthNow;
    //     }
    //     print(lantern.intensity);
    // }

    // Method that takes many parameters,
    // 1. Old Brightness Value.
    // 2. New Brightness Value.
    // 3. Time to make the transition in.
    // 4. The light object that is being changed.
    // private IEnumerator ChangeBrightness(float oldBrightness, float newBrightness, float timeFrame, UnityEngine.Rendering.Universal.Light2D light)
    // {
    //     print("Changing Brightness from: " + oldBrightness + " to: " + newBrightness + "\tOver the course of: " + timeFrame + " seconds.");
    //     timeStep = timeFrame * updateTime;
    //     iterations = 1.0f / updateTime;
    //     brightnessDifference = oldBrightness - newBrightness;
    //     light.intensity = oldBrightness;

    //     for (float i = 0.0f; i < iterations; i++)
    //     {
    //         //light.intensity = light.intensity - (brightnessDifference * updateTime);
    //         light.intensity -= (brightnessDifference*updateTime);
    //         yield return new WaitForSeconds(timeStep);
    //     }
    // }

    // public IEnumerator DeathFade()
    // {
    //     healthBar.gameObject.SetActive(false);
    //     healthText.gameObject.SetActive(false);
    //     timeStep = 1.0f * updateTime;
    //     iterations = 1.0f / updateTime;
    //     float alpha = 255.0f;
    //     float counter = 0.1f;
    //     for (float i = 0.0f; i < iterations; i++)
    //     {
    //         deathPanel.color = new Color(0.0f, 0.0f, 0.0f, (alpha * counter));
    //         counter += 0.1f;
    //         yield return new WaitForSeconds(timeStep);
    //     }
    // }

    public IEnumerator DeathFade(float lanternSnapshot, float torchSnapshot, float ambientSnapshot)
    {
        float timeToDark = 1.0f;
        float timeStep = timeToDark * updateTime;
        float iterations = timeToDark / updateTime;

        for (int i = 0; i < 10; i++)
        {
            yield return new WaitForSecondsRealtime(1.0f);
            lantern.intensity += Mathf.Clamp(-1 * (lanternSnapshot * updateTime), -1 * (lanternSnapshot / 10), 0.0f);

            ambientLight.intensity += Mathf.Clamp(-1 * (ambientSnapshot * updateTime), -1 * (ambientSnapshot / 10), 0.0f);

            foreach (Transform item in lightParent.transform)
            {
                childLight = item.GetComponent<UnityEngine.Rendering.Universal.Light2D>();
                childLight.intensity += Mathf.Clamp(-1 * (torchSnapshot * updateTime), -1 * (torchSnapshot / 10), 0.0f);
            }
            Time.timeScale = 0.0f;
        }
    }

    // public void FadeLight(UnityEngine.Rendering.Universal.Light2D light, float oldBrightness, float newBrightness, float timeFrame, float floorBrightness)
    // {
    //     float timePassed = 0.0f;

    //     while (timePassed < timeFrame)
    //     {
    //         timePassed += Time.deltaTime;

    //         light.intensity = Mathf.Lerp(oldBrightness, newBrightness, timePassed/timeFrame) + floorBrightness;
    //     }
    // }
}