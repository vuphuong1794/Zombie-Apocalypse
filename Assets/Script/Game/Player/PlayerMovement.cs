using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [Header("Required References")]
    [SerializeField] private UI_Inventory uiInventory;

    [Header("Movement Settings")]
    [SerializeField] private float _speed = 5f;

    private Rigidbody2D _rigidbody;
    private Vector2 _movementInput;
    private Inventory inventory;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody2D>();

        inventory = GetComponent<Inventory>();
        uiInventory.SetInventory(inventory);
        /*
        ItemWorld.SpawnItemWorld(new Vector3(-3, 3,-0.1f), new Item { itemType = Item.ItemType.Gun1, amount = 1 });
        ItemWorld.SpawnItemWorld(new Vector3(-7, 2, -0.1f), new Item { itemType = Item.ItemType.Gun2, amount = 1 });
        ItemWorld.SpawnItemWorld(new Vector3(-6, -2, -0.1f), new Item { itemType = Item.ItemType.HealthPotion, amount = 1 });
        */
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        ItemWorld itemWorld = collider.GetComponent<ItemWorld>();
        if(itemWorld != null)
        {
            //touching item
            inventory.AddItem(itemWorld.GetItem()); //them item vao inventory
            itemWorld.DestroySelf();//xoa item tren ban do

        }
    }

    private void FixedUpdate()
    {
        MovePlayer();
        RotateTowardsMouse();
    }

    private void MovePlayer()
    {
        Vector2 targetPosition = _rigidbody.position + _movementInput * _speed * Time.fixedDeltaTime;
        _rigidbody.MovePosition(targetPosition);
    }

    private void RotateTowardsMouse()
    {
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 direction = mousePos - transform.position;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, angle);
    }

    private void OnMove(InputValue inputValue)
    {
        _movementInput = inputValue.Get<Vector2>();
    }
}