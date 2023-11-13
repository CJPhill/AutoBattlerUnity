using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Draggable : MonoBehaviour
{
    private Vector3 mouseOffset;
    private bool isDragging = false;

    private void OnMouseDown()
    {
        mouseOffset = transform.position - GetMouseWorldPos();
        isDragging = true;
    }

    private void OnMouseDrag()
    {
        if (isDragging)
        {
            Vector3 currentMousePos = GetMouseWorldPos();
            transform.position = new Vector3(currentMousePos.x + mouseOffset.x, transform.position.y, currentMousePos.z + mouseOffset.z);
        }
    }

    private void OnMouseUp()
    {
        isDragging = false;

        // Check for collision with other objects and snap if needed
        SnapToClosestObject();
    }

    private void SnapToClosestObject()
    {
        Collider[] colliders = Physics.OverlapCapsule(transform.position - Vector3.up * 0.5f, transform.position + Vector3.up * 0.5f, 0.5f); // Adjust the height and radius as needed

        Transform closestObject = null;
        float closestDistance = float.MaxValue;

        foreach (var collider in colliders)
        {
            if (collider.gameObject != gameObject) // Exclude self
            {
                float distance = Vector3.Distance(transform.position, collider.transform.position);

                if (distance < closestDistance)
                {
                    closestObject = collider.transform;
                    closestDistance = distance;
                }
            }
        }

        if (closestObject != null)
        {
            // Snap to the center of the closest object, only affecting X and Z coordinates
            transform.position = new Vector3(closestObject.position.x, transform.position.y, closestObject.position.z);
        }
    }

    private Vector3 GetMouseWorldPos()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit))
        {
            return hit.point;
        }

        return Vector3.zero; // Return a default value if no hit point is found.
    }
}
