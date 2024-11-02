using UnityEngine;
using UnityEngine.UI;

public class UI_Inventory : MonoBehaviour
{
    [Header("Required References")]
    [SerializeField] private RectTransform itemSlotContainer;
    [SerializeField] private RectTransform itemSlotTemplate;

    [Header("Layout Settings")]

    private Inventory inventory;

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

    private void RefreshInventoryItems()
    {

        foreach(Transform child in itemSlotContainer)
        {
            if (child == itemSlotTemplate) continue;
            Destroy(child.gameObject);
        }

        if (itemSlotContainer == null || itemSlotTemplate == null || inventory == null)
        {
            Debug.LogError($"Required references missing in UI_Inventory on {gameObject.name}");
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
            itemSlot.anchoredPosition = new Vector2(x * totalCellSize, y * totalCellSize);
            Image image = itemSlot.Find("image").GetComponent<Image>();
            image.sprite = item.GetSprite();
            x++;
            if (x > 4)
            {
                x = 0;
                y++;
            }
        }
    }
}