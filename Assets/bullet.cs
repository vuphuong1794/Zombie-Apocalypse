using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour // Thay đổi tên lớp thành Bullet (theo quy tắc đặt tên)
{
    public GameObject bulletPrefab; // Đổi tên biến thành bulletPrefab cho rõ ràng hơn
    public Transform firePos;
    public float TimeBtwFire = 0.2f;
    public float bulletForce;

    private float timeBtwFire;

    void Start()
    {
        timeBtwFire = 0; // Khởi tạo timeBtwFire
    }

    void Update()
    {
        timeBtwFire -= Time.deltaTime; // Giảm thời gian chờ

        if (Input.GetMouseButton(0) && timeBtwFire <= 0) // Kiểm tra nếu có thể bắn
        {
            FireBullet();
        }
    }

    void FireBullet()
    {
        timeBtwFire = TimeBtwFire; // Đặt lại thời gian chờ
        GameObject bulletInstance = Instantiate(bulletPrefab, firePos.position, Quaternion.identity);

        // Thêm lực cho viên đạn
        Rigidbody2D rb = bulletInstance.GetComponent<Rigidbody2D>();
        if (rb != null) // Kiểm tra xem Rigidbody2D có tồn tại không
        {
            rb.AddForce(firePos.up * bulletForce, ForceMode2D.Impulse); // Thêm lực cho viên đạn
        }
    }
}
