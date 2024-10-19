using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class EnemyHealthController : MonoBehaviour
{
    [SerializeField]
    private float _currentHealth;

    [SerializeField]
    private float _maximumHealth;

    public EnemyHealthBarUI enemyHealthBarUI;

    void Start()
    {
        _currentHealth = _maximumHealth;
        if (enemyHealthBarUI != null)
        {
            enemyHealthBarUI.SetMaxHealth(_maximumHealth);
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

        if (_currentHealth < 0)
        {
            _currentHealth = 0;
        }

        if (_currentHealth == 0)
        {
            //OnDied.Invoke();
            Destroy(gameObject);
        }
        else
        {
            OnDamaged.Invoke();
        }

        if (enemyHealthBarUI != null)
        {
            enemyHealthBarUI.SetHealth(_currentHealth);
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

        if (enemyHealthBarUI != null)
        {
            enemyHealthBarUI.SetHealth(_currentHealth);
        }
    }
}