using UnityEngine;

public class WeaponHolder : MonoBehaviour
{
    int totalWeapon;
    int currentIndex;
    public GameObject[] guns;
    public GameObject weaponHolder;
    public GameObject currentGun;
    private bool[] weaponUnlocked;

    private Inventory inventory;

    private void Start()
    {
        Debug.Log("Start weapon holder");
        totalWeapon = weaponHolder.transform.childCount;
        guns = new GameObject[totalWeapon];
        weaponUnlocked = new bool[totalWeapon];

        // Lấy reference tới Inventory
        inventory = GetComponentInParent<Inventory>();
        if (inventory != null)
        {
            inventory.OnItemListChanged += Inventory_OnItemListChanged;
        }
        else
        {
            Debug.Log("inventory is null");
        }

        // Khởi tạo tất cả súng
        for (int i = 0; i < totalWeapon; i++)
        {
            guns[i] = weaponHolder.transform.GetChild(i).gameObject;
            guns[i].SetActive(false);
            weaponUnlocked[i] = false;
            Debug.Log($"WeaponHolder: Initialized weapon {i}");
        }

        // Mặc định cầm pistol (index 0)
        weaponUnlocked[0] = true;
        guns[0].SetActive(true);
        currentGun = guns[0];
        currentIndex = 0;

        Debug.Log("Weapon system initialized"); // Debug log
    }

    private void Inventory_OnItemListChanged(object sender, System.EventArgs e)
    {
        Debug.Log("WeaponHolder: Inventory changed"); // Debug log

        // Kiểm tra inventory và unlock súng mới
        foreach (Item item in inventory.GetItemList())
        {
            if (item.IsGun())
            {
                int gunIndex = item.GetGunIndex();
                Debug.Log($"WeaponHolder: Found gun with index: {gunIndex}"); // Debug log
                if (gunIndex >= 0 && gunIndex < totalWeapon)
                {
                    Debug.Log($"WeaponHolder: Unlocking weapon {gunIndex}"); // Debug log
                    UnlockWeapon(gunIndex);
                    // Verify unlock status
                    Debug.Log($"WeaponHolder: Weapon {gunIndex} unlock status: {weaponUnlocked[gunIndex]}");
                }
            }
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1)) // Rifle
        {
            SwitchGun(0);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2)) // Try to switch to another gun if unlocked (not pistol)
        {
            Debug.Log("Trying to switch to a gun other than Pistol.");
            if (weaponUnlocked[1]) // If Sniper or another gun is unlocked (other than pistol)
            {
                SwitchGun(1);
            }
            else
            {
                // Find another gun that's unlocked and not the pistol
                for (int i = 1; i < totalWeapon; i++)
                {
                    if (weaponUnlocked[i])
                    {
                        Debug.Log($"Switching to unlocked gun {i}.");
                        SwitchGun(i);
                        break;
                    }
                }
            }
        }
        else if (Input.GetKeyDown(KeyCode.Alpha3)) // Press '3'
        {
            SwitchGun(2);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha4)) // Press '4'
        {
            SwitchGun(3);
        }
    }

    private void SwitchGun(int index)
    {
        if (index < 0 || index >= totalWeapon) return;
        if (!weaponUnlocked[index]) return;

        Debug.Log($"Switching to gun {index}"); // Debug log
        guns[currentIndex].SetActive(false);
        currentIndex = index;
        guns[currentIndex].SetActive(true);
        currentGun = guns[currentIndex];
    }

    public void UnlockWeapon(int weaponIndex)
    {
        if (weaponIndex >= 0 && weaponIndex < totalWeapon)
        {
            Debug.Log($"WeaponHolder: Attempting to unlock weapon {weaponIndex}"); // Debug log before
            weaponUnlocked[weaponIndex] = true;
            Debug.Log($"WeaponHolder: Weapon {weaponIndex} unlocked status: {weaponUnlocked[weaponIndex]}"); // Debug log after
        }
    }

    private void OnDestroy()
    {
        if (inventory != null)
        {
            inventory.OnItemListChanged -= Inventory_OnItemListChanged;
        }
    }

    public void DeactivateWeapon(int weaponIndex)
    {
        if (weaponIndex >= 0 && weaponIndex < totalWeapon && weaponUnlocked[weaponIndex])
        {
            guns[weaponIndex].SetActive(false);
            weaponUnlocked[weaponIndex] = false;
            Debug.Log($"WeaponHolder: Weapon {weaponIndex} has been deactivated");
        }
    }

}