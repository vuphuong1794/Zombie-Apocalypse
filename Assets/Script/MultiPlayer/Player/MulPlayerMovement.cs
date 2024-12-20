﻿using UnityEngine.InputSystem;
using UnityEngine;
using Unity.Netcode;

public class MulPlayerMovement : NetworkBehaviour
{
    [Header("Required References")]
    [SerializeField] private UI_Inventory uiInventory;

    [Header("Movement Settings")]
    [SerializeField] private float _speed = 5f;

    private Rigidbody2D _rigidbody;
    private Vector2 _movementInput;
    private Inventory inventory;
    private Vector3 otherPos;

    public GameObject Mycamera;

    private void Start()
    {
        if (IsOwner) // Only update for the owning player  
        {
            this.gameObject.name += this.OwnerClientId;
            Debug.Log("MulMOVEMENT: Owner Client ID: " + OwnerClientId);
            Debug.Log("MulMOVEMENT find camera by name:" + this.gameObject.name + "_Camera");
        }

        _rigidbody = GetComponent<Rigidbody2D>();
        inventory = GetComponent<Inventory>();

        uiInventory.SetInventory(inventory);
        uiInventory.SetPlayer(this);
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        ItemWorld itemWorld = collider.GetComponent<ItemWorld>();
        if (itemWorld != null)
        {
            Item item = itemWorld.GetItem();

            if (item.itemType == Item.ItemType.Rifle || item.itemType == Item.ItemType.Sniper || item.itemType == Item.ItemType.Grenade)
            {
                bool hasPistol = inventory.GetItemList().Find(i => i.itemType == Item.ItemType.Pistol) != null;
                int gunCount = inventory.GetItemList().FindAll(i => i.itemType == Item.ItemType.Rifle || i.itemType == Item.ItemType.Sniper || i.itemType == Item.ItemType.Grenade).Count;

                if (hasPistol && gunCount < 1)
                {
                    inventory.AddItem(item);
                    itemWorld.DestroySelf();
                }
            }
            else
            {
                inventory.AddItem(item);
                itemWorld.DestroySelf();
            }
        }
    }

    private void LateUpdate()
    {
        if (Mycamera == null)
        {
            Mycamera = GameObject.Find(this.gameObject.name + "_Camera");
        }

        if (IsClient)
        {
            MovePlayer();
            RotateTowardsMouse();
            SyncPlayerStateServerRpc(transform.position, transform.rotation);
        }
        else
        {
            // Update the position and rotation of the non-owner  
            transform.position = otherPos;
            // Ensure the rotation is also synchronized  
            transform.rotation = Quaternion.Euler(0, 0, otherRotation);
        }
    }

    [ServerRpc]
    void SyncPlayerStateServerRpc(Vector3 pos, Quaternion rot)
    {
        otherPos = pos;
        otherRotation = rot.eulerAngles.z; // Store only the Z rotation  
    }

    private void MovePlayer()
    {
        Vector2 targetPosition = _rigidbody.position + _movementInput * _speed * Time.fixedDeltaTime;
        _rigidbody.MovePosition(targetPosition);
    }

    private void RotateTowardsMouse()
    {
        // Use Mycamera directly without .main  
        Vector3 mousePos = Mycamera.GetComponent<Camera>().ScreenToWorldPoint(Input.mousePosition);
        Vector2 direction = mousePos - transform.position;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, angle);
    }

    private void OnMove(InputValue inputValue)
    {
        _movementInput = inputValue.Get<Vector2>();
    }

    public Vector3 GetPosition()
    {
        return transform.position;
    }

    private float otherRotation; // Variable to store the rotation for syncing  
}