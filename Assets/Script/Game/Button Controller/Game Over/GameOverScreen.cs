using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class GameOverScreen : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI pointsText;

    private int points=0;

    [SerializeField]
    private SceneController _sceneController;

    public void SetUp()
    {
        int currentScore = ScoreManager.Instance.GetScore();
        gameObject.SetActive(true);
        pointsText.text = currentScore.ToString() + " POINTS";
    }

    //chuyển đến scene Game
    public void RestartButton()
    {
        ScoreManager.Instance.ResetScore();
        _sceneController.LoadScene("Game");
    }

    public void Revive()
    {
        HealthController healthController = FindObjectOfType<HealthController>();
        if (healthController != null)
        {
            healthController.Revive(); // Gọi hàm hồi sinh
        }
        gameObject.SetActive(false);
        
    }



    //chuyển đến scene Main Menu
    public void ExitButton()
    {
        ScoreManager.Instance.ResetScore();
        _sceneController.LoadScene("Main Menu");
    }
}
