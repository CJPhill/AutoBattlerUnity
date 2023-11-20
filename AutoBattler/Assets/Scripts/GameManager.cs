using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    //CharacterSpace
    public GameObject selectionTable;
    public List<GameObject> selections;


    //HexGrid
    public GameObject HexMap;

    //Scripts
    public Draggable draggable;




    private void Update()
    {
        //GridShow();
    }

    private void GridShow()
    {
        if (draggable.isDragging == true)
        {
            HexMap.SetActive(true);
        }
        else
        {
            HexMap.SetActive(false);
        }


            
    }




   
}
