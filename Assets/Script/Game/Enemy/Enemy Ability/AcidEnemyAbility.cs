using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AcidEnemyAbility : MonoBehaviour
{
    [SerializeField]
    private float _acidDamageAmount;

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.GetComponent<PlayerMovement>())
        {
            var healthController = collision.gameObject.GetComponent<HealthController>();

            healthController.TakeDamage(_acidDamageAmount);
        }
    }
}
