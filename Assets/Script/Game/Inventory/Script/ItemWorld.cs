using UnityEngine;
using CodeMonkey.Utils;

public class ItemWorld : MonoBehaviour
{
    public static ItemWorld SpawnItemWorld(Vector3 position, Item item)
    {
        // Check if ItemAssets instance is null
        if (ItemAssets.Instance == null)
        {
            Debug.LogError("ItemAssets.Instance is null! Ensure ItemAssets is in the scene and initialized.");
            return null;
        }

        // Check if pfItemWorld is assigned
        if (ItemAssets.Instance.pfItemWorld == null)
        {
            Debug.LogError("ItemAssets.Instance.pfItemWorld is not assigned!");
            return null;
        }

        // Adjust scale based on item type
        if (item.itemType == Item.ItemType.HealthPotion)
        {
            ItemAssets.Instance.pfItemWorld.localScale = Vector3.one * 0.15f;
        }
        else
        {
            ItemAssets.Instance.pfItemWorld.localScale = Vector3.one * 0.06f;
        }

        // Instantiate item in the world
        Transform transform = Instantiate(ItemAssets.Instance.pfItemWorld, position, Quaternion.identity);
        ItemWorld itemWorld = transform.GetComponent<ItemWorld>();
        itemWorld.SetItem(item);

        return itemWorld;
    }

    private Item item;
    private SpriteRenderer spriteRenderer;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public void SetItem(Item item)
    {
        this.item = item;
        spriteRenderer.sprite = item.GetSprite();
    }

    public Item GetItem() { return item; }

    public void DestroySelf()
    {
        Destroy(gameObject);
    }

    public static ItemWorld DropItem(Vector3 dropPosition, Item item, WeaponHolder weaponHolder)
    {
        Vector3 randomDir = UtilsClass.GetRandomDir();
        ItemWorld itemWorld = SpawnItemWorld(dropPosition + randomDir * 1f, item);
        Rigidbody2D itemRigidbody = itemWorld.GetComponent<Rigidbody2D>();
        itemRigidbody.AddForce(randomDir * 1f, ForceMode2D.Force);

        if (item.IsGun())
        {
            int gunIndex = item.GetGunIndex();
            weaponHolder.DeactivateWeapon(gunIndex);
        }

        return itemWorld;
    }
}
