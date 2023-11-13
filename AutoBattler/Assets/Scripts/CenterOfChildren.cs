using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CenterOfChildren : MonoBehaviour
{
    public GameObject FocusObject;

    void LateUpdate()
    {
        // Find the center of the children
        Vector3 center = CalculateCenterOfChildren();

        // Log the result (you can use it for whatever you need)
       //Debug.Log("Center of Children: " + center);
    }

    Vector3 CalculateCenterOfChildren()
    {
        // Get all the child transforms
        Transform[] children = GetComponentsInChildren<Transform>();

        if (children.Length == 1)
        {
            // If there's only one child, return its position
            return children[0].position;
        }

        // Initialize the center to (0, 0, 0)
        Vector3 center = Vector3.zero;

        // Sum up the positions of all children
        foreach (Transform child in children)
        {
            center += child.position;
        }

        // Calculate the average position (center) by dividing by the number of children
        center /= children.Length;

        FocusObject.transform.position = center;

        return center;
    }
}
