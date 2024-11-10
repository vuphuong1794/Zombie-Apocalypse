using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletGrenade : MonoBehaviour
{
    [SerializeField] private float timeMaxExist = 3f; // Thời gian tồn tại tối đa của viên đạn trước khi nổ
    [SerializeField] private float defScaleBullet = 0.1f; // Kích thước mặc định của viên đạn
    [SerializeField] private AudioClip hitSound; // Âm thanh phát ra khi viên đạn va chạm
    [SerializeField] private GameObject impactEffect; // Hiệu ứng khi viên đạn va chạm
    [SerializeField] private float impactEffectLifetime = 0.5f; // Thời gian tồn tại của hiệu ứng va chạm
    [SerializeField] private TrailRenderer trailRenderer; // Hiệu ứng đuôi khi viên đạn bay
    [SerializeField] private float explosionRadius = 3f; // Bán kính nổ của viên đạn
    [SerializeField] private LayerMask damageableLayer; // Layer của các đối tượng có thể nhận sát thương
    [SerializeField] private LayerMask obstacleLayer; // Layer của vật cản (tường hoặc chướng ngại)
    [SerializeField] private GameObject explosionEffect; // Hiệu ứng nổ khi viên đạn phát nổ
    [SerializeField] private float explosionEffectLifetime = 1f; // Thời gian tồn tại của hiệu ứng nổ
    [SerializeField] private float _damageAmount; // Lượng sát thương mà viên đạn gây ra

    private Rigidbody2D bulletBody; // Tham chiếu tới Rigidbody của viên đạn
    private AudioSource audioSource; // Tham chiếu tới AudioSource để phát âm thanh va chạm
    private float timeCount; // Bộ đếm thời gian để theo dõi thời gian tồn tại của viên đạn

    private void Start()
    {
        bulletBody = GetComponent<Rigidbody2D>();
        audioSource = GetComponent<AudioSource>();
        timeCount = 0;

        // Xóa hiệu ứng đuôi ban đầu để bắt đầu lại từ đầu
        if (trailRenderer != null) trailRenderer.Clear();
    }

    private void Update()
    {
        timeCount += Time.deltaTime;
        if (timeCount > timeMaxExist) DestroyBullet();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        DestroyBullet();
    }

    private void DestroyBullet()
    {
        // Tạo hiệu ứng va chạm nếu được thiết lập
        if (impactEffect != null)
        {
            GameObject effect = Instantiate(impactEffect, transform.position, transform.rotation);
            Destroy(effect, impactEffectLifetime);
        }

        // Gọi hàm nổ để gây sát thương trong phạm vi nổ
        Explode();

        // Hủy viên đạn sau khi đã nổ
        Destroy(gameObject);
    }

    private void Explode()
    {
        // Hiển thị hiệu ứng nổ tại vị trí viên đạn
        if (explosionEffect != null)
        {
            GameObject explosion = Instantiate(explosionEffect, transform.position, Quaternion.identity);
            Destroy(explosion, explosionEffectLifetime);
        }

        // Lấy tất cả các đối tượng trong phạm vi nổ và có thể nhận sát thương
        Collider2D[] hitTargets = Physics2D.OverlapCircleAll(transform.position, explosionRadius, damageableLayer);
        foreach (Collider2D target in hitTargets)
        {
            Vector2 directionToTarget = target.transform.position - transform.position;
            float distanceToTarget = directionToTarget.magnitude; // Tính khoảng cách đến mục tiêu

            // Kiểm tra nếu không có vật cản trên đường đi từ vụ nổ đến mục tiêu
            RaycastHit2D[] hits = Physics2D.RaycastAll(transform.position, directionToTarget, distanceToTarget, obstacleLayer);

            bool isBlocked = false;
            foreach (RaycastHit2D hit in hits)
            {
                if (hit.collider != target)
                {
                    isBlocked = true;
                    break;
                }
            }

            // Nếu không có vật cản giữa vụ nổ và mục tiêu
            if (!isBlocked)
            {
                EnemyHealthController enemyHealth = target.GetComponent<EnemyHealthController>();
                if (enemyHealth != null)
                {
                    // Tính phần trăm sát thương dựa trên khoảng cách
                    float damagePercentage = Mathf.Clamp01(1 - (distanceToTarget / explosionRadius));
                    float damageToApply = _damageAmount * damagePercentage;

                    enemyHealth.TakeDamage(damageToApply);
                }
            }
        }
    }


    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, explosionRadius);
    }
}
