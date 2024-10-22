using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AcidZombieAbility : MonoBehaviour
{
    [SerializeField]
    private float _acidDamageAmount = 10;

    [SerializeField]
    private float _damageInterval = 2f; // Thời gian giữa các lần gây sát thương

    [SerializeField]
    private int _damageTimes = 3; // Số lần gây sát thương

    private bool _isDamaging = false; // Cờ để theo dõi quá trình sát thương
    private Coroutine _damageCoroutine; // Để dừng hoặc bắt đầu lại coroutine khi cần

    // Khi player chạm vào zombie, bắt đầu quá trình gây sát thương
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.GetComponent<PlayerMovement>())
        {
            var healthController = collision.gameObject.GetComponent<HealthController>();

            // Nếu chưa có sát thương nào đang diễn ra, bắt đầu
            if (!_isDamaging)
            {
                _isDamaging = true;
                _damageCoroutine = StartCoroutine(InfectOverTime(healthController));
            }
        }
    }

    // Ngừng va chạm nhưng vẫn tiếp tục gây sát thương sau khi player rời đi
    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.GetComponent<PlayerMovement>())
        {
            // Không cần dừng coroutine, vì sát thương vẫn tiếp tục thêm vài lần sau đó
            _isDamaging = false;
        }
    }

    // Coroutine để gây sát thương dần dần ngay cả khi player rời khỏi zombie
    private IEnumerator InfectOverTime(HealthController healthController)
    {
        int timesDamaged = 0;

        // Gây sát thương liên tục cho tới khi đạt số lần quy định
        while (timesDamaged < _damageTimes)
        {
            healthController.TakeAbilityDamage(_acidDamageAmount);
            timesDamaged++;

            // Chờ trong 2 giây (hoặc thời gian tùy chỉnh) trước khi gây sát thương tiếp
            yield return new WaitForSeconds(_damageInterval);
        }

        // Sau khi sát thương đủ số lần, dừng lại
        _isDamaging = false;
    }
}
