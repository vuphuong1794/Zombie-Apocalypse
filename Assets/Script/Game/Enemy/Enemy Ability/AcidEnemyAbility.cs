using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Lớp này đại diện cho khả năng của Zombie Acid
public class AcidZombieAbility : MonoBehaviour
{
    // Số lượng sát thương gây ra bởi acid
    [SerializeField]
    private float _acidDamageAmount = 10;

    // Thời gian giữa các lần gây sát thương (tính bằng giây)
    [SerializeField]
    private float _damageInterval = 2f; // Thời gian giữa các lần gây sát thương

    // Số lần gây sát thương tối đa
    [SerializeField]
    private int _damageTimes = 3; // Số lần gây sát thương

    // Phương thức này được gọi khi có va chạm với người chơi
    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Lấy thành phần HealthController từ đối tượng va chạm
        var healthController = collision.gameObject.GetComponent<HealthController>();

        // Kiểm tra xem đối tượng có HealthController và DamageOverTimeManager đã được khởi tạo
        if (healthController != null && DamageOverTimeManager.Instance != null)
        {
            // Gọi phương thức ApplyAcidDamage trên DamageOverTimeManager để bắt đầu gây sát thương
            DamageOverTimeManager.Instance.ApplyAcidDamage(collision.gameObject, _acidDamageAmount, _damageInterval, _damageTimes);
        }
    }
}
