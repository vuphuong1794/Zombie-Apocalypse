using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosionDamage : MonoBehaviour
{
    [SerializeField] private float _damageAmount = 100f; // Lượng sát thương tối đa tại tâm vụ nổ
    [SerializeField] private float explosionRadius = 3f; // Bán kính nổ của vụ nổ

    private void Start()
    {
        _damageAmount = 100;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Zombie") || collision.gameObject.CompareTag("Player"))
        {
            // Tìm khoảng cách giữa vị trí va chạm và vị trí trung tâm của vụ nổ
            Vector2 directionToTarget = collision.transform.position - transform.position;
            float distanceToTarget = directionToTarget.magnitude; // Tính khoảng cách đến mục tiêu

            // Tính phần trăm sát thương dựa trên khoảng cách, càng xa tâm vụ nổ thì sát thương càng ít
            float damagePercentage = Mathf.Clamp01(1 - (distanceToTarget / explosionRadius));
            float damageToApply = _damageAmount * damagePercentage;

            // Kiểm tra và gây sát thương dựa trên loại đối tượng
            HealthController healthController = null;
            EnemyHealthController enemyHealthController = null;

            if (collision.gameObject.CompareTag("Player"))
            {
                healthController = collision.gameObject.GetComponent<HealthController>();
                if (healthController != null)
                {
                    healthController.TakeDamage(damageToApply);
                }
                else
                {
                    Debug.LogWarning("Không tìm thấy HealthController trên đối tượng va chạm.");
                }
            }
            else if (collision.gameObject.CompareTag("Zombie"))
            {
                enemyHealthController = collision.gameObject.GetComponent<EnemyHealthController>();
                if (enemyHealthController != null)
                {
                    enemyHealthController.TakeDamage(damageToApply);
                }
                else
                {
                    Debug.LogWarning("Không tìm thấy EnemyHealthController trên đối tượng va chạm.");
                }
            }
        }
    }
}
