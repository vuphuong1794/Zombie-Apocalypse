using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pause : MonoBehaviour
{
    [SerializeField]
    private GameObject PausePanel;

    [SerializeField]
    private SceneController _sceneController;

    public void PauseFunction()
    {
        PausePanel .SetActive(true);
        Time.timeScale = 0;
    }

    public void ContinnueFunction()
    {
        PausePanel.SetActive(false);
        Time.timeScale = 1;
    }

    public void TryAgain()
    {
        Time.timeScale = 1;
        _sceneController.LoadScene("Game");
    }

    public void ReturnMainMenu()
    {
        Time.timeScale = 1;
        _sceneController.LoadScene("Main Menu");
    }
}
