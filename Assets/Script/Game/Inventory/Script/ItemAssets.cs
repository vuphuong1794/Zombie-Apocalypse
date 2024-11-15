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
    public Sprite bulletSprite;
    public Sprite grenadeBulletSprite;

    private void Awake()
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
