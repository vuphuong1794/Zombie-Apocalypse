using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunAttachToArm : MonoBehaviour
{
    public GameObject RightHand; // Tay phải của nhân vật
    public GameObject[] Bullet; // Mảng các loại đạn

    // Start is called before the first frame update
    void Start()
    {
        // Đặt vị trí và góc quay của súng theo tay phải
        AttachGunToHand();
    }

    // Update is called once per frame
    void Update()
    {
        // Cập nhật vị trí và góc quay của súng theo tay phải
        AttachGunToHand();
    }

    // Phương thức để gắn súng vào tay
    private void AttachGunToHand()
    {
        this.transform.position = RightHand.transform.position;
        this.transform.rotation = RightHand.transform.rotation;
    }

    public void Shooting()
    {
        // Tạo viên đạn mới tại vị trí của súng với góc quay của súng
        Instantiate(Bullet[Random.Range(0, Bullet.Length)], transform.position, transform.rotation);
    }
}
