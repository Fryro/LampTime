using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class InventoryMenu : MonoBehaviour
{

    public bool inventoryOpen = false;
    public bool meleeOpen = false;
    public bool rangedOpen = false;
    public bool trinketsOpen = false;

    [SerializeField] public GameObject inventoryMenu;
    [SerializeField] public Inventory inventory;

    [SerializeField] public Button daggerButton;
    [SerializeField] public Button swordButton;
    [SerializeField] public Button flailButton;
    [SerializeField] public Button halberdButton;

    [SerializeField] public Button bowButton;
    [SerializeField] public Button shieldButton;
    [SerializeField] public Button staffButton;
    [SerializeField] public Button lanternButton;

    [SerializeField] public Button trinketsButton;


    // Start
    void Start()
    {
        CloseInventory();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Pause"))
        {
            if (inventoryOpen)
            {
                CloseInventory();
            }
        }
    }

    public void CloseInventory()
    {
        inventoryMenu.SetActive(false);
        inventoryOpen = false;
    }

    public void MeleeMenu()
    {
        if (meleeOpen)
        {
            daggerButton.gameObject.SetActive(false);
            swordButton.gameObject.SetActive(false);
            flailButton.gameObject.SetActive(false);
            halberdButton.gameObject.SetActive(false);
            meleeOpen = false;
        }
        else
        {
            daggerButton.gameObject.SetActive(true);
            swordButton.gameObject.SetActive(true);
            flailButton.gameObject.SetActive(true);
            halberdButton.gameObject.SetActive(true);   
            meleeOpen = true;
        }
    }

    public void RangedMenu()
    {
        if (rangedOpen)
        {
            shieldButton.gameObject.SetActive(false);
            staffButton.gameObject.SetActive(false);
            bowButton.gameObject.SetActive(false);
            lanternButton.gameObject.SetActive(false);
        }
        else
        {
            shieldButton.gameObject.SetActive(true);
            staffButton.gameObject.SetActive(true);
            bowButton.gameObject.SetActive(true);
            lanternButton.gameObject.SetActive(true);
        }
    }

    public void TrinketsMenu()
    {
        if (trinketsOpen)
        {
            trinketsButton.gameObject.SetActive(false);
        }
        else
        {
            trinketsButton.gameObject.SetActive(true);
        }
    }
    
    public void SetInventoryOpenTrue()
    {
        this.inventoryOpen = true;
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
}
