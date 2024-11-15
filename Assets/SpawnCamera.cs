using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnCamera : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject Mycamera;
    void Start()
    {
        GameObject myCamera = Instantiate(Mycamera, this.transform.position, this.transform.rotation);
        myCamera.transform.position = this.transform.position;
        myCamera.GetComponent<CameraFollow>().Player = this.gameObject;
        myCamera.name=this.gameObject.name+"_Camera";
    }


}
