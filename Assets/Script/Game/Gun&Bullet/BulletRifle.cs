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
    private bool hasReflected = false; // Trạng thái để ngăn phản xạ liên tiếp

    [SerializeField]
    private float _damageAmount;

    private void Start()
    {
        Debug.Log("Dan sinh ra");
        transform.Rotate(new Vector3(0, 0, 90));
        transform.localScale = new Vector3(10, 10, 10);
        // Định dạng kích thước mặc định của viên đạn
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
            var enemyHealthController = collision.gameObject.GetComponent<EnemyHealthController>();

            if (enemyHealthController != null)
            {
                enemyHealthController.TakeDamage(_damageAmount);
            }
            else
            {
                Debug.LogWarning("Thành phần EnemyHealthController không được tìm thấy trên đối tượng Zombie.");
            }
            DestroyBullet();
        }
        else if (collision.gameObject.CompareTag("Wall") && !hasReflected)
        {
            hasReflected = true; // Đặt trạng thái phản xạ
            ContactPoint2D contactPoint = collision.contacts[0];
            Vector2 incomingVector = bulletBody.velocity;
            Vector2 normalVector = contactPoint.normal;
            Vector2 reflectedVector = Vector2.Reflect(incomingVector, normalVector);

            // Hạn chế thay đổi quá lớn về vận tốc
            reflectedVector = reflectedVector.normalized * Mathf.Min(reflectedVector.magnitude, 10f); // Giới hạn vận tốc

            // Tắt angular velocity để tránh xoay vòng không mong muốn
            bulletBody.angularVelocity = 0f;

            // Cập nhật vận tốc viên đạn theo hướng phản xạ
            bulletBody.velocity = reflectedVector;

            // Cập nhật hướng của viên đạn theo phản xạ
            float angle = Mathf.Atan2(reflectedVector.y, reflectedVector.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));

            // Phát âm thanh va chạm
            if (hitSound != null && audioSource != null)
            {
                audioSource.PlayOneShot(hitSound);
            }

            // Hiển thị hiệu ứng va chạm
            ShowImpactEffect(contactPoint.point);

            // Đặt lại trạng thái phản xạ sau một khoảng thời gian ngắn
            Invoke(nameof(ResetReflection), 0.1f);
        }
    }

    // Phương thức để đặt lại trạng thái phản xạ
    private void ResetReflection()
    {
        hasReflected = false;
    }

    // Phương thức hủy viên đạn và tạo hiệu ứng vụn nổ nếu cần
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
