using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField]
    private GameObject _enemyPrefab;

    // Biến lưu lại kẻ thù đã spawn
    private GameObject _spawnedEnemy;

    // Khi Player vào vùng spawner
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player" && _spawnedEnemy == null)
        {
            // Tạo enemy tại vị trí spawner
            _spawnedEnemy = Instantiate(_enemyPrefab, transform.position, Quaternion.identity);
        }
    }

    // Khi Player rời khỏi vùng spawner
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            // Xóa enemy nếu Player rời khỏi vùng
            if (_spawnedEnemy != null)
            {
                Destroy(_spawnedEnemy);
            }
        }
    }
}
