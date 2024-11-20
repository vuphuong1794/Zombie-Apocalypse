using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class InteractableObject : CollidableObject
{
    private bool z_Interacted = false;

    protected override void OnCollided(GameObject collidedObject)
    {
        if (Input.GetKey(KeyCode.E))
        {
            OnInteract();
        }
    }

    protected virtual void OnInteract()
    {
        if (!z_Interacted)
        {
            z_Interacted = true;
            Debug.Log("INTERACT WITH " + name);
            GameObject _target = transform.parent.gameObject;
            _target.transform.rotation = Quaternion.Euler(0, 0, 90);
        }
    }
}