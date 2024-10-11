using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChimneySpawner : MonoBehaviour
{
    public GameObject[] Chimney;
    public float TimeSpawner;
    public float Timer;
    public float DefPosX, DefPosY, DefPosZ;
    public CounterScript counterScript;
    // Start is called before the first frame update
    void Start()
    {
        Timer = 0; TimeSpawner = Random.Range(2,8);
        transform.position = new Vector3(100,-25,0);
        DefPosX = transform.position.x+200;
        DefPosY = transform.position.y;
        DefPosZ = transform.position.z;
        counterScript = GameObject.FindGameObjectWithTag("ScoreCounter").GetComponent<CounterScript>();
        SpawnerChimney();
    }

    // Update is called once per frame
    void Update()
    {
        if (!counterScript.GameIsOver)
        {
            SpawnerChimney();
        }

    }
    void SpawnerChimney()
    {
        if (Timer < TimeSpawner)
        {
            Timer += Time.deltaTime;
        }
        else
        {
            Instantiate(Chimney[Random.Range(0,Chimney.Length)], new Vector3(DefPosX, DefPosY, DefPosZ), transform.rotation);
            Timer = 0;
            TimeSpawner = Random.Range(2, 8);
        }
    }
}
