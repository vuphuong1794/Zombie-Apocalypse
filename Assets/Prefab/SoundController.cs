using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement; // Import SceneManager for scene handling  

public class SoundController : MonoBehaviour
{
    [SerializeField] private AudioSource[] audioSources; // Ensure this is properly assigned in the Inspector  
    [SerializeField] private Slider slider;
    [SerializeField] private Text textUI;

    // Static instance to keep a singular SoundController  
    private static SoundController instance;

    private void Awake()
    {
        // If another instance exists, destroy the new one  
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }

        // Assign instance and don't destroy on load  
        instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public void Start()
    {
        if (audioSources == null || audioSources.Length == 0)
        {
            Debug.LogError("AudioSources are not assigned!");
            return;
        }

        float savedVolume = PlayerPrefs.GetFloat("sldkey", 1f); // Load saved volume  
        slider.value = savedVolume;
        UpdateVolume(savedVolume);

        // Subscribe to the sceneLoaded event  
        SceneManager.sceneLoaded += OnSceneLoaded;
        OnSceneLoaded(SceneManager.GetActiveScene(), LoadSceneMode.Single); // Call it with LoadSceneMode  
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // Check the current scene name and activate the appropriate AudioSource  
        string currentSceneName = scene.name;

        if (currentSceneName == "Game" || currentSceneName == "Multiplayer_Gamemode")
        {
            Debug.Log("Changed music for game: " + audioSources[1].name);
            ActivateAudioSource(1);
        }
        else
        {
            Debug.Log("Changed music for game: " + audioSources[0].name);
            ActivateAudioSource(0);
        }
    }

    private void ActivateAudioSource(int index)
    {
        for (int i = 0; i < audioSources.Length; i++)
        {
            if (audioSources[i] != null)
            {
                audioSources[i].Stop(); // Stop all audio  
                audioSources[i].mute = true; // Mute all audio  
            }
        }

        // Activate the selected AudioSource  
        if (index >= 0 && index < audioSources.Length)
        {
            audioSources[index].mute = false; // Unmute the chosen audio source  
            audioSources[index].Play(); // Start playing the selected AudioSource  
            audioSources[index].loop = true; // Loop the selected audio source  
        }
        else
        {
            Debug.LogError("Invalid AudioSource index: " + index);
        }
    }

    public void OnSliderValueChanged()
    {
        float volume = slider.value;
        UpdateVolume(volume);
        // Save the volume setting  
        PlayerPrefs.SetFloat("sldkey", volume);
        PlayerPrefs.Save(); // Ensure that the PlayerPrefs are saved  
    }

    private void UpdateVolume(float volume)
    {
        AudioListener.volume = volume;
        textUI.text = (volume * 100).ToString("0") + "%"; // Update UI text  
    }

    private void OnDestroy()
    {
        // Unsubscribe from the sceneLoaded event when the object is destroyed  
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }
}