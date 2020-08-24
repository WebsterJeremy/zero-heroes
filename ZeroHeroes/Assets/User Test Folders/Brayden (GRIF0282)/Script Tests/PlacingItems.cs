using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;


public class PlacingItems : MonoBehaviour
{
    public Tilemap gridSystem;
    public TileBase currentTile;
    
    

    public int x;
    public int y;
    public int z;
    // Start is called before the first frame update
    void Start()
    {
        //gridSystem = Tilemap.FindObjectOfType <Background >;
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 mousePos = (Input.mousePosition);
        Vector3 worldPos = Camera.main.ScreenToWorldPoint(mousePos);
        worldPos.z = Camera.main.nearClipPlane;
        
        Vector3Int mousePosInt = new Vector3Int(x = (int)worldPos.x, y = (int)worldPos.y, z = (int)worldPos.z);
        

        if (Input.GetMouseButtonDown(0))
        {
            Debug.Log("Mouse Click");
            gridSystem.SetTile(mousePosInt, currentTile);
            //currentTile = gridSystem.GetTile(mousePosInt);

        }

        
    }
}
