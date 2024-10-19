using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplodeEnemyAbility : MonoBehaviour
{
    [SerializeField]
    private float explodeDamage;

    private void OnTriggerEnter(Collider collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            Destroy(gameObject);
        }
    }
}
