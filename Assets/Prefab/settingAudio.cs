using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SettingAudio : MonoBehaviour
{
    [SerializeField] Slider slider;
    [SerializeField] Text textUI;
    private static SettingAudio instance; // Đảm bảo chỉ có một instance
    private float num;

    private void Awake()
    {
        // Kiểm tra nếu đã có instance, phá hủy GameObject mới
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }

        // Gán instance và giữ lại GameObject
        instance = this;
        DontDestroyOnLoad(gameObject);
    }

    void Start()
    {
        if (!PlayerPrefs.HasKey("sldkey"))
        {
            PlayerPrefs.SetFloat("sldkey", 1f);
        }

        // Load âm thanh khi lần đầu khởi chạy
        loadAudio();
    }

    public void ChangeAudio()
    {
        num = slider.value * 100;
        textUI.text = num.ToString("0") + "%";
        AudioListener.volume = slider.value;
        saveAudio();
    }

    public void saveAudio()
    {
        PlayerPrefs.SetFloat("sldkey", slider.value);
        loadAudio();
    }

    public void loadAudio()
    {
        slider.value = PlayerPrefs.GetFloat("sldkey");
    }
}
