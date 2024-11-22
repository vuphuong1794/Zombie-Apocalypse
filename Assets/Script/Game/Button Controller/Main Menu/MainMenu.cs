using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    [SerializeField]
    private SceneController _sceneController;

    public void PlayGame()
    {
        _sceneController.LoadScene("Game");
    }

    public void PlayMultiplayerGamemode()
    {
        _sceneController.LoadScene("Matchmaking");
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
