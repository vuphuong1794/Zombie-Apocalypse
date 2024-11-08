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
        //AddItem(new Item { itemType = Item.ItemType.HealthPotion, amount = 1 });
    }

    //thêm vật phẩm
    // Add item to the inventory
    public void AddItem(Item item)
    {
        if (item.itemType == Item.ItemType.Pistol)
        {
            // Always allow adding a pistol, even if one already exists
            itemList.Add(item);
            Debug.Log($"Added {item.itemType} to inventory and firing OnItemListChanged event");
            OnItemListChanged?.Invoke(this, EventArgs.Empty);
        }
        else if (item.itemType == Item.ItemType.Rifle || item.itemType == Item.ItemType.Sniper || item.itemType == Item.ItemType.Grenade)
        {
            // Check if the player already has a gun besides the pistol
            bool hasOtherGun = false;
            foreach (Item inventoryItem in itemList)
            {
                if (inventoryItem.itemType == Item.ItemType.Rifle || inventoryItem.itemType == Item.ItemType.Sniper || inventoryItem.itemType == Item.ItemType.Grenade)
                {
                    hasOtherGun = true;
                    break; // If already has one of these guns, don't allow adding another
                }
            }

            if (!hasOtherGun)
            {
                itemList.Add(item);
                Debug.Log($"Added {item.itemType} to inventory and firing OnItemListChanged event");
                OnItemListChanged?.Invoke(this, EventArgs.Empty);
            }
            else
            {
                Debug.Log($"Already have a gun besides the pistol, can't add {item.itemType}");
            }
        }
        else
        {
            // Add other items (like Grenades or Potions)
            itemList.Add(item);
            Debug.Log($"Added {item.itemType} to inventory and firing OnItemListChanged event");
            OnItemListChanged?.Invoke(this, EventArgs.Empty);
        }
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