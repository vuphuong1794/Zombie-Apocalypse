using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Lớp quản lý các hiệu ứng gây sát thương theo thời gian (Damage Over Time)
public class DamageOverTimeManager : MonoBehaviour
{
    // Biến static để giữ instance duy nhất của DamageOverTimeManager
    public static DamageOverTimeManager Instance { get; private set; }

    private void Awake()
    {
        // Khi đối tượng được khởi tạo
        if (Instance == null)
        {
            // Nếu chưa có instance, gán instance hiện tại
            Instance = this;
        }
        else
        {
            // Nếu đã có instance, xóa đối tượng mới tạo để đảm bảo chỉ có một instance
            Destroy(gameObject);
        }
    }

    // HashSet để lưu trữ các đối tượng đã bị ảnh hưởng bởi hiệu ứng acid
    private HashSet<GameObject> _acidAffectedTargets = new HashSet<GameObject>();

    // Phương thức để áp dụng sát thương acid lên một đối tượng
    public void ApplyAcidDamage(GameObject target, float damageAmount, float damageInterval, int times)
    {
        // Ngăn chặn việc áp dụng hiệu ứng acid nhiều lần lên cùng một đối tượng
        if (_acidAffectedTargets.Contains(target)) return;

        // Thêm đối tượng vào danh sách các đối tượng đã bị ảnh hưởng
        _acidAffectedTargets.Add(target);

        // Bắt đầu Coroutine để gây sát thương theo thời gian
        StartCoroutine(ApplyDamageOverTime(target, damageAmount, damageInterval, times));
    }

    // Coroutine để gây sát thương dần dần theo thời gian
    private IEnumerator ApplyDamageOverTime(GameObject target, float damageAmount, float damageInterval, int times)
    {
        // Lấy HealthController từ đối tượng để thực hiện các thao tác liên quan đến sức khỏe
        var healthController = target.GetComponent<HealthController>();

        // Gây sát thương liên tục cho tới khi đạt số lần quy định
        for (int i = 0; i < times && healthController != null; i++)
        {
            // Gọi phương thức để gây sát thương cho đối tượng
            healthController.TakeAbilityDamage(damageAmount);
            // Chờ trong số giây được đặt (damageInterval) trước khi gây sát thương tiếp
            yield return new WaitForSeconds(damageInterval);
        }
        // Xóa đối tượng khỏi danh sách sau khi hiệu ứng kết thúc
        _acidAffectedTargets.Remove(target);
    }
}
