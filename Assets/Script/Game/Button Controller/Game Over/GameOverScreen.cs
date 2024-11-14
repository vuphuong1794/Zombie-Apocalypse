using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameOverScreen : MonoBehaviour
{
    public Text pointsText;

    [SerializeField]
    private SceneController _sceneController;

    public void SetUp(int score)
    {
        gameObject.SetActive(true);
        pointsText.text = score.ToString() + " SCORES";
    }

    //chuyển đến scene Game
    public void RestartButton()
    {
        _sceneController.LoadScene("Game");
    }

    //chuyển đến scene Main Menu
    public void ExitButton()
    {
        _sceneController.LoadScene("Main Menu");
    }
}
