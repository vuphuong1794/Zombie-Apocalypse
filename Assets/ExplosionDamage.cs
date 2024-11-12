using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosionDamage : MonoBehaviour
{
    [SerializeField] private float _damageAmount=100; // Lượng sát thương mà viên đạn gây ra
    [SerializeField] private float explosionRadius = 3f; // Bán kính nổ của viên đạn
    // Start is called before the first frame update
    void Start()
    {
        _damageAmount = 100;
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Zombie")
        {
            // Kiểm tra nếu mục tiêu có thành phần EnemyHealthController (giả sử là Zombie)
            EnemyHealthController enemyHealth = collision.gameObject.GetComponent<EnemyHealthController>();
            if (enemyHealth != null)
            {
                Vector2 directionToTarget = collision.gameObject.transform.position - transform.position;
                float distanceToTarget = directionToTarget.magnitude; // Tính khoảng cách đến mục tiêu
                // Tính phần trăm sát thương dựa trên khoảng cách
                float damagePercentage = Mathf.Clamp01(1 - (distanceToTarget / explosionRadius));
                float damageToApply = _damageAmount * damagePercentage;

                enemyHealth.TakeDamage(damageToApply);
            }
        }
    }
}
