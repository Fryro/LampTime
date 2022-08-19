using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemObject : MonoBehaviour
{
    [SerializeField] private GameObject parent;
    [SerializeField] private Inventory inventory;
    private GameObject player;
    public InventoryItemData referenceItem;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("Player");
        inventory = player.GetComponent<Inventory>();
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
