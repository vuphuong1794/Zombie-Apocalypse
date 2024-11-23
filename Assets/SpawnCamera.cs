using System.Collections;
using System.Collections.Generic;
using Unity.Multiplayer.Samples.Utilities;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SpawnCamera : NetworkBehaviour
{
    // Start is called before the first frame update
    public GameObject Mycamera;
    private void Start()
    {
        if (IsOwner) // Ensure this runs only on the server  
        {
            Debug.Log("Onsceneloaded in multi mode");
            GameObject myCamera = Instantiate(Mycamera, this.transform.position, this.transform.rotation);
            myCamera.name = this.gameObject.name + "_Camera";

            myCamera.GetComponent<CameraFollow>().Player = this.gameObject;
            Debug.Log(myCamera.name + "is spawned");
        }
    }
    

}
