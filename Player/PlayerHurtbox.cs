using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHurtbox : MonoBehaviour
{
    [SerializeField] private PlayerResources player;
    [SerializeField] private PlayerMovement playerMove;

    private float timeAnchor;
    private float timeNow;

    private float iFrames;

    void Start()
    {
        iFrames = 0.75f;
    }

    void Update()
    {
        timeNow = Time.time;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Enemy")
        {
            if (Mathf.Abs(timeNow - timeAnchor) > iFrames)
            {
                RegisterHit(other);
                playerMove.GetHit();
                timeAnchor = timeNow;
            }
            else
            print("Allowed: " + iFrames + " Elapsed: " + Mathf.Abs(timeNow - timeAnchor));
        }
    }

    void OnTriggerStay2D(Collider2D other)
    {
        if (other.gameObject.tag == "Enemy")
        {
            if (Mathf.Abs(timeNow - timeAnchor) > iFrames)
            {
                RegisterHit(other);
                playerMove.GetHit();
                timeAnchor = timeNow;
            }
            else
            print("Allowed: " + iFrames + " Elapsed: " + Mathf.Abs(timeNow - timeAnchor));
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.tag == "Enemy")
        {
            //print("safety!");
        }
    }

    void RegisterHit(Collider2D other)
    {
        EnemyBehaviour enemy = other.GetComponent<EnemyBehaviour>();
        player.TakeDamage(enemy.damage);
        print("youch! current hp: " + player.health);
        playerMove.GetHit();
    }
}
