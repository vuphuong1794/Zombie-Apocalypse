using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bulletController : MonoBehaviour
{
    [SerializeField] public int bulletNumber;
    // Start is called before the first frame update
    void Start()
    {
        bulletNumber = Random.Range(0, 20);   
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
