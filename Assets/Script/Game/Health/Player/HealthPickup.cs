// HealthPickup.cs
using UnityEngine;

public class HealthPickup : MonoBehaviour
{
    [SerializeField] private float healAmount = 20f; // Số máu được hồi
    [SerializeField] private bool destroyOnPickup = true; // Có hủy vật phẩm sau khi nhặt không
    [SerializeField] private AudioClip pickupSound; // Âm thanh khi nhặt máu

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            HealthController healthController = other.GetComponent<HealthController>();

            if (healthController != null)
            {
                // Kiểm tra xem người chơi có đang ở máu tối đa không
                if (healthController.CurrentHealth < healthController.MaximumHealth)
                {
                    // Chỉ hồi một lượng máu cố định
                    healthController.AddHealth(healAmount);

                    if (pickupSound != null)
                    {
                        AudioSource.PlayClipAtPoint(pickupSound, transform.position);
                    }

                    if (destroyOnPickup)
                    {
                        Destroy(gameObject);
                    }
                }
            }
        }
    }
}