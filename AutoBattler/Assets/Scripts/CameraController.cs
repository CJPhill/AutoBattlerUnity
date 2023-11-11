using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Transform target; // The target object to focus on

    void Start()
    {
        if (target != null)
        {
            // Adjust the camera's position and rotation to focus on the target
            transform.LookAt(target);
        }
        else
        {
            Debug.LogWarning("Camera target is not set!");
        }
    }
}

