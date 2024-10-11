using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Shooting : MonoBehaviour
{
    private Animator[] animatorSanta;
    public float timerCount;
    public float timerTime;
    // Start is called before the first frame update
    void Start()
    {
        animatorSanta = GetComponentsInChildren<Animator>();
        timerCount = 0;
        timerTime=2.5f;
    }

    // Update is called once per frame
    void Update()
    {
        if (timerCount < timerTime)
        {
            timerCount += Time.deltaTime;
        }
        else
        {
            if (Input.GetKeyDown(KeyCode.Mouse1))
            {
                animatorSanta[0].SetTrigger("Throwing");
                timerCount = 0;
            }
            if (Input.GetKeyDown(KeyCode.Mouse0)) {
                animatorSanta[0].SetTrigger("SantaShooting");
                animatorSanta[1].SetTrigger("Shooting");
                timerCount = 0;
            }            
        }
    }
}
