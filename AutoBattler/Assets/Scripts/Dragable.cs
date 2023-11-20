using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class Draggable : MonoBehaviour
{
    private Vector3 mouseOffset;
    public bool isDragging = false;
    private bool hasUnitTag;
    private bool hasTileTag;
    private Vector3 oldPosition;
    private int friendlyUnitLayer = 9;

    private void OnMouseDown()
    {
        mouseOffset = transform.position - GetMouseWorldPos();
        isDragging = true;
        oldPosition = transform.position;
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
        //TODO CURRENT ISSUES: Works for most part need to include on hextiles too but
        //Seems like circle colliders might be effecting placement on table even with ignore.
        //Unsure if code issue or unity issue.
        //NEW IDEA: CircleColliders are only needed during battle so turn off during placement to prevent conflicts.
    {
        Collider[] colliders = Physics.OverlapCapsule(transform.position - Vector3.up * 0.5f, transform.position + Vector3.up * 0.5f, 0.5f); // Adjust the height and radius as needed


        Transform closestObject = null;
        float closestDistance = float.MaxValue;
        hasTileTag = false;
        hasUnitTag = false;

        foreach (var collider in colliders)
        {
            if (collider.gameObject != gameObject)// Exclude self
            {
                //Change this to check friendly unit Note: implementation below did not fix issue.
                //if (collider.CompareTag("Unit"))
                if (collider.gameObject.layer.Equals(friendlyUnitLayer))
                {
                    hasUnitTag = true;
                }
                if (collider.CompareTag("Tile"))
                {
                    hasTileTag = true;
                }

            }
        }
        foreach (var collider in colliders) 
        { 

            if ((!(hasTileTag && hasUnitTag)) || (!hasUnitTag))
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
            
        }

        if (closestObject != null)
        {
            // Snap to the center of the closest object, only affecting X and Z coordinates
            transform.position = new Vector3(closestObject.position.x, transform.position.y, closestObject.position.z);
        }
        else
        {
            transform.position = oldPosition;
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
