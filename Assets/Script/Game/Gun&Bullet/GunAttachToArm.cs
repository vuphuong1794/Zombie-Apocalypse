using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class GunAttachToArm : MonoBehaviour
{
    public GameObject RightHand;
    public float count = 0;
    // Start is called before the first frame update
    void Start()
    {
        this.transform.position=RightHand.transform.position;
        count = 0;
    }

    // Update is called once per frame
    void Update()
    {
        this.transform.position = RightHand.transform.position;
        if (count >3)
        {
            //Debug.Log(this.name+": "+this.transform.position+" "+this.transform.localPosition+", "+RightHand.name+": "+ RightHand.transform.position+" "+RightHand.transform.localPosition);
            count = 0;
        }
        count += Time.deltaTime;
    }

}