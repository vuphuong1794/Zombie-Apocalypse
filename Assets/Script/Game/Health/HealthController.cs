using System.Collections;
using System.Collections.Generic;
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

    void Start()
    {
        _currentHealth = _maximumHealth;
        if (healthBarUI != null)
        {
            healthBarUI.SetMaxHealth(_maximumHealth);
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