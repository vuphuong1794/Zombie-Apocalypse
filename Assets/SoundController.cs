using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SoundController : MonoBehaviour
{
    [SerializeField] private AudioSource audioSource;
    [SerializeField] Slider slider;
    [SerializeField] Text textUI;
    private float num;

    // Static instance để giữ SoundController duy nhất
    private static SoundController instance;

    private void Awake()
    {
        // Nếu đã có instance khác, phá hủy đối tượng mới
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }

        // Gán instance và không phá hủy khi chuyển scene
        instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public void Start()
    {
        if (audioSource == null)
        {
            Debug.LogError("AudioSource chưa được gán!");
            return;
        }

        // Phát nhạc nếu chưa phát
        
            slider.value = PlayerPrefs.GetFloat("sldkey");
            num = slider.value * 100;
            textUI.text = num.ToString("0") + "%";
            AudioListener.volume = slider.value;
            audioSource.Play();
            audioSource.loop = true;
        
    }
}
