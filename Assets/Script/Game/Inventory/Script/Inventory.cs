using System;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public event EventHandler OnItemListChanged;
    private List<Item> itemList;
    public const int MAX_BULLETS = 100; // Số đạn tối đa có thể mang

    private void Awake()
    {
        Debug.Log("Inventory Awake called");
        itemList = new List<Item>();
        AddStartingItems();
    }

    private void AddStartingItems()
    {
        AddItem(new Item { itemType = Item.ItemType.Pistol, amount = 1 });
        AddItem(new Item { itemType = Item.ItemType.bullet, amount = 30 }); // Thêm đạn khởi đầu
    }

    public void AddItem(Item item)
    {
        if (item == null) return;

        if (item.IsStackable())
        {
            Item existingItem = itemList.Find(i => i.itemType == item.itemType);

            if (existingItem != null)
            {
                // Kiểm tra giới hạn đạn nếu item là đạn
                if (item.itemType == Item.ItemType.bullet)
                {
                    int newAmount = existingItem.amount + item.amount;
                    existingItem.amount = Mathf.Min(newAmount, MAX_BULLETS);
                    Debug.Log($"Đạn hiện tại: {existingItem.amount}/{MAX_BULLETS}");
                }
                else
                {
                    existingItem.amount += item.amount;
                }
                Debug.Log($"Tăng số lượng {item.itemType} lên {existingItem.amount}");
            }
            else
            {
                // Nếu là đạn, kiểm tra giới hạn
                if (item.itemType == Item.ItemType.bullet)
                {
                    item.amount = Mathf.Min(item.amount, MAX_BULLETS);
                }
                itemList.Add(item);
                Debug.Log($"Thêm mới {item.itemType} với số lượng {item.amount}");
            }
        }
        else
        {
            // Xử lý thêm vũ khí
            if (item.IsGun())
            {
                bool weaponExists = itemList.Exists(i => i.itemType == item.itemType);
                if (!weaponExists)
                {
                    itemList.Add(item);
                    Debug.Log($"Thêm vũ khí mới: {item.itemType}");
                }
                else
                {
                    Debug.Log($"Đã có vũ khí {item.itemType} trong inventory");
                }
            }
            else
            {
                itemList.Add(item);
                Debug.Log($"Thêm item không xếp chồng: {item.itemType}");
            }
        }

        OnItemListChanged?.Invoke(this, EventArgs.Empty);
    }

    public void RemoveItem(Item item)
    {
        if (item == null) return;

        if (item.IsStackable())
        {
            Item existingItem = itemList.Find(i => i.itemType == item.itemType);
            if (existingItem != null)
            {
                existingItem.amount -= item.amount;
                Debug.Log($"Giảm {item.itemType} xuống còn {existingItem.amount}");

                if (existingItem.amount <= 0)
                {
                    itemList.Remove(existingItem);
                    Debug.Log($"Đã xóa {item.itemType} khỏi inventory do số lượng = 0");
                }
            }
        }
        else
        {
            itemList.Remove(item);
            Debug.Log($"Đã xóa {item.itemType} khỏi inventory");
        }

        OnItemListChanged?.Invoke(this, EventArgs.Empty);
    }

    public List<Item> GetItemList()
    {
        return itemList ?? (itemList = new List<Item>());
    }

    public bool HasItem(Item.ItemType itemType, int amount = 1)
    {
        Item item = itemList.Find(i => i.itemType == itemType);
        return item != null && item.amount >= amount;
    }

    public int GetItemCount(Item.ItemType itemType)
    {
        Item item = itemList.Find(i => i.itemType == itemType);
        return item?.amount ?? 0;
    }

    public void UseItem(Item item)
    {
        if (item == null) return;

        switch (item.itemType)
        {
            case Item.ItemType.bullet:
                // Xử lý sử dụng đạn
                RemoveItem(new Item { itemType = Item.ItemType.bullet, amount = 1 });
                break;

            case Item.ItemType.HealthPotion:
                // Xử lý sử dụng health potion
                if (HasItem(Item.ItemType.HealthPotion))
                {
                    RemoveItem(new Item { itemType = Item.ItemType.HealthPotion, amount = 1 });
                    // Thêm logic hồi máu ở đây
                }
                break;

            default:
                Debug.Log($"Không có logic xử lý cho item type: {item.itemType}");
                break;
        }
    }
}