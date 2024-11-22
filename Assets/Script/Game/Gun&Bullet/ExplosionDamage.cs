using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosionDamage : MonoBehaviour
{
    [SerializeField] private float _damageAmount = 100f; // Lượng sát thương tối đa tại tâm vụ nổ
    [SerializeField] private float explosionRadius = 5f; // Bán kính nổ của vụ nổ
    [SerializeField] private LayerMask damageableLayers; // Lớp các đối tượng có thể nhận sát thương

    private void Start()
    {
        _damageAmount = 100;
        ApplyExplosionDamage();
    }

    private void ApplyExplosionDamage()
    {
        // Lấy tất cả các collider trong bán kính vụ nổ
        Collider2D[] hitTargets = Physics2D.OverlapCircleAll(transform.position, explosionRadius, damageableLayers);

        foreach (Collider2D target in hitTargets)
        {
            float damageToApply = CalculateDamage(target.transform.position);

            if (target.CompareTag("Zombie"))
            {
                var enemyHealthController = target.GetComponent<EnemyHealthController>();
                if (enemyHealthController != null)
                {
                    enemyHealthController.TakeDamage(damageToApply);
                }
                else
                {
                    Debug.LogWarning("Không tìm thấy EnemyHealthController trên đối tượng Zombie.");
                }
            }
            else if (target.CompareTag("Player"))
            {
                var healthController = target.GetComponent<HealthController>();
                if (healthController != null)
                {
                    healthController.TakeDamage(damageToApply);
                }
                else
                {
                    Debug.LogWarning("Không tìm thấy HealthController trên đối tượng Player.");
                }
            }
            else if (target.CompareTag("Chest"))
            {
                var chestHealthController = target.GetComponent<ChestHealthController>();
                if (chestHealthController != null)
                {
                    chestHealthController.TakeDamage(damageToApply);
                }
                else
                {
                    Debug.LogWarning("Không tìm thấy ChestHealthController trên đối tượng Chest.");
                }
            }
        }
    }

    private float CalculateDamage(Vector2 targetPosition)
    {
        // Tìm khoảng cách giữa vị trí mục tiêu và tâm vụ nổ
        float distanceToTarget = Vector2.Distance(targetPosition, transform.position);

        // Tính phần trăm sát thương dựa trên khoảng cách (càng xa thì sát thương càng ít)
        float damagePercentage = Mathf.Clamp01(1 - (distanceToTarget / explosionRadius));
        float damageToApply = _damageAmount * damagePercentage;

        return damageToApply;
    }

    private void OnDrawGizmosSelected()
    {
        // Hiển thị bán kính vụ nổ trong Scene view để dễ kiểm tra
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, explosionRadius);
    }
}
