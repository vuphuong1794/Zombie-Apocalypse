using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColliderMiddleCollumnScript : MonoBehaviour
{
    public CounterScript counterScript;
    // Start is called before the first frame update
    void Start()
    {
        counterScript = GameObject.FindGameObjectWithTag("ScoreCounter").GetComponent<CounterScript>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log(collision.gameObject.name);
        if (collision.gameObject.name == "Player")
        {
            counterScript.addScore();
            Destroy(this.gameObject);
        }
    }
}
