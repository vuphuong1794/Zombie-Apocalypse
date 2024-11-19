using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal; // Đảm bảo namespace này được thêm vào

public class BulletGrenade : MonoBehaviour
{
    [SerializeField] private float timeMaxExist = 3f; // Thời gian tồn tại tối đa của viên đạn trước khi nổ
    [SerializeField] private float defScaleBullet = 0.1f; // Kích thước mặc định của viên đạn
    [SerializeField] private AudioClip hitSound; // Âm thanh phát ra khi viên đạn va chạm
    [SerializeField] private GameObject impactEffect; // Hiệu ứng khi viên đạn va chạm
    [SerializeField] private float impactEffectLifetime = 0.5f; // Thời gian tồn tại của hiệu ứng va chạm
    [SerializeField] private TrailRenderer trailRenderer; // Hiệu ứng đuôi khi viên đạn bay
    [SerializeField] private float explosionRadius = 3f; // Bán kính nổ của viên đạn
    [SerializeField] private LayerMask damageableLayer; // Layer của các đối tượng có thể nhận sát thương
    [SerializeField] private LayerMask obstacleLayer; // Layer của vật cản (tường hoặc chướng ngại)
    [SerializeField] private GameObject explosionEffect; // Hiệu ứng nổ khi viên đạn phát nổ
    [SerializeField] private float explosionEffectLifetime = 0.3f; // Thời gian tồn tại của hiệu ứng nổ
    [SerializeField] private float _damageAmount; // Lượng sát thương mà viên đạn gây ra

    private Rigidbody2D bulletBody; // Tham chiếu tới Rigidbody của viên đạn
    private AudioSource audioSource; // Tham chiếu tới AudioSource để phát âm thanh va chạm
    private float timeCount; // Bộ đếm thời gian để theo dõi thời gian tồn tại của viên đạn
    private bool hasBounced = false; // Đánh dấu nếu viên đạn đã va vào tường
    private bool hasReflected = false; // Trạng thái để ngăn phản xạ liên tiếp

    [SerializeField]
    private Inventory inventory;
    private static Inventory playerInventory;

    [SerializeField] private GameObject muzzleFlashEffect; // Hiệu ứng bắn đạn (như ánh sáng hoặc lửa từ nòng súng)
    [SerializeField] private float muzzleFlashLifetime = 0.2f; // Thời gian tồn tại của hiệu ứng bắn đạn


    private void Start()
    {
        bulletBody = GetComponent<Rigidbody2D>();
        audioSource = GetComponent<AudioSource>();
        timeCount = 0;

        if (playerInventory == null)
        {
            playerInventory = FindObjectOfType<Inventory>();
            if (playerInventory == null)
            {
                Debug.LogError("Không tìm thấy Inventory trong scene!");
            }
        }

        if (trailRenderer != null) trailRenderer.Clear();
        DecreaseBulletCount();

        // Gọi hiệu ứng bắn đạn
        ShowMuzzleFlashEffect();
    }


    private void DecreaseBulletCount()
    {
        if (playerInventory != null)
        {
            // Giảm số đạn trong inventory
            Item bulletItem = new Item { itemType = Item.ItemType.bullet, amount = 1 };
            playerInventory.RemoveItem(bulletItem);
        }
    }

    private void Update()
    {
        timeCount += Time.deltaTime;
        if (timeCount > timeMaxExist) DestroyBullet();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Zombie"))
        {
            EnemyHealthController enemyHealthController = null;
            enemyHealthController = collision.gameObject.GetComponent<EnemyHealthController>();

            if (enemyHealthController != null)
            {
                enemyHealthController.TakeDamage(_damageAmount);
            }
            DestroyBullet();
        }
        else if (collision.gameObject.CompareTag("Chest"))
        {
            ChestHealthController chestHealthController = null;
            chestHealthController = collision.gameObject.GetComponent<ChestHealthController>();

            if (chestHealthController != null)
            {
                chestHealthController.TakeDamage(_damageAmount);
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

    private void DestroyBullet()
    {
        if (impactEffect != null)
        {
            GameObject effect = Instantiate(impactEffect, transform.position, transform.rotation);
            Destroy(effect, impactEffectLifetime);
        }

        CreateExplosionLight(); // Thêm hiệu ứng phát 
        Explode();
        Destroy(gameObject);
    }

    private void Explode()
    {
        if (explosionEffect != null)
        {
            GameObject explosion = Instantiate(explosionEffect, transform.position, Quaternion.identity);
            Destroy(explosion, explosionEffectLifetime);
        }

        //Collider2D[] hitTargets = Physics2D.OverlapCircleAll(transform.position, explosionRadius, damageableLayer);
        //foreach (Collider2D target in hitTargets)
        //{
        //    Vector2 directionToTarget = target.transform.position - transform.position;
        //    float distanceToTarget = directionToTarget.magnitude;
        //    RaycastHit2D hit = Physics2D.Raycast(transform.position, directionToTarget, distanceToTarget, obstacleLayer);

        //    if (hit.collider == null || !hit.collider.CompareTag("Wall"))
        //    {
        //        EnemyHealthController enemyHealth = target.GetComponent<EnemyHealthController>();
        //        if (enemyHealth != null)
        //        {
        //            float damagePercentage = Mathf.Clamp01(1 - (distanceToTarget / explosionRadius));
        //            float damageToApply = _damageAmount * damagePercentage;
        //            enemyHealth.TakeDamage(damageToApply);
        //        }
        //    }
        //}
    }

    private void ShowMuzzleFlashEffect()
    {
        if (muzzleFlashEffect != null)
        {
            // Tạo hiệu ứng nòng súng tại vị trí hiện tại của viên đạn
            GameObject flash = Instantiate(muzzleFlashEffect, transform.position, transform.rotation);

            // Hủy hiệu ứng sau một thời gian ngắn
            Destroy(flash, muzzleFlashLifetime);
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
        Destroy(lightObject,0.5f);
    }

    //private IEnumerator FadeAndDestroyLight(Light2D light, GameObject lightObject)
    //{
    //    float fadeDuration = 3f;
    //    float timer = 0f;

    //    while (timer < fadeDuration)
    //    {
    //        timer += Time.deltaTime;
    //        light.intensity = Mathf.Lerp(5f, 0f, timer / fadeDuration);
    //        yield return null;
    //    }

    //    // Hủy đối tượng ánh sáng
    //    if (lightObject != null)
    //    {
    //        //timer = 0f;
    //        //Destroy(lightObject);
    //    }
    //}

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, explosionRadius);
    }
    private void ResetReflection()
    {
        hasReflected = false;
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
