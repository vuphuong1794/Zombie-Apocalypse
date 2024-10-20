using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootGrenade : MonoBehaviour
{
    [SerializeField] private GameObject bulletPrefab; // Prefab của viên đạn
    [SerializeField] private Transform firePos;       // Vị trí mà viên đạn sẽ được spawn ra
    [SerializeField] private float timeBetweenShots = 0.2f; // Thời gian chờ giữa các lần bắn
    [SerializeField] private float bulletSpeed = 1; // Tốc độ bay của viên đạn
    [SerializeField] private float damage = 1; // Sát thương của viên đạn

    private float timeSinceLastShot = 0f;

    private void Update()
    {
        timeSinceLastShot += Time.deltaTime;

        // Kiểm tra nếu nhấn chuột trái và đủ thời gian giữa các lần bắn
        if (Input.GetMouseButton(0) && timeSinceLastShot >= timeBetweenShots)
        {
            FireBullet();
            timeSinceLastShot = 0f; // Đặt lại thời gian chờ giữa các lần bắn
        }
    }

    private void FireBullet()
    {
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
