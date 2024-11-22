using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class InteractableObject : CollidableObject
{
    private bool isOpened = false;
    protected override void OnCollided(GameObject collidedObject)
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            OnInteract();
        }
    }



    protected virtual void OnInteract()
    {
        if (isOpened==false)
        {
            isOpened = true;
            Debug.Log("INTERACT WITH " + name);
            GameObject _target = transform.parent.gameObject;
            _target.transform.rotation = Quaternion.Euler(0, 0, 90);

        }
        else
        {
            isOpened = false;
            Debug.Log("INTERACT WITH " + name);
            GameObject _target = transform.parent.gameObject;
            _target.transform.rotation = Quaternion.Euler(0, 0, 0);

        }
    }
}