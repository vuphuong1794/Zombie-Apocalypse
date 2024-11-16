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
        bullet,
        grenadeBullet
    }

    public ItemType itemType;
    public int amount;
    public int ammoCount; // Thêm thuộc tính đạn

    // Định nghĩa số đạn tối đa cho mỗi loại súng
    private static readonly int[] MaxAmmoPerGun = new int[]
    {
        30,  // Pistol max ammo
        45,  // Rifle max ammo
        10,  // Sniper max ammo
        5    // Grenade max ammo
    };

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
            case ItemType.grenadeBullet: return ItemAssets.Instance.grenadeBulletSprite;
        }
    }

    public bool IsGun()
    {
        return itemType == ItemType.Pistol ||
               itemType == ItemType.Sniper ||
               itemType == ItemType.Grenade ||
               itemType == ItemType.Rifle;
    }

    public bool IsPistol()
    {
        return itemType == ItemType.Pistol;
    }

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
            case ItemType.grenadeBullet:
                return true;
        }
    }

    public int GetGunIndex()
    {
        switch (itemType)
        {
            case ItemType.Pistol: return 0;
            case ItemType.Rifle: return 1;
            case ItemType.Sniper: return 2;
            case ItemType.Grenade: return 3;
            default: return -1;
        }
    }

    // Thêm các phương thức mới để xử lý đạn
    public int GetMaxAmmo()
    {
        int gunIndex = GetGunIndex();
        if (gunIndex >= 0 && gunIndex < MaxAmmoPerGun.Length)
        {
            return MaxAmmoPerGun[gunIndex];
        }
        return 0;
    }

    public void AddAmmo(int amount)
    {
        if (IsGun())
        {
            ammoCount = Mathf.Min(ammoCount + amount, GetMaxAmmo());
        }
    }

    public bool HasAmmo()
    {
        return IsGun() && ammoCount > 0;
    }

    public bool UseAmmo(int amount = 1)
    {
        if (IsGun() && ammoCount >= amount)
        {
            ammoCount -= amount;
            return true;
        }
        return false;
    }

    // Constructor mới để tạo súng với số đạn ban đầu
    public static Item CreateGun(ItemType gunType, int initialAmmo = 0)
    {
        Item gun = new Item { itemType = gunType, amount = 1 };
        if (gun.IsGun())
        {
            gun.ammoCount = Mathf.Min(initialAmmo, gun.GetMaxAmmo());
        }
        return gun;
    }
}