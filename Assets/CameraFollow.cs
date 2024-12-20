using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class CameraFollow : NetworkBehaviour
{
    public GameObject Player;
    // Start is called before the first frame update

    // Update is called once per frame
    void Update()
    {
        // Set the position of the camera, but keep the z value fixed at -2
        Vector3 newPosition = Player.transform.position;
        newPosition.z = -15;  // Fix the z position
        this.transform.position = newPosition;
    }

}
