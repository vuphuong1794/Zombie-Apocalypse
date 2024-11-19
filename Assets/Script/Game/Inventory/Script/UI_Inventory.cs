using CodeMonkey.Utils;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static Item;

public class UI_Inventory : MonoBehaviour
{
    [Header("Required References")]
    [SerializeField] private RectTransform itemSlotContainer;
    [SerializeField] private RectTransform itemSlotTemplate;

    private Inventory inventory;
    private PlayerMovement player;
    public GameObject background;

    public void SetPlayer(PlayerMovement player)
    {
        this.player = player;
    }

    public void SetInventory(Inventory inventory)
    {
        this.inventory = inventory;
        inventory.OnItemListChanged += Inventory_OnItemListChanged;
        RefreshInventoryItems();
    }

    private void Inventory_OnItemListChanged(object sender, EventArgs e)
    {
        RefreshInventoryItems();
    }

    private void Update()
    {
        // Drop any equipped gun (Sniper, Grenade, Rifle) when Q is pressed
        if (Input.GetKeyDown(KeyCode.Q))
        {
            DropEquippedGun();
        }
    }

    private void RefreshInventoryItems()
    {
        if (itemSlotContainer == null)
        {
            Debug.LogError($"itemSlotContainer is null in UI_Inventory on {gameObject.name}");
            return;
        }

        if (itemSlotTemplate == null)
        {
            Debug.LogError($"itemSlotTemplate is null in UI_Inventory on {gameObject.name}");
            return;
        }

        if (inventory == null)
        {
            Debug.LogError($"inventory is null in UI_Inventory on {gameObject.name}");
            return;
        }

        // Clear existing items
        foreach (Transform child in itemSlotContainer)
        {
            if (child != itemSlotTemplate)
            {
                Destroy(child.gameObject);
            }
        }

        itemSlotTemplate.gameObject.SetActive(false);

        int x = 0;
        int y = 0;
        float totalCellSize = 75f;
        int i = 0;

        foreach (Item item in inventory.GetItemList())
        {
            RectTransform itemSlot = Instantiate(itemSlotTemplate, itemSlotContainer);
            itemSlot.gameObject.SetActive(true);
            i += 1;

            itemSlot.anchoredPosition = new Vector2(background.transform.localPosition.x + totalCellSize + 200 + 85 * i, background.transform.localPosition.y - 157);

            Image image = itemSlot.Find("image")?.GetComponent<Image>();
            if (image != null)
            {
                image.sprite = item.GetSprite();
            }

            TextMeshProUGUI uiText = itemSlot.Find("amountText")?.GetComponent<TextMeshProUGUI>();
            if (uiText != null)
            {
                if (item.amount > 1)
                {
                    uiText.SetText(item.amount.ToString());
                }
                else
                {
                    uiText.SetText("");
                }
            }

            x++;
            if (x > 4)
            {
                x = 0;
                y++;
            }
        }
    }

    private void DropEquippedGun()
    {
        // Loop through the items in the inventory and check for gun types
        foreach (Item item in inventory.GetItemList())
        {
            if (item != null && (item.itemType == ItemType.Sniper ||
                                 item.itemType == ItemType.Grenade ||
                                 item.itemType == ItemType.Rifle))
            {
                // Remove the item from the inventory
                inventory.RemoveItem(item);

                // Drop the item in the game world
                WeaponHolder weaponHolder = FindObjectOfType<WeaponHolder>();
                ItemWorld.DropItem(player.GetPosition(), item, weaponHolder);

                Debug.Log($"Dropped gun: {item}");
                break; // Exit after dropping one gun
            }
        }
    }

    internal void SetPlayer(MulPlayerMovement mulPlayerMovement)
    {
        throw new NotImplementedException();
    }
}