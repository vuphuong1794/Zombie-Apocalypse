using System;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public event EventHandler OnItemListChanged;
    private List<Item> itemList;

    private void Awake()
    {
        Debug.Log("Inventory Awake called");
        itemList = new List<Item>();
        AddStartingItems();
    }

    private void AddStartingItems()
    {
        AddItem(new Item { itemType = Item.ItemType.Pistol, amount = 1 });
        //AddItem(new Item { itemType = Item.ItemType.Sniper, amount = 1 });
        AddItem(new Item { itemType = Item.ItemType.HealthPotion, amount = 1 });
    }

    //thêm vật phẩm
    public void AddItem(Item item)
    {
        if (item.IsStackable())
        {
            Item existingItem = itemList.Find(i => i.itemType == item.itemType);
            if (existingItem != null)
            {
                Debug.Log("Increasing item quantity");
                existingItem.amount += item.amount;
            }
            else
            {
                itemList.Add(item);
            }
        }
        else if (item.itemType == Item.ItemType.Pistol ||
                 item.itemType == Item.ItemType.Rifle ||
                 item.itemType == Item.ItemType.Sniper ||
                 item.itemType == Item.ItemType.Grenade)
        {
            bool gunExists = itemList.Exists(i => i.IsGun());
            if (!gunExists || item.itemType == Item.ItemType.Pistol)
            {
                itemList.Add(item);
            }
        }
        else
        {
            itemList.Add(item);
        }
        OnItemListChanged?.Invoke(this, EventArgs.Empty);
    }


    public List<Item> GetItemList()
    {
        if (itemList == null)
        {
            itemList = new List<Item>();
        }
        return itemList;
    }


    public void RemoveItem(Item item)
    {
        /*
        if (item.IsStackable())
        {
            Item itemInInventory = null;
            foreach (Item inventoryItem in itemList)
            {
                if (inventoryItem.itemType == item.itemType)
                {
                    inventoryItem.amount -= item.amount;
                    itemInInventory = inventoryItem;
                }
            }
            if (itemInInventory != null && itemInInventory.amount <=0)
            {
                itemList.Add(item);
            }
        }
        else
        {
            itemList.Remove(item);
        }*/


        itemList.Remove(item);
        OnItemListChanged?.Invoke(this, EventArgs.Empty);

    }

    public void UseItem(Item item)
    {

    }
}