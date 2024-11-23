using UnityEngine;
public class healthPickup : MonoBehaviour
{
    [SerializeField] private float healAmount = 20f; // Số máu được hồi mỗi lần nhặt
    [SerializeField] private bool destroyOnPickup = true;
    [SerializeField] private AudioClip pickupSound;

    private void OnTriggerEnter2D(Collider2D other)
    {
        ItemWorld itemWorld = this.GetComponent<ItemWorld>();
        Item item = itemWorld.GetItem();
        if ((item.itemType == Item.ItemType.HealthPotion) && other.CompareTag("Player"))
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
                        Destroy(this);
                    }
                }
            }
        }
    }
}