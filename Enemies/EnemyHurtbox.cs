/*using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHurtbox : MonoBehaviour
{
    private EnemyBehaviour me;

    private float timeAnchor;
    private float timeNow;

    private float iFrames;

    void Start()
    {
        me = GetComponent<EnemyBehaviour>();

        iFrames = 0.25f;
    }

    void Update()
    {
        timeNow = Time.time;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "PlayerDamage")
        {
            if (Mathf.Abs(timeNow - timeAnchor) > iFrames)
            {
                RegisterHit(other);
                timeAnchor = timeNow;
            }
            else
            print("Allowed: " + iFrames + " Elapsed: " + Mathf.Abs(timeNow - timeAnchor));
        }
    }

    void OnTriggerStay2D(Collider2D other)
    {
        if (other.gameObject.tag == "PlayerDamage")
        {
            if (Mathf.Abs(timeNow - timeAnchor) > iFrames)
            {
                RegisterHit(other);
                timeAnchor = timeNow;
            }
            else
            print("Allowed: " + iFrames + " Elapsed: " + Mathf.Abs(timeNow - timeAnchor));
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.tag == "PlayerDamage")
        {
            //print("safety!");
        }
    }

    void RegisterHit(Collider2D other)
    {
        PlayerHitbox player = other.GetComponent<PlayerHitbox>();
        //me.TakeDamage(player.damage);
        print("Enemy Hit! Enemy hp: " + me.health);
    }
}
*/