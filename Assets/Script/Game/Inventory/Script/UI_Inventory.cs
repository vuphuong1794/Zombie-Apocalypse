using CodeMonkey.Utils;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_Inventory : MonoBehaviour
{
    [Header("Required References")]
    [SerializeField] private RectTransform itemSlotContainer;
    [SerializeField] private RectTransform itemSlotTemplate;

    [Header("Layout Settings")]

    private Inventory inventory;
    private PlayerMovement player;

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

    private void Inventory_OnItemListChanged(object sender, System.EventArgs e)
    {
        RefreshInventoryItems();
    }

    //cập nhật lại kho đồ 
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

        foreach (Item item in inventory.GetItemList())
        {
            RectTransform itemSlot = Instantiate(itemSlotTemplate, itemSlotContainer);
            itemSlot.gameObject.SetActive(true);
            
            itemSlot.GetComponent<Button_UI>().ClickFunc = () =>
            {
                //use item
                inventory.UseItem(item);
            };

            itemSlot.GetComponent<Button_UI>().MouseRightClickFunc = () => {
                //Drop item
                inventory.RemoveItem(item);
                ItemWorld.DropItem(player.GetPosition(), item);
               
            };

            itemSlot.anchoredPosition = new Vector2(x * totalCellSize, y * totalCellSize);

            Image image = itemSlot.Find("image")?.GetComponent<Image>();
            if (image != null)
            {
                image.sprite = item.GetSprite();
            }

            //tăng số lượng vật phẩm nêu trùng 
            
            TextMeshProUGUI uiText = itemSlot.Find("text")?.GetComponent<TextMeshProUGUI>();
            if (uiText != null)
            {
                if (item.amount > 1)
                {
                    uiText.SetText(item.amount.ToString());
                }
                else
                    uiText.SetText("");
            }

            x++;
            if (x > 4)
            {
                x = 0;
                y++;
            }
        }
    }
}