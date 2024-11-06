using UnityEngine;

public class ItemAssets : MonoBehaviour
{
    public static ItemAssets Instance { get; private set; }

    public Transform pfItemWorld; 
    public Sprite pistolSprite;
    public Sprite sniperSprite;
    public Sprite rifleSprite;
    public Sprite grenadeSprite;
    public Sprite healthPotionSprite;

    private void Awake()
    {
        Instance = this;  // Set instance first

        if (pfItemWorld == null) // Kiểm tra prefab
        {
            Debug.LogError("Chưa gán prefab ItemWorld!");
        }
    }
}