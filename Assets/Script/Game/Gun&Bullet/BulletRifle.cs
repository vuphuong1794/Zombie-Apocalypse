using UnityEngine;

public class BulletRifle : MonoBehaviour
{
    [SerializeField] private float timeMaxExist = 3f; // Thời gian tồn tại tối đa của viên đạn
    [SerializeField] private float defScaleBullet = 0.1f; // Kích thước mặc định của viên đạn
    [SerializeField] private AudioClip hitSound; // Âm thanh khi viên đạn va chạm
    [SerializeField] private GameObject impactEffect; // Hiệu ứng khi đạn va chạm
    [SerializeField] private float impactEffectLifetime = 0.5f; // Thời gian tồn tại của hiệu ứng va chạm
    private Rigidbody2D bulletBody;
    private AudioSource audioSource;
    private float timeCount;

    [SerializeField]
    private float _damageAmount;

    private void Start()
    {
        Debug.Log("Dan sinh ra");
        // Xoay đạn 90 độ để đúng hướng với hướng người chơi
        transform.Rotate(new Vector3(0, 0, 90));

        // Định dạng kích thước mặc định của viên đạn
        gameObject.transform.localScale = new Vector3(defScaleBullet, defScaleBullet, defScaleBullet);

        // Lấy Rigidbody2D của đạn
        bulletBody = GetComponent<Rigidbody2D>();
        audioSource = GetComponent<AudioSource>();

        // Khởi tạo thời gian tồn tại của đạn
        timeCount = 0;

        // Kiểm tra Rigidbody2D có tồn tại không
        if (bulletBody == null)
        {
            Debug.LogError("Rigidbody2D không được tìm thấy trên viên đạn.");
            return;
        }
    }

    private void Update()
    {
        // Tăng thời gian tồn tại của viên đạn
        timeCount += Time.deltaTime;

        // Hủy viên đạn nếu thời gian tồn tại vượt quá giới hạn
        if (timeCount > timeMaxExist)
        {
            DestroyBullet();
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject == null)
        {
            Debug.Log("Collision object is null!");
        }

        // Kiểm tra nếu viên đạn va chạm với đối tượng có tag "Zombie"
        if (collision.gameObject.CompareTag("Zombie"))
        {
            // Hủy viên đạn khi va chạm với zombie

            DestroyBullet();

            Destroy(gameObject);
            //  Giảm máu zombie bị đạn va chạm
            var enemyHealthController = collision.gameObject.GetComponent<EnemyHealthController>();

            if (enemyHealthController == null)
            {
                Debug.Log("enemyHealthController is null!");
            }

            enemyHealthController.TakeDamage(_damageAmount);

        }
        // Kiểm tra nếu viên đạn va chạm với đối tượng có tag "Wall"
        else if (collision.gameObject.CompareTag("Wall"))
        {
            // Lấy điểm tiếp xúc đầu tiên khi va chạm với tường
            ContactPoint2D contactPoint = collision.contacts[0];

            // Phát âm thanh khi đạn va chạm vào tường
            

            // Hiển thị hiệu ứng va chạm tại vị trí tiếp xúc
            ShowImpactEffect(contactPoint.point);

            // Hủy viên đạn khi va chạm với tường
            DestroyBullet();
        }
    }


    // Phương thức hủy viên đạn và tạo hiệu ứng vụn nổ nếu cần
    private void DestroyBullet()
    {
        // Hiển thị hiệu ứng vụn nổ tại vị trí của viên đạn trước khi hủy nó
        if (impactEffect != null)
        {
            GameObject effect = Instantiate(impactEffect, transform.position, transform.rotation);
            Destroy(effect, impactEffectLifetime); // Hủy hiệu ứng sau một khoảng thời gian
        }

        // Hủy viên đạn
        Destroy(gameObject);
    }

    
    // Phương thức hiển thị hiệu ứng va chạm tại điểm va chạm
    private void ShowImpactEffect(Vector2 position)
    {
        if (impactEffect != null)
        {
            GameObject effect = Instantiate(impactEffect, position, impactEffect.transform.rotation);
            Destroy(effect, impactEffectLifetime); // Hủy hiệu ứng sau một khoảng thời gian
        }
    }

}
