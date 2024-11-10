using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CodeMonkey.Utils;

public class ItemWorld : MonoBehaviour
{
    public static ItemWorld SpawnItemWorld(Vector3 position, Item item)
    {
        if (item.itemType == Item.ItemType.HealthPotion)
        {
            ItemAssets.Instance.pfItemWorld.localScale = Vector3.one * 0.15f; // Adjust the scale for HealthPotion
        } 
        else
        {
            ItemAssets.Instance.pfItemWorld.localScale = Vector3.one * 0.06f; // Use the default scale for other items
        }


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

    //xóa vật phẩm
    public void DestroySelf()
    {
        Destroy(gameObject);
    }

    //vứt vật phẩm ra khỏi kho đồ
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

