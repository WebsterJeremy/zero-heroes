using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TestScriptBG : MonoBehaviour
{

    Tilemap tilemp;
    void Start() {
        tilemp = GameObject.Find("Foreground").GetComponent<Tilemap>();
    }
    public TileBase[] currentTile;

    public int selectNum = 0;

    public TileBase sand_1;
    public TileBase rock_1;
    public TileBase reed_21;

    public TileBase collision;

    public bool turnedOn = true;

    public string itemID;

    void Update() {


        if(turnedOn == true)
        {
            if (Input.GetKeyDown(KeyCode.C))
            {
                selectNum++;
                if(selectNum >= 3)
                {
                    selectNum = 0;
                }
            }
            if (Input.GetKeyDown(KeyCode.V))
            {
                selectNum--;
                if(selectNum <= 0)
                {
                    selectNum = 2;
                }
            }
                /*if (Input.GetKeyDown(KeyCode.Alpha2))
                {
                    selectNum = 1;
                }
                if (Input.GetKeyDown(KeyCode.Alpha1))
                {
                    selectNum = 0;
                }
                if (Input.GetKeyDown(KeyCode.Alpha3))
                {
                    selectNum = 2;
                }*/
        

            Vector3 point = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            point.z = Camera.main.nearClipPlane;
            if (Input.GetMouseButtonDown(0)) {
                if(currentTile[selectNum] == sand_1)
                {
                    Debug.Log("move to foreground");
                    tilemp = GameObject.Find("Foreground").GetComponent<Tilemap>();
                    Vector3Int selectedTile = tilemp.WorldToCell(point);
                    tilemp.SetTile(selectedTile, currentTile[selectNum]);
                }
                else if (currentTile[selectNum] == rock_1)
                {
                    Debug.Log("move to foreground 2");
                    tilemp = GameObject.Find("Foreground 2").GetComponent<Tilemap>();
                    Vector3Int selectedTile = tilemp.WorldToCell(point);
                    tilemp.SetTile(selectedTile, currentTile[selectNum]);
                    tilemp = GameObject.Find("Collision").GetComponent<Tilemap>();
                    tilemp.SetTile(selectedTile, collision);
                }
                else if (currentTile[selectNum] == reed_21)
                {
                    Debug.Log("move to foreground 2");
                    tilemp = GameObject.Find("Foreground 2").GetComponent<Tilemap>();
                    Vector3Int selectedTile = tilemp.WorldToCell(point);
                    tilemp.SetTile(selectedTile, currentTile[selectNum]);
                }


                //Camera.main.transform.position = (point);
                //Vector3Int selectedTile = tilemp.WorldToCell(point);
                //tilemp.SetTile(selectedTile, currentTile[selectNum]);
            }
        }
        
    }


    void SelectedItem()
    {
        //itemID = GameObject.Find("TileBase").GetComponent<ID>();

        if(itemID == "")
        {
            //selectNum = ;
        }
        if (itemID == "")
        {
            //selectNum = ;
        }
        if (itemID == "")
        {
            //selectNum = ;
        }
        if (itemID == "")
        {
            //selectNum = ;
        }
        if (itemID == "")
        {
            //selectNum = ;
        }
        if (itemID == "")
        {
            //selectNum = ;
        }
    }

    
}

/*
switch(){
   
    }
    */