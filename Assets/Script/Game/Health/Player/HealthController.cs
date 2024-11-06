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

    public HealthBarUI healthBarUI;
    public Sound[] sounds;
    private float timeCount = 0f;
    private float soundCooldown = 3f;
    void Start()
    {
        SetupAudioSources();

        _currentHealth = _maximumHealth;
        if (healthBarUI != null)
        {
            healthBarUI.SetMaxHealth(_maximumHealth);
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

    public void AddHealth(float amountToAdd)
    {
        if (_currentHealth == _maximumHealth)
        {
            return;
        }

        _currentHealth += amountToAdd;

        if (_currentHealth > _maximumHealth)
        {
            _currentHealth = _maximumHealth;
        }

        if (healthBarUI != null)
        {
            healthBarUI.SetHealth(_currentHealth);
        }
    }
}