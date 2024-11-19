using System.Collections;
using UnityEngine;

public class EnemyHealthController : MonoBehaviour
{
    public delegate void EnemyDestroyedHandler();
    public event EnemyDestroyedHandler OnEnemyDestroyed;

    [SerializeField] private float _currentHealth;
    [SerializeField] private float _maximumHealth;

    public ParticleSystem damageParticlePrefab;
    public EnemyHealthBarUI enemyHealthBarUI;
    private ExplodeEnemyAbility explodeAbility;

    public Sound[] sounds;
    private float timeCount = 0f;
    private float soundCooldown = 3f;

    public GameOverScreen gameOverScreen;

    private void Start()
    {
        SetupAudioSources();
        _currentHealth = _maximumHealth;

        if (enemyHealthBarUI != null)
            enemyHealthBarUI.SetMaxHealth(_maximumHealth);

        explodeAbility = GetComponent<ExplodeEnemyAbility>();
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
        if (explodeAbility != null && explodeAbility.isExploding || _currentHealth <= 0)
            return;

        _currentHealth -= damageAmount;

        PlayDamageEffect();
        PlayDamageSound();

        if (enemyHealthBarUI != null)
            enemyHealthBarUI.SetHealth(_currentHealth);

        if (_currentHealth <= 0)
        {
            Die();
        }
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
        int randomSoundIndex = Random.Range(0, sounds.Length);
        Sound selectedSound = sounds[randomSoundIndex];

        if (selectedSound != null && selectedSound.source != null && !selectedSound.source.isPlaying)
        {
            selectedSound.source.Play();
            yield return new WaitForSeconds(selectedSound.source.clip.length);
        }
    }

    public void AddHealth(float amountToAdd)
    {
        if (_currentHealth == _maximumHealth)
            return;

        _currentHealth = Mathf.Min(_currentHealth + amountToAdd, _maximumHealth);

        if (enemyHealthBarUI != null)
            enemyHealthBarUI.SetHealth(_currentHealth);
    }

    private void Die()
    {
        OnEnemyDestroyed?.Invoke();
        SpawnCorpse spawnCorpse = this.GetComponentInChildren<SpawnCorpse>();
        spawnCorpse.SpawningCorpses();
        Destroy(gameObject);
        ScoreManager.Instance.AddScore(1); // Thêm 1 điểm
    }


}
