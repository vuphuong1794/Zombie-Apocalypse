using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplodeEnemyAbility : MonoBehaviour
{
    [SerializeField]
    private float explodeDamage;

    //materials
    private UnityEngine.Object explosionRef;
    private Vector3 scaleChange;

    void Start()
    {
        explosionRef = Resources.Load("Explosion");
    }

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
        scaleChange = new Vector3(0.05f, 0.05f, 0.05f);

        for(int i=0; i<8; i++)
        {
            transform.localScale += scaleChange;
            yield return new WaitForSeconds(.5f);
        }

        GameObject explosion = (GameObject)Instantiate(explosionRef);
        explosion.transform.position = new Vector3(transform.position.x, transform.position.y + .3f, transform.position.z);

        Destroy(gameObject);

        healthController.TakeDamage(40);

    }
}
