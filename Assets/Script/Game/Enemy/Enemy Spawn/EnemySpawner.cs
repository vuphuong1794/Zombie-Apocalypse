using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField]
    private GameObject _enemyPrefab;

    // Biến lưu lại kẻ thù đã spawn
    private GameObject _spawnedEnemy;

    // Biến kiểm tra kẻ thù có đang chờ để spawn lại không
    private bool _isWaitingForRespawn = false;

    private void Start()
    {
        _enemyPrefab.transform.SetParent(null);
    }
    // Khi Player vào vùng spawner
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player" && _spawnedEnemy == null && !_isWaitingForRespawn)
        {
            // Tạo enemy tại vị trí spawner
            //_spawnedEnemy = Instantiate(_enemyPrefab, transform.position, Quaternion.identity);
            //_spawnedEnemy.GetComponent<EnemyHealthController>().OnEnemyDestroyed += HandleEnemyDestroyed;
            SpawnEnemy();
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
                _spawnedEnemy = null;   
            }
        }
    }

    // Spawn zombie
    private void SpawnEnemy()
    {
        // Tạo enemy tại vị trí spawner
        _spawnedEnemy = Instantiate(_enemyPrefab, this.transform.position, this.transform.rotation);
        Debug.Log("Spawnzombie object locate at: " + this.transform.position);
        Debug.Log("Zombie spawn at: "+_spawnedEnemy.transform.position);
        _spawnedEnemy.GetComponent<EnemyHealthController>().OnEnemyDestroyed += HandleEnemyDestroyed;
        var explodeAbility = _spawnedEnemy.GetComponent<ExplodeEnemyAbility>();

        // Truyền EnemySpawner vào ExplodeEnemyAbility để quản lý respawn
        if(explodeAbility) explodeAbility.SetSpawner(this);
    }

    // Xử lý khi enemy bị tiêu diệt
    public void HandleEnemyDestroyed()
    {
        // Đặt biến _spawnedEnemy về null và bắt đầu Coroutine để đợi respawn
        _spawnedEnemy = null;
        StartCoroutine(RespawnEnemyAfterDelay(3f));
    }

    // Coroutine để đợi 5 giây trước khi spawn lại enemy
    private IEnumerator RespawnEnemyAfterDelay(float delay)
    {
        _isWaitingForRespawn = true;
        yield return new WaitForSeconds(delay);
        _isWaitingForRespawn = false;

        if (_spawnedEnemy == null)
        {
            // Tạo lại enemy sau khi đợi
            //_spawnedEnemy = Instantiate(_enemyPrefab, transform.position, Quaternion.identity);
            //_spawnedEnemy.GetComponent<EnemyHealthController>().OnEnemyDestroyed += HandleEnemyDestroyed;
            SpawnEnemy();
        }
    }
}
