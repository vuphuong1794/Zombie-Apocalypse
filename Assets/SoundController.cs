using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundController : MonoBehaviour
{
    [SerializeField] private AudioSource audioSource;

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
        if (!audioSource.isPlaying)
        {
            audioSource.Play();
            audioSource.loop = true;
        }
    }
}
