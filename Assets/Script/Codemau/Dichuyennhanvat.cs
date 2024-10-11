using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class SantaController : MonoBehaviour
{
    public Animator AnimationSanta;
    public Rigidbody2D body;
    private Vector3 newScales;
    private Vector3 defPos;
    private Vector2 vectorThroughTime=new Vector2(0f,0f);
    private float maxSpeed = 10;
    private float timeCount=0, timeIncreaseSpeed=0.1f;
    public CounterScript counterScript;

    public Vector3 speedx;
    public Vector3 speedy = new Vector3(0, 500, 0);
    // Start is called before the first frame update
    void Start()
    {
        speedx = new Vector3(22.5f, 0, 0);
        speedy = new Vector3(0, 20, 0);
        AnimationSanta = GetComponentInChildren<Animator>();
        body= GetComponent<Rigidbody2D>();
        body.gravityScale = 0;
        float scaleValue = 0.3f;
        newScales = new Vector3(scaleValue,scaleValue,scaleValue);
        transform.localScale = newScales;
        defPos = new Vector3(-30f, 17f, -0.5f);
        transform.localPosition = defPos;
        counterScript = GameObject.FindGameObjectWithTag("ScoreCounter").GetComponent<CounterScript>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!counterScript.GameIsOver && transform.position.y>=-35 && transform.position.y<=40 ) 
        {
            flying();
        }
        else
        {
            counterScript.GameOver();
            Debug.Log("Game is over");
            this.enabled = false;
        }

    }

    void flying()
    {
        if (vectorThroughTime.y < maxSpeed)
        {
            if (timeCount < timeIncreaseSpeed)
            {
                timeCount += Time.deltaTime;
            }
            else
            {
                timeCount = 0;
                vectorThroughTime.y += 0.2f;
                body.gravityScale += 0.1f;
            }
        }

        if (Input.GetKey(KeyCode.A)&& transform.position.y<=65)
        {
            gameObject.transform.position += Vector3.left*Time.deltaTime;
        }
        if (Input.GetKey(KeyCode.S) && transform.position.y <= 65)
        {
            gameObject.transform.position += Vector3.down * Time.deltaTime;
        }
        if (Input.GetKey(KeyCode.D) && transform.position.y <= 65)
        {
            gameObject.transform.position += speedx * Time.deltaTime;
        }
        if (Input.GetKey(KeyCode.W) && transform.position.y <= 65)
        {
            gameObject.transform.position += speedy * Time.deltaTime;
        }
    }



}
