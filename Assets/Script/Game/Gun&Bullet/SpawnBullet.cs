using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnBullet : MonoBehaviour
{
    public GameObject itemDrop;

    AudioManager audioManager;

    private void Awake()
    {
        audioManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManager>();
    }

    private void OnDestroy()
    {
        audioManager.PlaySFX(audioManager.chestBreak);
        // Tạo instance mới của itemDrop trước
        GameObject newItem = Instantiate(itemDrop, gameObject.transform.position, gameObject.transform.rotation);

        // Sau đó set scale cho instance mới này
        newItem.transform.localScale = new Vector3(6.088f, 7.3f, 1f);
    }
}