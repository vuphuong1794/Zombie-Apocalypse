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
        //AddItem(new Item { itemType = Item.ItemType.Gun1, amount = 1 });
        //AddItem(new Item { itemType = Item.ItemType.Gun2, amount = 1 });
        //AddItem(new Item { itemType = Item.ItemType.HealthPotion, amount = 1 });
    }

    //thêm vật phẩm
    public void AddItem(Item item)
    {
        //kiểm tra xem vật phẩm này có tăng số lượng được không 
        /*
        if (item.IsStackable())
        {
            bool itemAlreadyInInventory = false;
            foreach (Item inventoryItem in itemList) { 
                if(inventoryItem.itemType == item.itemType)
                {
                    inventoryItem.amount += item.amount;
                }
            }
            if(!itemAlreadyInInventory)
            {
                itemList.Add(item);
            }
        }
        else
        {
            itemList.Add(item);
        }*/
        itemList.Add(item);
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