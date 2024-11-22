using UnityEngine;
using Unity.Netcode; // Thêm nếu bạn đang dùng Netcode for GameObjects

public class PlayerController : NetworkBehaviour
{
    public Camera playerCamera;

    void Start()
    {
        Debug.Log("Find : " + this.gameObject.name + "_Camera");
        playerCamera = GameObject.Find(this.gameObject.name + "_Camera").GetComponent<Camera>();
        if (IsLocalPlayer)
        {
            playerCamera.enabled = true;  // Bật camera cho local player
        }
        else
        {
            playerCamera.enabled = false; // Tắt camera cho các player khác
        }
    }
    private void LateUpdate()
    {
        if (playerCamera == null)
        {
            playerCamera = GameObject.Find(this.gameObject.name + "_Camera").GetComponent<Camera>();
        }
    }
}
