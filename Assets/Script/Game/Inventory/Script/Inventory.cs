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
        AddItem(new Item { itemType = Item.ItemType.Gun1, amount = 1 });
        AddItem(new Item { itemType = Item.ItemType.Gun2, amount = 1 });
        AddItem(new Item { itemType = Item.ItemType.HealthPotion, amount = 1 });
    }

    public void AddItem(Item item)
    {
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
}