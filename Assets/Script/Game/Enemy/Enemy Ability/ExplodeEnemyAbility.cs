using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

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

    AudioManager audioManager;

    private void Awake()
    {
        audioManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManager>();
    }

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
        CreateExplosionLight(); // Thêm hiệu ứng phát 
        audioManager.PlaySFX(audioManager.explode); // Phát âm thanh nổ
        // Xóa đối tượng kẻ thù sau khi nổ
        Destroy(gameObject);
        

        if (distanceToPlayer <= 8)
        {
            healthController.TakeDamage(40); // Gây sát thương cho người chơi
        }
    }

    private void CreateExplosionLight()
    {
        // Tạo một đối tượng Light
        GameObject lightObject = new GameObject("ExplosionLight");
        Light2D light = lightObject.AddComponent<Light2D>(); // Thêm Light2D
        light.lightType = Light2D.LightType.Point; // Đặt loại ánh sáng (Point, Spot, Global, etc.)
        light.color = new Color(1f, 0.5f, 0.2f); // Màu ánh sáng (màu cam lửa)
        light.intensity = 10f; // Độ sáng
        light.pointLightOuterRadius = 5f; // Bán kính ngoài (phạm vi sáng)
        light.pointLightInnerRadius = 3f; // Bán kính trong

        // Đặt vị trí của Light trùng với vị trí của vụ nổ
        lightObject.transform.position = transform.position;

        // Đính ánh sáng vào đối tượng vụ nổ (tùy chọn)
        //lightObject.transform.SetParent(this.transform);

        //// Tạo hiệu ứng giảm độ sáng
        //StartCoroutine(FadeAndDestroyLight(light, lightObject));
        Destroy(lightObject, 0.5f);
    }
}
