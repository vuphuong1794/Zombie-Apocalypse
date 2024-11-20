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

    public HealthBarUI healthBarUI;
    public Sound[] sounds;
    private float timeCount = 0f;
    private float soundCooldown = 3f;

    public float CurrentHealth => _currentHealth;
    public float MaximumHealth => _maximumHealth;

    void Start()
    {
        SetupAudioSources();
        _currentHealth = _maximumHealth;
        UpdateHealthBar();
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

    public UnityEvent OnRevevi;

    private void Update()
    {
        timeCount += Time.deltaTime;
    }

    public void TakeDamage(float damageAmount)
    {
        if (_currentHealth == 0)
        {

            return;
        }

        if (IsInvincible)
        {
            return;
        }

        _currentHealth -= damageAmount;
        PlayDamageSound();


        if (_currentHealth < 0)
        {
            _currentHealth = 0;
        }

        if (_currentHealth == 0)
        {
            OnDied.Invoke();
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
    public void Revive()
    {
        // Đặt lại máu về mức tối đa
        _currentHealth = _maximumHealth;

        // Cập nhật thanh máu
        UpdateHealthBar();

        OnRevevi.Invoke();


        
    }

}