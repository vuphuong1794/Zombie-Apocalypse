using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField]
    private GameObject Sniper;
    public Vector3 target;
    Vector3 velocity = Vector3.zero;

    [Range(0, 1)]
    public float smoothTime;

    public Vector3 positionOffset;


    private void LateUpdate()
    {
        Vector3 targetPosition = target+positionOffset;
        transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref velocity, smoothTime);

        Camera thisCamera= this.GetComponent<Camera>() ;
        if (Sniper.activeSelf && thisCamera.orthographicSize<7.5f)
        {
            thisCamera.orthographicSize += 0.2f;
        }
        else if (!Sniper.activeSelf && thisCamera.orthographicSize > 5)
        {
            thisCamera.orthographicSize -= 0.2f;
        }
    }
}
