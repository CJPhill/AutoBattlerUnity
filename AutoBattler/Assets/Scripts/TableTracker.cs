using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class TableTracker : MonoBehaviour
{
    public GameObject selectionTable;
    public List<GameObject> selections;

    public void Start()
    {
        selectionTable = GetComponent<GameObject>();
        grabTheChildren();
    }

    private void grabTheChildren()
    {
        GameObject child0 = selectionTable.transform.GetChild(0).gameObject;
        GameObject child1 = selectionTable.transform.GetChild(1).gameObject;
        GameObject child2 = selectionTable.transform.GetChild(2).gameObject;
        GameObject child3 = selectionTable.transform.GetChild(3).gameObject;
        GameObject child4 = selectionTable.transform.GetChild(4).gameObject;
        GameObject child5 = selectionTable.transform.GetChild(5).gameObject;
        GameObject child6 = selectionTable.transform.GetChild(6).gameObject;

        selections.Add(child0);
        selections.Add(child1);
        selections.Add(child2);
        selections.Add(child3);
        selections.Add(child4);
        selections.Add(child5);
        selections.Add(child6);

    }

    private void OnCollisionStay(Collision collision)
    {
        
    }


}
