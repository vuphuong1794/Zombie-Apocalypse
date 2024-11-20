using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D.Animation;

public class RandomComponent : MonoBehaviour
{
    [SerializeField]
    public SpriteRandomRenderer[] spriteRandomRenderers; // List of GameObjects with SpriteRenderers

    private SpriteRenderer spriteRenderer; // SpriteRenderer for the zombie
    private SpriteSkin spriteSkin;
    // Start is called before the first frame update
    void Start()
    {
        // Assign the SpriteRenderer to this object (zombie)
        this.spriteRenderer = GetComponent<SpriteRenderer>();
        this.spriteSkin = GetComponent<SpriteSkin>();

        // Randomly choose a GameObject from the list
        if (spriteRandomRenderers.Length > 0)
        {
            int randomIndex = Random.Range(0, spriteRandomRenderers.Length);
            GameObject selectedObject = spriteRandomRenderers[randomIndex].objectWithSpriteRenderer;

            // Check if the selected object has a SpriteRenderer
            if (selectedObject != null && selectedObject.GetComponent<SpriteRenderer>() != null)
            {
                // Get the SpriteRenderer component from the selected GameObject
                Sprite selectedSprite = selectedObject.GetComponent<SpriteRenderer>().sprite;

                // Assign the selected sprite to the zombie's SpriteRenderer
                this.spriteRenderer.sprite = selectedSprite;                

                // Set sorting order based on the object's name
                switch (this.gameObject.name)
                {
                    case "Shirt":
                        this.spriteRenderer.sortingOrder = 4;
                        break;
                    case "Hair":
                        this.spriteRenderer.sortingOrder = 7;
                        break;
                    case "Body":
                        this.spriteRenderer.sortingOrder = 3;
                        break;
                    case "Face":
                        this.spriteRenderer.sortingOrder = 6;
                        break;
                    case "Head":
                        this.spriteRenderer.sortingOrder = 5;
                        break;
                }
                //Debug.Log("Random sprite selected: " + selectedSprite.name + " Order:"+this.spriteRenderer.sortingOrder+"\n"+", "+this.spriteRenderer.transform.position);
            }
            else
            {
                //Debug.LogWarning("Selected object at index " + randomIndex + " does not have a SpriteRenderer.");
            }
        }
        else
        {
            //Debug.LogWarning("spriteRandomRenderers list is empty.");
        }
    }
}