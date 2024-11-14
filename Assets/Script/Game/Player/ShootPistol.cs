using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootPistol : MonoBehaviour
{
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private Transform firePos;
    [SerializeField] private float timeBetweenShots = 0.4f;
    [SerializeField] private float bulletSpeed = 20;
    [SerializeField] private float damage = 1;
    [SerializeField] private AudioSource audioSource; // Thêm audio source cho âm thanh
    [SerializeField] private AudioClip shootSound; // Âm thanh khi bắn
    [SerializeField] private AudioClip emptyGunSound; // Âm thanh khi hết đạn

    private float timeSinceLastShot = 0f;
    private Inventory inventory;
    private bool isReloading = false;

    private void Start()
    {
        // Tìm inventory trong scene
        inventory = FindObjectOfType<Inventory>();
        if (inventory == null)
        {
            Debug.LogError("Không tìm thấy Inventory trong scene!");
        }

        // Kiểm tra và thêm AudioSource nếu cần
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }
    }

    private void Update()
    {
        timeSinceLastShot += Time.deltaTime;

        // Kiểm tra nếu đang reload thì không cho bắn
        if (isReloading)
            return;

        // Kiểm tra nếu nhấn chuột trái và đủ thời gian giữa các lần bắn
        if (Input.GetMouseButton(0) && timeSinceLastShot >= timeBetweenShots)
        {
            if (CanShoot())
            {
                FireBullet();
                timeSinceLastShot = 0f;
            }
            else
            {
                PlayEmptyGunSound();
            }
        }

        // Thêm chức năng reload (tùy chọn)
        if (Input.GetKeyDown(KeyCode.R))
        {
            StartCoroutine(Reload());
        }
    }

    private bool CanShoot()
    {
        if (inventory == null) return false;
        return inventory.HasItem(Item.ItemType.bullet);
    }

    private void FireBullet()
    {
        // Tạo viên đạn
        GameObject bulletInstance = Instantiate(bulletPrefab, firePos.position, transform.rotation);

        // Cài đặt damage cho viên đạn (nếu có component xử lý damage)
        BulletRifle bulletComponent = bulletInstance.GetComponent<BulletRifle>();
        if (bulletComponent != null)
        {
            // Nếu có trường để set damage
            // bulletComponent.SetDamage(damage);
        }

        // Đặt vận tốc cho viên đạn
        Rigidbody2D bulletRb = bulletInstance.GetComponent<Rigidbody2D>();
        if (bulletRb != null)
        {
            bulletRb.velocity = transform.up * bulletSpeed;
        }

        // Phát âm thanh bắn
        PlayShootSound();
    }

    private void PlayShootSound()
    {
        if (audioSource != null && shootSound != null)
        {
            audioSource.PlayOneShot(shootSound);
        }
    }

    private void PlayEmptyGunSound()
    {
        if (audioSource != null && emptyGunSound != null && !audioSource.isPlaying)
        {
            audioSource.PlayOneShot(emptyGunSound);
        }
    }

    // Hệ thống reload (tùy chọn)
    private IEnumerator Reload()
    {
        if (isReloading) yield break;

        isReloading = true;
        Debug.Log("Đang nạp đạn...");

        // Thời gian nạp đạn (có thể điều chỉnh)
        float reloadTime = 1.5f;
        yield return new WaitForSeconds(reloadTime);

        isReloading = false;
        Debug.Log("Nạp đạn xong!");
    }

    // Method để lấy số đạn còn lại (có thể dùng để hiển thị UI)
    public int GetRemainingBullets()
    {
        if (inventory == null) return 0;
        return inventory.GetItemCount(Item.ItemType.bullet);
    }
}