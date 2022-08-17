using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHitbox : MonoBehaviour
{
    [SerializeField] private Collider2D hitbox;
    [SerializeField] private PlayerResources player;
    [SerializeField] private UnityEngine.Rendering.Universal.Light2D lantern;

    private ContactFilter2D filter;
    private Collider2D[] results;
    private int numCollisions;

    private bool primaryInput;
    private bool secondaryInput;

    //private primaryAttackShape;
    private float primaryFrontload;
    private float primaryAttackDamage;
    private float primaryHeal;
    private float primaryCD;
    //private secondaryAttackShape;    
    private float secondaryFrontload;
    private float secondaryAttackDamage;
    private float secondaryHeal;
    private float secondaryCD;

    private float overallCD;

    private float timeAnchor;
    private float timeNow;


    // Types of Attacks
    // 1 : sword
    // 2 : flail
    // 3 : halberd
    // 4 : dagger
    // 5 : bow
    // 6 : staff
    // 7 : lantern

    // Start is called before the first frame update
    void Start()
    {
        timeAnchor = Time.time;
        filter = new ContactFilter2D();

        numCollisions = 10;
        results = new Collider2D[numCollisions];

        primaryFrontload = 0.5f;
        primaryInput = false;
        primaryAttackDamage = 3.0f;
        primaryHeal = 2.5f;
        primaryCD = 1.5f;

        secondaryFrontload = 1.0f;
        secondaryInput = false;
        secondaryAttackDamage = 5.0f;
        secondaryHeal = 5.0f;
        secondaryCD = 3.0f;

        overallCD = 0.0f;
    }

    // Update is called once per frame
    void Update()
    {
        timeNow = Time.time;

        primaryInput = Input.GetButtonDown("Fire1");
        secondaryInput = Input.GetButtonDown("Fire2");

        if ((Mathf.Abs(timeNow - timeAnchor) > overallCD))
        {
            if (primaryInput)
            {
                overallCD = primaryCD;
                timeAnchor = timeNow;
                StartCoroutine(Attack(primaryFrontload, primaryAttackDamage, primaryHeal, 1));
            }
            else if (secondaryInput)
            {
                overallCD = secondaryCD;
                timeAnchor = timeNow;
                StartCoroutine(Attack(secondaryFrontload, secondaryAttackDamage, secondaryHeal, 1));
            }
        }
    }

    private IEnumerator Attack(float frontload, float damage, float healing, int typeOfAttack)
    {
        print("Telegraphing attack...");
        StartCoroutine(Telegraph(frontload, typeOfAttack));
        yield return new WaitForSeconds(frontload);
        print("Swing!");
        results = new Collider2D[numCollisions];
        int collisions = hitbox.OverlapCollider(filter, results);
        for (int i = 0; i < collisions; i++)
        {
            Collider2D other = results[i];
            if (other.gameObject.tag == "Enemy")
            {
                EnemyBehaviour enemy = other.GetComponent<EnemyBehaviour>();
                enemy.TakeDamage(damage);
                player.Heal(healing);
            }
        }
        lantern.intensity = 1.5f;
    }
    private IEnumerator Telegraph(float frontload, int typeOfAttack)
    {
        if (typeOfAttack == 1)
        {
            float fll = frontload / 8.0f;
            //print("im here");
            for (float i = 0.0f; i < frontload; i += fll)
            {
                lantern.intensity = intensityFunction(i + 1.5f);
                yield return new WaitForSeconds(fll);
            }
            lantern.intensity = 1.5f;
        }
    }

    private float intensityFunction(float t)
    {
        return (t);
    }
}
