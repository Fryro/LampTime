using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class InventoryMenu : MonoBehaviour
{

    public bool inventoryOpen = false;
    public bool meleeOpen = false;
    public bool rangedOpen = false;
    public bool trinketsOpen = false;

    public PauseMenu pauseMenuController;
    [SerializeField] public GameObject pauseMenu;
    [SerializeField] public GameObject inventoryMenu;
    [SerializeField] public Inventory inventory;

    [SerializeField] public Button meleeButton;
    [SerializeField] public Button rangedButton;
    [SerializeField] public Button trinketsButton;

    [SerializeField] public Button daggerButton;
    [SerializeField] public Button swordButton;
    [SerializeField] public Button flailButton;
    [SerializeField] public Button halberdButton;

    [SerializeField] public Button bowButton;
    [SerializeField] public Button shieldButton;
    [SerializeField] public Button staffButton;
    [SerializeField] public Button lanternButton;

    [SerializeField] public Button ammyButton;
    [SerializeField] public Button moneyButton;

    // Item Reference Data
    [SerializeField] private InventoryItemData daggerReference;
    [SerializeField] private InventoryItemData moneyReference;

    // Item Display Variables
    private string daggerText = "Dagger";
    private int daggerCount;

    private string moneyText = "Money";
    private int moneyCount;




    // Start
    void Start()
    {
        pauseMenuController = GetComponent<PauseMenu>();

        meleeOpen = true;
        MeleeMenu();
        rangedOpen = true;
        RangedMenu();
        trinketsOpen = true;
        TrinketsMenu();
    }

    // Update is called once per frame
    void Update()
    {
        daggerCount = inventory.GetStackSize(daggerReference);
        moneyCount = inventory.GetStackSize(moneyReference);
    }

    public void CloseInventory()
    {
        inventoryOpen = false;
        inventoryMenu.SetActive(false);
        pauseMenu.SetActive(true);
    }

    public void MeleeMenu()
    {
        if (meleeOpen)
        {
            halberdButton.gameObject.SetActive(false);
            flailButton.gameObject.SetActive(false);
            swordButton.gameObject.SetActive(false);
            daggerButton.gameObject.SetActive(false);
            meleeOpen = false;
        }
        else
        {
            halberdButton.gameObject.SetActive(true);   
            flailButton.gameObject.SetActive(true);
            swordButton.gameObject.SetActive(true);
            daggerButton.gameObject.SetActive(true);
            ChangeText(daggerButton, daggerText, ""+daggerCount);
            meleeOpen = true;
        }
    }

    public void RangedMenu()
    {
        if (rangedOpen)
        {
            lanternButton.gameObject.SetActive(false);
            staffButton.gameObject.SetActive(false);
            bowButton.gameObject.SetActive(false);
            shieldButton.gameObject.SetActive(false);
            rangedOpen = false;
        }
        else
        {
            lanternButton.gameObject.SetActive(true);
            staffButton.gameObject.SetActive(true);
            bowButton.gameObject.SetActive(true);
            shieldButton.gameObject.SetActive(true);
            rangedOpen = true;
        }
    }

    public void TrinketsMenu()
    {
        if (trinketsOpen)
        {
            ammyButton.gameObject.SetActive(false);
            moneyButton.gameObject.SetActive(false);
            trinketsOpen = false;
        }
        else
        {
            ammyButton.gameObject.SetActive(true);
            moneyButton.gameObject.SetActive(true);
            ChangeText(moneyButton, moneyText, ""+moneyCount);
            trinketsOpen = true;
        }
    }
    
    public void SetInventoryOpenTrue()
    {
        this.inventoryOpen = true;
    }

    public void SetInventoryOpenFalse()
    {
        this.inventoryOpen = false;
    }

    public void CloseAllMenus()
    {
        this.meleeOpen = true;
        this.rangedOpen = true;
        this.trinketsOpen = true;
        MeleeMenu();
        RangedMenu();
        TrinketsMenu();
    }

    public bool AreMenusOpen()
    {
        if (meleeOpen || rangedOpen || trinketsOpen)
        {
            return true;
        }
        return false;
    }
    
    public void EquipSword()
    {
        
    }

    public void EquipDagger()
    {
        
    }
    public void EquipFlail()
    {
        
    }
    public void EquipHalberd()
    {
        
    }
    public void EquipLantern()
    {
        
    }
    public void EquipStaff()
    {
        
    }
    public void EquipBow()
    {
        
    }
    public void EquipShield()
    {
        
    }
    public void EquipAmmy()
    {
        
    }

    private void ChangeText(Button button, string base_text, string qualifier)
    {
        button.GetComponentInChildren<TMP_Text>().text = (base_text + " (" + qualifier + ")"); 
    }
}
