using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class MulShootGrenade : NetworkBehaviour
{
    [SerializeField] private GameObject bulletPrefab;     // Prefab của viên đạn, phải có NetworkObject
    [SerializeField] private Transform firePos;           // Vị trí spawn viên đạn
    [SerializeField] private float timeBetweenShots = 1f; // Thời gian chờ giữa các lần bắn
    [SerializeField] private float bulletSpeed = 20f;     // Tốc độ bay của viên đạn

    private float timeSinceLastShot = 0f;

    private void Update()
    {
        if (!IsOwner) return; // Chỉ chạy mã này cho người chơi hiện tại

        timeSinceLastShot += Time.deltaTime;

        if (Input.GetMouseButton(0) && timeSinceLastShot >= timeBetweenShots)
        {
            FireBulletServerRpc(); // Gọi ServerRpc để spawn viên đạn
            timeSinceLastShot = 0f;
        }
    }

    [ServerRpc]
    private void FireBulletServerRpc()
    {
        // Tạo viên đạn trên server
        GameObject bulletInstance = Instantiate(bulletPrefab, firePos.position, firePos.rotation);

        // Gán NetworkObject cho viên đạn và spawn để đồng bộ cho các client
        NetworkObject bulletNetworkObject = bulletInstance.GetComponent<NetworkObject>();
        if (bulletNetworkObject != null)
        {
            bulletNetworkObject.Spawn();  // Đồng bộ đạn với các client

            // Thiết lập vận tốc trên server
            Rigidbody2D bulletRb = bulletInstance.GetComponent<Rigidbody2D>();
            if (bulletRb != null)
            {
                Vector2 velocity = firePos.up * bulletSpeed;
                bulletRb.velocity = velocity;

                // Gửi vận tốc tới các client để đồng bộ
                SetBulletVelocityClientRpc(bulletNetworkObject.NetworkObjectId, velocity);
            }
        }
    }

    [ClientRpc]
    private void SetBulletVelocityClientRpc(ulong bulletId, Vector2 velocity)
    {
        // Tìm đối tượng viên đạn theo NetworkObjectId
        NetworkObject bulletNetworkObject = NetworkManager.Singleton.SpawnManager.SpawnedObjects[bulletId];
        if (bulletNetworkObject != null)
        {
            Rigidbody2D bulletRb = bulletNetworkObject.GetComponent<Rigidbody2D>();
            if (bulletRb != null)
            {
                bulletRb.velocity = velocity;
            }
        }
    }
}
