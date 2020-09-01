using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TestScript : MonoBehaviour
{

    Tilemap tilemp; void Start() { tilemp = GameObject.Find("Foreground").GetComponent<Tilemap>(); }
    public TileBase[] currentTile;

    public int selectNum = 0;

    void Update() {

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
            //Camera.main.transform.position = (point);
            Vector3Int selectedTile = tilemp.WorldToCell(point);
            tilemp.SetTile(selectedTile, currentTile[selectNum]);
        }
    }

    
}
