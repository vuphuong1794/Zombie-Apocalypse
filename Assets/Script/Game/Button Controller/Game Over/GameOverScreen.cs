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
    public SceneController _sceneController;

    public void Start()
    {
        _sceneController = GameObject.FindGameObjectWithTag("SceneController").GetComponent<SceneController>();
    }
    public void SetUp(int score)
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

    //chuyển đến scene Main Menu
    public void ExitButton()
    {
        ScoreManager.Instance.ResetScore();
        _sceneController.LoadScene("Main Menu");
    }
}
