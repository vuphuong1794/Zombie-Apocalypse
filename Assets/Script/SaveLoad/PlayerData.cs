using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlayerData
{
    public int point;
    public int health;
    public float[] position;
    public int currentBullet;

    public PlayerData (PlayerSave playerSave)
    {
        point = playerSave.point;
        health = playerSave.health;

        position = new float[3];
        position[0] = playerSave.transform.position.x;
        position[1] = playerSave.transform.position.y;
        position[2] = playerSave.transform.position.z;

        currentBullet = playerSave.currentBullets;
    }

}
