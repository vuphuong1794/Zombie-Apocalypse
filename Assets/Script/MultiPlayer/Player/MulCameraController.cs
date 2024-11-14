using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
//using Mirror;
using Unity.Netcode;

public class MulCameraController : NetworkBehaviour
{
    public GameObject cameraHolder;
    public Vector3 offset;

    public override void OnNetworkSpawn()
    {
        if (IsOwner && cameraHolder != null)
        {
            cameraHolder.SetActive(true);
        }
    }

    // Update is called once per frame
    public void Update()
    {
        if (!IsOwner) return; // Only runs for the client that is the owner of this object

        if (SceneManager.GetActiveScene().name == "Multiplayer Gamemode" && cameraHolder != null)
        {
            cameraHolder.transform.position=transform.position + offset;
        }
    }
}
