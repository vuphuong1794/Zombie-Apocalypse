using System;
using UnityEngine;

[Serializable]
public class Item
{
    public enum ItemType
    {
        Gun1,
        Gun2,
        HealthPotion
    }

    public ItemType itemType;
    public int amount;

    public Sprite GetSprite()
    {
        switch (itemType)
        {
            case ItemType.Gun1: return ItemAssets.Instance.gun1Sprite;
            case ItemType.Gun2: return ItemAssets.Instance.gun2Sprite;
            case ItemType.HealthPotion: return ItemAssets.Instance.healthPotionSprite;
            default: return null;
        }
    }

    //điều kiện tăng số lượng vật phẩm
    public bool IsStackable()
    {
        switch (itemType)
        {
            default:
            case ItemType.Gun1:
            case ItemType.Gun2:
                return false;
            case ItemType.HealthPotion:
                return true;
            
        }
    }
    
}