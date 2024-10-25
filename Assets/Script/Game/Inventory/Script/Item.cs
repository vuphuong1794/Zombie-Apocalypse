using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item
{
    public enum ItemType { 
        Gun1,
        Gun2,
        HealthPositon
    }

    public ItemType itemType;
    public int amount;
}
