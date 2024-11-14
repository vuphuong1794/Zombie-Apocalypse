using UnityEngine;

public class CameraFixedRotation : MonoBehaviour
{
    private Quaternion initialRotation;

    void Start()
    {

    }

    void Update()
    {
        this.transform.rotation = new Quaternion(0, 0, 0,0);
    }
}
