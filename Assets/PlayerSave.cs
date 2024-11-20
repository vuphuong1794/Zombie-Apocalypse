using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayerSave : MonoBehaviour
{
    [SerializeField] private HealthController healthController;
    [SerializeField]
    private TextMeshProUGUI pointsText;
    public int curentHealth => (int)healthController.CurrentHealth;
    public int point => ScoreManager.Instance.GetScore();
    public int health;


    private void Update()
    {
        if (curentHealth <= 0)
        {
            LoadPlayer();
        }
    }


    public void SavePlayer()
    {
        SaveSystem.SavePlayer(this);
    }

    public void LoadPlayer()
    {
        PlayerData data = SaveSystem.LoadPlayer();
        ScoreManager.Instance.SetScore(data.point);
        ScoreManager.Instance.AddScore(0);
        health = 100;


        Vector3 position;
        position.x = data.position[0];
        position.y = data.position[1];
        position.z = data.position[2];

        transform.position = position;

    }
    public void RevivePlayer()
    {
        LoadPlayer();

        healthController.AddHealth(healthController.MaximumHealth);
    }
}
