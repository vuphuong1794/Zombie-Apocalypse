using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponHolder : MonoBehaviour
{
    int totalWeapon;
    int currentIndex;

    public GameObject[] guns;
    public GameObject weaponHolder;
    public GameObject currentGun;

    private void Start()
    {
        totalWeapon = weaponHolder.transform.childCount;
        guns = new GameObject[totalWeapon];

        for (int i = 0; i < totalWeapon; i++)
        {
            guns[i] = weaponHolder.transform.GetChild(i).gameObject;
            guns[i].SetActive(false);
        }
        guns[0].SetActive(true);
        currentGun = guns[0];
        currentIndex = 0;
    }

    private void Update()
    {
        // Check for input keys
        if (Input.GetKeyDown(KeyCode.Alpha1)) // Press '1'
        {
            SwitchGun(0);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2)) // Press '2'
        {
            SwitchGun(1);
        }
    }

    private void SwitchGun(int index)
    {
        if (index < 0 || index >= totalWeapon) return; // Validate index

        guns[currentIndex].SetActive(false); // Deactivate current gun
        currentIndex = index; // Update current index
        guns[currentIndex].SetActive(true); // Activate new gun
    }
}
