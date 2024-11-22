using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField]
    private List<Transform> spawnPoints; // Các điểm spawn
    [SerializeField]
    private GameObject enemyPrefab; 
    [SerializeField]
    private int maxEnemies = 5; // Số lượng kẻ thù tối đa trên màn hình
    [SerializeField]
    private float spawnInterval = 10f; // Thời gian giữa các lần spawn
    [SerializeField]
    private float minDistanceFromPlayer = 8f; // Khoảng cách tối thiểu từ Player
    [SerializeField]
    private float maxDistanceFromPlayer = 30f; // Khoảng cách tối đa từ Player
    [SerializeField]
    private float destroyDistance = 30f; // Khoảng cách tự hủy zombie

    private List<GameObject> spawnedEnemies = new List<GameObject>();
    private Transform player;

    private void Start()
    {
        // Tìm Player
        player = GameObject.FindGameObjectWithTag("Player").transform;

        // Bắt đầu quá trình spawn định kỳ
        StartCoroutine(SpawnEnemies());
    }
    private void Update()
    {
        CheckEnemiesDistance();
    }

    private IEnumerator SpawnEnemies()
    {
        while (true)
        {
            // Kiểm tra số lượng kẻ thù hiện tại
            spawnedEnemies.RemoveAll(enemy => enemy == null); // Loại bỏ các kẻ thù đã bị tiêu diệt

            // Kiểm tra nếu số lượng enemy đạt giới hạn
            if (spawnedEnemies.Count >= maxEnemies)
            {
                yield return null; // Đợi frame tiếp theo
                continue;
            }

            // Lựa chọn điểm spawn
            Transform spawnPoint = GetValidSpawnPoint();

            if (spawnPoint != null)
            {
                // Spawn ngẫu nhiên một loại enemy
                //GameObject enemyPrefab = enemyPrefabs[Random.Range(0, enemyPrefabs.Length)];
                GameObject spawnedEnemy = Instantiate(enemyPrefab, spawnPoint.position, Quaternion.identity);
                Debug.LogWarning("Spawned" + name);
                spawnedEnemies.Add(spawnedEnemy);
            }

            yield return new WaitForSeconds(spawnInterval);
        }
    }

    private Transform GetValidSpawnPoint()
    {
        // Tìm các điểm spawn hợp lệ
        List<Transform> validPoints = new List<Transform>();

        foreach (Transform spawnPoint in spawnPoints)
        {
            float distanceToPlayer = Vector2.Distance(spawnPoint.position, player.position);

            if (distanceToPlayer > minDistanceFromPlayer && distanceToPlayer < maxDistanceFromPlayer)
            {
                validPoints.Add(spawnPoint);
            }
        }

        // Trả về một điểm spawn ngẫu nhiên trong danh sách hợp lệ
        if (validPoints.Count > 0)
        {
            return validPoints[Random.Range(0, validPoints.Count)];
        }

        return null; // Không có điểm spawn hợp lệ
    }
    private void CheckEnemiesDistance()
    {
        // Kiểm tra khoảng cách từ mỗi zombie tới Player
        for (int i = spawnedEnemies.Count - 1; i >= 0; i--)
        {
            if (spawnedEnemies[i] != null)
            {
                float distanceToPlayer = Vector2.Distance(spawnedEnemies[i].transform.position, player.position);

                if (distanceToPlayer >= destroyDistance)
                {
                    // Xóa zombie nếu quá xa Player
                    Destroy(spawnedEnemies[i]);
                    spawnedEnemies.RemoveAt(i);
                }
            }
        }
    }
}
