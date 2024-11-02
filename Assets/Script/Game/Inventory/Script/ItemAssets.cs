using UnityEngine;

public class ItemAssets : MonoBehaviour
{
    public static ItemAssets Instance { get; private set; }

    public Transform pfItemWorld; 
    public Sprite gun1Sprite;
    public Sprite gun2Sprite;
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