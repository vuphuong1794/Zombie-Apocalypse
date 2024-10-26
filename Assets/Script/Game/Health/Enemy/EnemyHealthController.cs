using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class EnemyHealthController : MonoBehaviour
{
    // Khai báo delegate cho sự kiện tiêu diệt
    public delegate void EnemyDestroyedHandler();
    public event EnemyDestroyedHandler OnEnemyDestroyed;

    [SerializeField]
    private float _currentHealth;

    [SerializeField]
    private float _maximumHealth;

    public EnemyHealthBarUI enemyHealthBarUI;

    private ExplodeEnemyAbility explodeAbility; // Biến lưu ExplodeEnemyAbility

    void Start()
    {
        _currentHealth = _maximumHealth;
        if (enemyHealthBarUI != null)
        {
            enemyHealthBarUI.SetMaxHealth(_maximumHealth);
        }
        explodeAbility = GetComponent<ExplodeEnemyAbility>();
    }

    public float RemainingHealthPercentage
    {
        get
        {
            return _currentHealth / _maximumHealth;
        }
    }

    private Object explosionRef;

    public void TakeDamage(float damageAmount)
    {
        // Ngăn chặn mất máu nếu đang trong quá trình nổ
        if (explodeAbility != null && explodeAbility.isExploding)
            return;

        if (_currentHealth == 0)
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
            //StopAllCoroutines(); // Dừng tất cả các coroutine của zombie
            Die();  
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

    // Hàm xử lý khi kẻ thù chết
    private void Die()
    {
        // Kiểm tra nếu có sự kiện đã được gán, thì gọi sự kiện
        if (OnEnemyDestroyed != null)
        {
            OnEnemyDestroyed();
        }

        // Hủy đối tượng sau khi tiêu diệt
        Destroy(gameObject);
    }
}