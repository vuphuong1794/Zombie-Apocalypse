using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameOverScreen : MonoBehaviour
{
    public Text pointsText;

    public void SetUp(int score)
    {
        gameObject.SetActive(true);
        pointsText.text = score.ToString() + " SCORES";
    }

    //chuyển đến scene Game
    public void RestartButton()
    {
        SceneManager.LoadScene("Game");
    }

    //chuyển đến scene Main Menu
    public void ExitButton()
    {
        SceneManager.LoadScene("Main Menu");
    }
}
