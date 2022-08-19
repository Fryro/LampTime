using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{

    public static bool paused = false;
    [SerializeField] public GameObject pauseMenu;
    [SerializeField] public GameObject inventoryMenu;
    private GameObject player;
    private SpriteRenderer playerSprite;
    private InventoryMenu inventoryMenuController;
    private PlayerHitbox playerHitbox;
    private PlayerLantern playerLantern;
    private float attackCDSnapshot;

    // Start
    void Start()
    {
        inventoryMenuController = GetComponent<InventoryMenu>();
        playerHitbox = GameObject.Find("PlayerHitbox").GetComponent<PlayerHitbox>();
        player = GameObject.Find("Player");
        playerSprite = player.GetComponent<SpriteRenderer>();
        playerLantern = player.GetComponentInChildren<PlayerLantern>();
        Resume();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Pause"))
        {
            if (inventoryMenuController.inventoryOpen)
            {
                if (inventoryMenuController.AreMenusOpen())
                {
                    inventoryMenuController.CloseAllMenus();
                }
                else
                {
                    CloseInventory();
                }
            }
            else 
            {
                if (paused)
                {
                    Resume();
                }
                else
                {
                    Pause();
                }
            }
        }
    }

    public void Resume()
    {
        if (!playerLantern.dying)
        {
            pauseMenu.SetActive(false);
            Time.timeScale = 1.0f;
            paused = false;
            playerHitbox.overallCD = attackCDSnapshot;
            attackCDSnapshot = 0.0f;
            playerSprite.sortingOrder = 100;
        }
    }

    public void Pause()
    {
        if (!playerLantern.dying)
        {
            pauseMenu.SetActive(true);
            Time.timeScale = 0.0f;
            paused = true;
            attackCDSnapshot = playerHitbox.overallCD;
            playerHitbox.overallCD = 1000000.0f;
            playerSprite.sortingOrder = 89; // Just under the pause menu, but above everything else.
        }
    }

    public void OpenInventory()
    {
        pauseMenu.SetActive(false);
        inventoryMenu.SetActive(true);
        inventoryMenuController.SetInventoryOpenTrue();
    }

    public void CloseInventory()
    {
        inventoryMenuController.SetInventoryOpenFalse();
        inventoryMenu.SetActive(false);
        pauseMenu.SetActive(true);
    }

    public void LoadMenu()
    {
        SceneManager.LoadScene("Menu");
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
