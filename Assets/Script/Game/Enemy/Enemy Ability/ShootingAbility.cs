using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootingAbility : MonoBehaviour
{
    [SerializeField] private GameObject bulletPrefab; // Prefab của viên đạn
    [SerializeField] private Transform firePos;       // Vị trí mà viên đạn sẽ được spawn ra
    [SerializeField] private float timeBetweenShots = 0.4f; // Thời gian chờ giữa các lần bắn
    [SerializeField] private float bulletSpeed = 10; // Tốc độ bay của viên đạn
    [SerializeField] private float damage = 1; // Sát thương của viên đạn

    private float timeSinceLastShot = 0f;
    private float timeCount;

    private void Start()
    {
        timeCount = 0;
    }

    private void Update()
    {
        timeCount += Time.deltaTime;
        if (timeCount > 5)
        {
            int randNum = Random.Range(1, 7);
            Debug.LogWarning(randNum);
            //timeSinceLastShot += Time.deltaTime;

            // Kiểm tra nếu nhấn chuột trái và đủ thời gian giữa các lần bắn
            if (randNum != 4)
            {
                FireBullet();
                //timeSinceLastShot = 0f; // Đặt lại thời gian chờ giữa các lần bắn
            }
            timeCount = 0;
        }
    }

    private void FireBullet()
    {
        Debug.LogWarning("Zombie shooted");
        // Tạo viên đạn tại vị trí firePos với góc quay mặc định
        GameObject bulletInstance = Instantiate(bulletPrefab, firePos.position, transform.rotation);

        // Lấy Rigidbody2D của viên đạn và đặt vận tốc cho nó
        Rigidbody2D bulletRb = bulletInstance.GetComponent<Rigidbody2D>();
        if (bulletRb != null)
        {
            bulletRb.velocity = transform.up * bulletSpeed;
        }
    }
}
