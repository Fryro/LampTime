using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBehaviour : Enemy
{
    [SerializeField] private GameObject parent;
    private Transform parentTransform;

    // Drop Table (Size can vary)
    //private Dictionary<int, GameObject> dropTable;
    private int[] dropTable;
    private const int dropTableUniqueEntries = 3;
    private int dropTableTotalEntries; 

    // Drop Table Items and Chances
    [SerializeField] private GameObject money;
    private const int moneyDropChance = 70;

    [SerializeField] private GameObject dagger;
    private const int daggerDropChance = 10;

    [SerializeField] private GameObject noDrop; // = new GameObject("noDrop");
    private const int noDropChance = 20;

    // List of Items, List of Chances
    private List<GameObject> dropList;
    private int[] dropChances;

    // Helper Variables for Dropping Items
    private int generateItem;
    private GameObject dropped;

    // Start is called before the first frame update
    public override void Start()
    {
        // Add drops and dropChances to respective tables
        // Must be added in descending order (70, 20, 10)
        dropChances = new int[dropTableUniqueEntries] {moneyDropChance, noDropChance, daggerDropChance}; // 3 = total entries in following list.
        // Must be added in same order as above
        dropList = new List<GameObject>();
        dropList.Add(money);
        dropList.Add(noDrop);
        dropList.Add(dagger); 

        // Generate and populate the droptable
        GenerateDropTable();

        maxHealth = 10.0f;
        health = 10.0f;
        guardMultiplier = 0.2f;
        guarding = false;
        damage = 10.0f;
        speed = 0.1f;

        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        graphics = GetComponent<SpriteRenderer>();

        parentTransform = parent.GetComponent<Transform>();
    }

    // Update is called once per frame
    public void Update()
    {

        //graphics.color = new Color (graphics.color.Red, graphics.color.Green, graphics.color.Blue, ((health/maxHealth)));

        HandleMovement();
        timeNow = Time.time;

        if (health <= 0)
        {
            print("Enemy Killed.");
            DropItems();
            Destroy(parent);
        }
    }

    public void GenerateDropTable()
    {
        dropTable = new int[dropTableUniqueEntries];
        // For each index in dropChances
        for (int i = 0; i < dropChances.Length; i++)
        {
            // Append the chance to the table
            dropTable[i] = dropChances[i];
        }
        // For each chance in the dropTable, add that chance to the total entry count.
        dropTableTotalEntries = 0;
        foreach (var item in dropTable)
        {
            dropTableTotalEntries += item;
        }
    }

    public void DropItems()
    {
        // Get a random value between 0 and the total entries (lower inclusive, upper exclusive).
        generateItem = Random.Range(0, dropTableTotalEntries);
        print("Generated Item: " + generateItem);
        // For each chance in the droptable...
        for (int i = 0; i < dropTable.Length; i++)
        {
            // If random value is below that chance value, generate that item and return.
            if (generateItem <= dropTable[i])
            {
                dropped = Instantiate(dropList[i]);
                dropped.transform.position = transform.position;
                return;
            }
            // If it isn't, decrement by the chance and move on to the next chance in the table.
            else
            {
                generateItem -= dropTable[i];
            }
        }
    }
}