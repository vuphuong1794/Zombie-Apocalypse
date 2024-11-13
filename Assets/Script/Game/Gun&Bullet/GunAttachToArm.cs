using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunAttachToArm : MonoBehaviour
{
    public GameObject RightHand;
    
    // Start is called before the first frame update
    void Start()
    {
        this.transform.position=RightHand.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        this.transform.position = RightHand.transform.position;
    }

}