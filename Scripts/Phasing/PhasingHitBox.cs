using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhasingHitBox : MonoBehaviour
{
    [Header("Components")]
    public PlayerMovement player;
    public GameObject Enemy;

    [Header("Damage")]
    public int damage;

    [Header("Hits")]
    public bool SingleHit;
    public bool MultipleHits;

    private void OnTriggerStay2D(Collider2D target)
    {
        if (MultipleHits)
        {
            if (target.gameObject.tag == "enemy")
            {
                EnemyController enemy = target.GetComponent<EnemyController>();
                enemy.TakeDamage(damage);
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D target)
    {
        if (SingleHit)
        {
            if (target.gameObject.tag == "enemy")
            {
                EnemyController enemy = target.GetComponent<EnemyController>();
                enemy.TakeDamage(damage);
            }
        }
    }
}
