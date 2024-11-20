using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplodeEnemyAbility : MonoBehaviour
{
    [SerializeField]
    private float explodeDamage = 40f; // Sát thương gây ra khi nổ

    [SerializeField]
    private float explodeSpeed = 2f; // Tốc độ nổ (thời gian tăng kích thước)

    [SerializeField]
    private float scaleTimes = 8f; // Số lần tăng kích thước trước khi nổ

    [SerializeField]
    private float maxSizeAdd = 0.3f; // Kích thước tối đa mà zombie sẽ tăng lên

    // Tài nguyên cho hiệu ứng nổ
    private UnityEngine.Object explosionRef;
    // Kích thước thay đổi từng lần
    private Vector3 scaleChange; 

    private EnemySpawner enemySpawner;  // Biến để lưu spawner

    // Biến để theo dõi trạng thái nổ
    public bool isExploding = false;

    public Animator animator;

    [Header("Audio Settings")]
    [SerializeField]
    private AudioClip explodeSound; // Âm thanh khi nổ
    private AudioSource audioSource; // Nguồn phát âm thanh

    // Hàm thiết lập EnemySpawner
    public void SetSpawner(EnemySpawner spawner)
    {
        enemySpawner = spawner; // Lưu trữ spawner
    }

    void Start()
    {
        isExploding = false; // Khởi tạo trạng thái nổ là false
        explosionRef = Resources.Load("Explosion"); // Tải tài nguyên nổ từ thư mục Resources
        animator = GetComponent<Animator>();

        // Kiểm tra và thêm AudioSource
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }
    }

    // Phương thức xử lý va chạm
    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Kiểm tra xem đối tượng va chạm có phải là người chơi không
        if (collision.gameObject.tag == "Player")
        {
            animator.SetBool("AttackPlayer", true);
            EnemyMovement moveScript = GetComponent<EnemyMovement>(); // Lấy script di chuyển của zombie
            moveScript.canMove = false; // Ngăn không cho zombie di chuyển

            
            StartCoroutine(ExplodeDelay(collision)); // Bắt đầu coroutine để xử lý hiệu ứng nổ
        }
    }

    // Coroutine để xử lý quá trình nổ
    private IEnumerator ExplodeDelay(Collider2D collision)
    {
        HealthController healthController = collision.gameObject.GetComponent<HealthController>(); // Lấy HealthController của người chơi
        isExploding = true; // Đánh dấu là đang nổ

        // Tính toán kích thước tăng thêm cho mỗi lần tăng kích thước
        float sizeAddPerScale = maxSizeAdd / scaleTimes;
        scaleChange = new Vector3(sizeAddPerScale, sizeAddPerScale, sizeAddPerScale); // Tạo vector thay đổi kích thước

        // Tính thời gian cho mỗi lần thay đổi kích thước
        float timePerScale = explodeSpeed / scaleTimes;
        for (int i = 0; i < scaleTimes; i++)
        {
            transform.localScale += scaleChange; // Tăng kích thước
            yield return new WaitForSeconds(timePerScale); // Chờ trước khi thay đổi kích thước tiếp theo
        }

        // Tạo hiệu ứng nổ
        GameObject explosion = (GameObject)Instantiate(explosionRef);
        // Đặt vị trí của hiệu ứng nổ
        explosion.transform.position = new Vector3(transform.position.x, transform.position.y + .3f, transform.position.z);

        float distanceToPlayer = Vector2.Distance(transform.position, collision.gameObject.transform.position);
        PlaySound(explodeSound); // Phát âm thanh nổ
        // Xóa đối tượng kẻ thù sau khi nổ
        Destroy(gameObject);
        

        if (distanceToPlayer <= 8)
        {
            healthController.TakeDamage(40); // Gây sát thương cho người chơi
        }
    }

    private void PlaySound(AudioClip clip)
    {
        if (audioSource != null && clip != null)
        {
            audioSource.PlayOneShot(clip);
        }
    }
}
