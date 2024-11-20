using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayerSave : MonoBehaviour
{
    [SerializeField] private HealthController healthController;
    [SerializeField] private TextMeshProUGUI pointsText;
    [SerializeField] private Inventory inventory; // Tham chiếu tới Inventory
     // Biến lưu số đạn hiện tại


    public int curentHealth => (int)healthController.CurrentHealth;
    public int point => ScoreManager.Instance.GetScore();
    public int health;
    public int currentBullets => inventory.GetItemCount(Item.ItemType.grenadeBullet);
    public int StartBullets;

    private void Update()
    {
        if (curentHealth <= 0)
        {
            LoadPlayer();
        }
    }


    public void SavePlayer()
    {
        StartBullets = currentBullets;
        SaveSystem.SavePlayer(this);
    }

    public void LoadPlayer()
    {
        PlayerData data = SaveSystem.LoadPlayer();
        ScoreManager.Instance.SetScore(data.point);
        ScoreManager.Instance.AddScore(0);
        health = 100;
        StartBullets = data.currentBullet;

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
        // Cập nhật lại số đạn trong inventory
        int bulletsToRestore = currentBullets; // Lấy số đạn đã lưu
        inventory.AddItem(new Item { itemType = Item.ItemType.grenadeBullet, amount = StartBullets - bulletsToRestore });
    }
}
