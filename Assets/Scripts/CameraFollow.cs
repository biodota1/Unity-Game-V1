using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target; // The object for the camera to follow

    void LateUpdate()
    {
        if (target != null)
        {
            // Set the camera's position to the target's position, but keep the same z value
            transform.position = new Vector3(target.position.x, target.position.y, target.position.z);
        }
    }
}
