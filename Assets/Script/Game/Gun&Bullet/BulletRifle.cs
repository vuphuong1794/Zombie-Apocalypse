using UnityEngine;

public class BulletRifle : MonoBehaviour
{
    [SerializeField] private float timeMaxExist = 3f; // Thời gian tồn tại tối đa của viên đạn
    [SerializeField] private float defScaleBullet = 0.1f; // Kích thước mặc định của viên đạn
    [SerializeField] private AudioClip hitSound; // Âm thanh khi viên đạn va chạm
    [SerializeField] private GameObject impactEffect; // Hiệu ứng khi đạn va chạm
    [SerializeField] private float impactEffectLifetime = 0.5f; // Thời gian tồn tại của hiệu ứng va chạm
    [SerializeField] private TrailRenderer trailRenderer; // Thêm tham chiếu tới TrailRenderer

    private Rigidbody2D bulletBody;
    private AudioSource audioSource;
    private float timeCount;

    [SerializeField]
    private float _damageAmount;

    private void Start()
    {
        Debug.Log("Dan sinh ra");
        transform.Rotate(new Vector3(0, 0, 90));
        gameObject.transform.localScale = new Vector3(defScaleBullet, defScaleBullet, defScaleBullet);
        bulletBody = GetComponent<Rigidbody2D>();
        audioSource = GetComponent<AudioSource>();
        timeCount = 0;

        if (bulletBody == null)
        {
            Debug.LogError("Rigidbody2D không được tìm thấy trên viên đạn.");
            return;
        }

        // Bắt đầu đường bay của đạn
        if (trailRenderer != null)
        {
            trailRenderer.Clear();
        }
    }

    private void Update()
    {
        timeCount += Time.deltaTime;
        if (timeCount > timeMaxExist)
        {
            DestroyBullet();
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Zombie") || collision.gameObject.CompareTag("Player"))
        {
            DestroyBullet();
            var enemyHealthController = collision.gameObject.GetComponent<EnemyHealthController>();
            enemyHealthController.TakeDamage(_damageAmount);
        }
        else if (collision.gameObject.CompareTag("Wall"))
        {
            ContactPoint2D contactPoint = collision.contacts[0];
            Vector2 incomingVector = bulletBody.velocity;
            Vector2 normalVector = contactPoint.normal;
            Vector2 reflectedVector = Vector2.Reflect(incomingVector, normalVector);

            bulletBody.velocity = reflectedVector * 1f;

            // Cập nhật hướng của viên đạn theo hướng mới
            float angle = Mathf.Atan2(reflectedVector.y, reflectedVector.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));

            if (hitSound != null && audioSource != null)
            {
                audioSource.PlayOneShot(hitSound);
            }

            ShowImpactEffect(contactPoint.point);
        }
    }

    private void DestroyBullet()
    {
        if (impactEffect != null)
        {
            GameObject effect = Instantiate(impactEffect, transform.position, transform.rotation);
            Destroy(effect, impactEffectLifetime);
        }

        Destroy(gameObject);
    }

    private void ShowImpactEffect(Vector2 position)
    {
        if (impactEffect != null)
        {
            GameObject effect = Instantiate(impactEffect, position, impactEffect.transform.rotation);
            Destroy(effect, impactEffectLifetime);
        }
    }
}
