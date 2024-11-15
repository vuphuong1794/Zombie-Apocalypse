using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class HealthController : MonoBehaviour
{
    [SerializeField]
    private float _currentHealth;

    [SerializeField]
    private float _maximumHealth;
    [SerializeField] private bool canExceedMaxHealth = false; // Có cho phép máu vượt quá giới hạn không

    public GameObject Canvas;
    private HealthBarUI healthBarUI;
    private GameObject gameOverBroad;
    public Sound[] sounds;
    private float timeCount = 0f;
    private float soundCooldown = 3f;
    public float CurrentHealth => _currentHealth;
    public float MaximumHealth => _maximumHealth;

    void Start()
    {
        Canvas canvas = Instantiate(Canvas).GetComponent<Canvas>();
        healthBarUI = canvas.GetComponentInChildren<HealthBarUI>();
        // Tìm GameOverBoard trong Canvas bằng tên hoặc qua cấu trúc Hierarchy
        Transform gameOverTransform = canvas.transform.Find("Game Over Board");
        if (gameOverTransform != null)
        {
            gameOverBroad = gameOverTransform.gameObject;
            gameOverBroad.SetActive(false); // Ẩn màn hình Game Over lúc đầu
        }
        else
        {
            Debug.LogError("GameOverBoard not found in Canvas!");
        }

        SetupAudioSources();
        _currentHealth = _maximumHealth;
        UpdateHealthBar();
    }

    public void TakeDamage(float damageAmount)
    {
        if (_currentHealth == 0) return;
        if (IsInvincible) return;

        _currentHealth -= damageAmount;
        PlayDamageSound();

        if (_currentHealth < 0)
        {
            _currentHealth = 0;
        }

        if (_currentHealth == 0)
        {
            OnDied.Invoke();
            if (gameOverBroad != null)
            {
                gameOverBroad.SetActive(true); // Hiển thị màn hình Game Over
            }
        }
        else
        {
            OnDamaged.Invoke();
        }

        if (healthBarUI != null)
        {
            healthBarUI.SetHealth(_currentHealth);
        }
    }


    private void SetupAudioSources()
    {
        foreach (Sound s in sounds)
        {
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.clip;
            s.source.volume = 1f;
            s.source.pitch = 1f;
            s.source.playOnAwake = false;
        }
    }

    public float RemainingHealthPercentage
    {
        get
        {
            return _currentHealth / _maximumHealth;
        }
    }

    public bool IsInvincible { get; set; }

    public UnityEvent OnDied;

    public UnityEvent OnDamaged;
    private object instanate;

    private void Update()
    {
        timeCount += Time.deltaTime;
    }


    private void UpdateHealthBar()
    {
        if (healthBarUI != null)
        {
            healthBarUI.SetHealth(_currentHealth);
        }
    }

    public void AddHealth(float amountToAdd)
    {
        // Nếu đã ở máu tối đa thì không hồi nữa
        if (_currentHealth >= _maximumHealth)
        {
            return;
        }

        // Tính toán lượng máu sau khi hồi
        float newHealth = _currentHealth + amountToAdd;

        // Đảm bảo máu không vượt quá giới hạn tối đa
        _currentHealth = Mathf.Min(newHealth, _maximumHealth);

        // Cập nhật thanh máu
        UpdateHealthBar();

        // Hiệu ứng hồi máu (nếu có)
        PlayHealEffect();
    }

    private void PlayHealEffect()
    {
        // Thêm hiệu ứng particle hoặc âm thanh khi hồi máu
        ParticleSystem healEffect = GetComponent<ParticleSystem>();
        if (healEffect != null)
        {
            healEffect.Play();
        }
    }

    private void PlayDamageSound()
    {
        if (timeCount >= soundCooldown)
        {
            StartCoroutine(PlayRandomDamageSound());
            timeCount = 0f;
        }
    }
    private IEnumerator PlayRandomDamageSound()
    {
        int randomSoundIndex = Random.Range(0, sounds.Length);
        Sound selectedSound = sounds[randomSoundIndex];

        if (selectedSound != null && selectedSound.source != null && !selectedSound.source.isPlaying)
        {
            selectedSound.source.Play();
            yield return new WaitForSeconds(selectedSound.source.clip.length);
        }
    }

    public void TakeAbilityDamage(float abilityDamageAmount)
    {
        if (_currentHealth == 0)
        {
            return;
        }

        _currentHealth -= abilityDamageAmount;

        if (_currentHealth < 0)
        {
            _currentHealth = 0;
        }

        if (_currentHealth == 0)
        {
            OnDied.Invoke();
        }

        if (healthBarUI != null)
        {
            healthBarUI.SetHealth(_currentHealth);
        }
    }

}