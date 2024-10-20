using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplodeEnemyAbility : MonoBehaviour
{
    [SerializeField]
    private float explodeDamage;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            EnemyMovement moveScript = GetComponent<EnemyMovement>();
            moveScript.canMove = false;
            HealthController healthController = collision.gameObject.GetComponent<HealthController>();
            StartCoroutine(ExplodeDelay(healthController));
        }
    }

    private IEnumerator ExplodeDelay(HealthController healthController)
    {
        yield return new WaitForSeconds(3);
        Destroy(gameObject);
        //PlayerMovement moveScript = GetComponent<PlayerMovement>();

        healthController.TakeDamage(40);

    }
}
