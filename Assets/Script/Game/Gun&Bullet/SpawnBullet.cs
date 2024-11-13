using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnBullet : MonoBehaviour
{
    public GameObject itemDrop;

    private void OnDestroy()
    {
        // Tạo instance mới của itemDrop trước
        GameObject newItem = Instantiate(itemDrop, gameObject.transform.position, gameObject.transform.rotation);

        // Sau đó set scale cho instance mới này
        newItem.transform.localScale = new Vector3(6.088f, 7.3f, 1f);
    }
}