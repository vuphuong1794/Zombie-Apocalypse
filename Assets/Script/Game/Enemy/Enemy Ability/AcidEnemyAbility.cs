using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AcidEnemyAbility : MonoBehaviour
{
    [SerializeField]
    private float _acidDamageAmount=10;

    [SerializeField]
    private float _damageTimes=3;


    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.GetComponent<PlayerMovement>())
        {
            var healthController = collision.gameObject.GetComponent<HealthController>();
            StartCoroutine(InfectDelay(healthController));
        }
    }
    private IEnumerator InfectDelay(HealthController healthController)
    {
        for (int i = 0; i < 3; i++)
        {
            //Debug.Log("haha");
            healthController.TakeAbilityDamage(_acidDamageAmount);
            yield return new WaitForSeconds(2);
        }

    }
}
