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


    private void Awake() // Use Awake to initialize the instance
    {
        Debug.Log("item Assets awake call");
        if (Instance == null)
        {
            Instance = this;
            Debug.Log("ItemAssets instance initialized in Awake.");
        }
        else
        {
            Debug.LogWarning("Multiple ItemAssets instances detected! Destroying duplicate.");
            Destroy(gameObject);
        }
    }

}
