using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunShooting : MonoBehaviour
{
    [Header("Weapon Settings")]
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private Transform firePos;
    [SerializeField] private float timeBetweenShots = 0.2f;
    [SerializeField] private float bulletSpeed = 20f;
    [SerializeField] private float damage = 1f;
    [SerializeField] private int maxAmmo = 12;

    [Header("Audio")]
    [SerializeField] private AudioSource m_ShoottingPistol;
    [SerializeField] private AudioClip emptyGunSound;

    private float timeSinceLastShot = 0f;
    private Inventory inventory;
    private bool isReloading = false;

    private void Start()
    {
        InitializeComponents();
    }

    private void InitializeComponents()
    {
        inventory = FindObjectOfType<Inventory>();
        if (inventory == null)
        {
            Debug.LogError("Không tìm thấy Inventory trong scene!");
        }

        if (m_ShoottingPistol == null)
        {
            m_ShoottingPistol = GetComponent<AudioSource>();
        }

        firePos.transform.localPosition = new Vector3(183f, -222f, 0);
    }

    private void Update()
    {
        timeSinceLastShot += Time.deltaTime;

        if (isReloading)
            return;

        HandleInput();
    }

    private void HandleInput()
    {
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

        if (Input.GetKeyDown(KeyCode.R))
        {
            StartCoroutine(Reload());
        }
    }

    private bool CanShoot()
    {
        return inventory != null && inventory.HasItem(Item.ItemType.bullet);
    }

    private void FireBullet()
    {
        if (!inventory.HasItem(Item.ItemType.bullet))
        {
            PlayEmptyGunSound();
            return;
        }

        GameObject bulletInstance = Instantiate(bulletPrefab, firePos.position, transform.rotation);
        Rigidbody2D bulletRb = bulletInstance.GetComponent<Rigidbody2D>();
        if (bulletRb != null)
        {
            bulletRb.velocity = transform.up * bulletSpeed;
        }

        if (m_ShoottingPistol != null)
        {
            m_ShoottingPistol.Play();
        }
    }

    private void PlayEmptyGunSound()
    {
        if (m_ShoottingPistol != null && emptyGunSound != null && !m_ShoottingPistol.isPlaying)
        {
            m_ShoottingPistol.PlayOneShot(emptyGunSound);
        }
    }

    private IEnumerator Reload()
    {
        if (isReloading || GetRemainingBullets() >= maxAmmo)
            yield break;

        isReloading = true;
        Debug.Log("Đang nạp đạn...");

        float reloadTime = 1.5f;
        yield return new WaitForSeconds(reloadTime);

        int currentAmmo = GetRemainingBullets();
        int bulletsToAdd = maxAmmo - currentAmmo;

        isReloading = false;
        Debug.Log("Nạp đạn xong!");
    }

    public int GetRemainingBullets()
    {
        return inventory != null ? inventory.GetItemCount(Item.ItemType.bullet) : 0;
    }
}
