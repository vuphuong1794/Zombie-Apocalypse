using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChestHealthController : MonoBehaviour
{
    //public delegate void EnemyDestroyedHandler();
    //public event EnemyDestroyedHandler OnEnemyDestroyed;

    [SerializeField] private float _currentHealth;
    [SerializeField] private float _maximumHealth;

    public ParticleSystem damageParticlePrefab;

    public Sound[] sounds;
    private float timeCount = 0f;
    private float soundCooldown = 3f;

    private void Start()
    {
        SetupAudioSources();
        _currentHealth = _maximumHealth;

    }

    private void Update()
    {
        timeCount += Time.deltaTime;
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

    public void TakeDamage(float damageAmount)
    {
        _currentHealth -= damageAmount;

        PlayDamageEffect();
        PlayDamageSound();

        if (_currentHealth <= 0)
            Die();
    }

    private void PlayDamageEffect()
    {
        if (damageParticlePrefab != null)
        {
            ParticleSystem damageEffect = Instantiate(damageParticlePrefab, transform.position, Quaternion.identity);
            damageEffect.Play();
            Destroy(damageEffect.gameObject, damageEffect.main.duration);
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
        if(sounds != null && sounds.Length > 0)
        {
            int randomSoundIndex = Random.Range(0, sounds.Length);
            Sound selectedSound = sounds[randomSoundIndex];

            if (selectedSound != null && selectedSound.source != null && !selectedSound.source.isPlaying)
            {
                selectedSound.source.Play();
                yield return new WaitForSeconds(selectedSound.source.clip.length);
            }
        }
        else
        {
            Debug.LogWarning("No sounds available to play.");
        }
    }

    private void Die()
    {
        Debug.Log("chest open");
        //OnEnemyDestroyed?.Invoke();
        Destroy(gameObject);
    }
}
