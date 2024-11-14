using System;
using UnityEngine;

[Serializable]
public class Item
{
    public enum ItemType
    {
        Pistol,  // index 0
        Rifle,   // index 1
        Sniper,   // index 2
        Grenade, //index 3
        HealthPotion,
        bullet

    }

    public ItemType itemType;
    public int amount;

    public Sprite GetSprite()
    {
        if (ItemAssets.Instance == null)
        {
            Debug.LogError("ItemAssets.Instance is null!");
            return null;
        }
        switch (itemType)
        {
            default:
            case ItemType.Pistol: return ItemAssets.Instance.pistolSprite;
            case ItemType.Sniper: return ItemAssets.Instance.sniperSprite;
            case ItemType.Rifle: return ItemAssets.Instance.rifleSprite;
            case ItemType.Grenade: return ItemAssets.Instance.grenadeSprite;
            case ItemType.HealthPotion: return ItemAssets.Instance.healthPotionSprite;
            case ItemType.bullet: return ItemAssets.Instance.bulletSprite;
        }
    }

    public bool IsGun()
    {
        return itemType == ItemType.Pistol || itemType == ItemType.Sniper || itemType == ItemType.Grenade || itemType == ItemType.Rifle;
    }

    public bool IsPistol()
    {
        return itemType == ItemType.Pistol;
    }


    //điều kiện tăng số lượng vật phẩm
    public bool IsStackable()
    {
        switch (itemType)
        {
            default:
            case ItemType.Pistol:
            case ItemType.Sniper:
            case ItemType.Grenade:
            case ItemType.Rifle:
            case ItemType.HealthPotion:
                return false;
            case ItemType.bullet:
                return true;

        }
    }

    public int GetGunIndex()
    {
        switch (itemType)
        {
            case ItemType.Pistol: return 0;
            case ItemType.Rifle: return 1; // Rifle là index 1
            case ItemType.Sniper: return 2; // Sniper là index 2
            case ItemType.Grenade: return 3;
            default: return -1;
        }
    }

}