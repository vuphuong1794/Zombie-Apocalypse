using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UsernameManager : MonoBehaviour
{   
    [SerializeField]
    private SceneController _sceneController;

    [SerializeField] TMP_InputField ipUsename;

    public void SetUsername()
    {
        string username = ipUsename.text;
        if (!string.IsNullOrEmpty(username))
        {
            PlayerPrefs.SetString("PlayerUsername", username); // Lưu tên người chơi
            PlayerPrefs.Save();

            // Chuyển sang scene Game
            _sceneController.LoadScene("Game");
        }
        else
        {
            Debug.LogWarning("Username cannot be empty!");
        }

    }
}
