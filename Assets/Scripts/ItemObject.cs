using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemObject : MonoBehaviour
{
    [SerializeField] private GameObject parent;
    [SerializeField] private Inventory inventory;
    public InventoryItemData referenceItem;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnHandlePickupItem()
    {
        inventory.Add(referenceItem);
        Destroy(parent);
    }
}
