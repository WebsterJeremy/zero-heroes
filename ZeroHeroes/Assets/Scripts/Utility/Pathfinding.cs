using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Tilemaps;

public class Pathfinding : MonoBehaviour
{
    #region AccessVariables


    [Header("Setup")]
    [SerializeField] private GameObject tileHoverSprite;


    #endregion
    #region PrivateVariables


    private Vector2 hoveredPos = new Vector2(0, 0);
    private TileBase[] hoveredTiles;

    private Tilemap[] tileMaps;


    #endregion
    #region Initlization


    private static Pathfinding instance;
    public static Pathfinding Instance // Assign Singlton
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<Pathfinding>();
                if (Instance == null)
                {
                    var instanceContainer = new GameObject("Pathfinding");
                    instance = instanceContainer.AddComponent<Pathfinding>();
                }
            }
            return instance;
        }
    }

    void Start()
    {
        tileMaps = FindObjectsOfType<Tilemap>();
        hoveredTiles = new TileBase[tileMaps.Length];
    }


    #endregion
    #region Getters & Setters


    #endregion
    #region Main


    void Update()
    {
        TrackMouse();
    }

    private void TrackMouse()
    {
        if (tileMaps == null || tileMaps.Length == 0) return;
        if (EventSystem.current.IsPointerOverGameObject()) return;

        
        Vector3 mousePos = Input.mousePosition;
        mousePos.z = -Camera.main.transform.position.z;
        Vector3 pos = Camera.main.ScreenToWorldPoint(mousePos);
        pos.x += 1.0f;
        pos.y += 1.0f;

        Vector3Int tilePos = tileMaps[0].WorldToCell(pos);

        hoveredPos.x = tilePos.x;
        hoveredPos.y = tilePos.y;

        for (int i = 0;i < tileMaps.Length;i++)
        {
            hoveredTiles[i] = tileMaps[i].GetTile(tileMaps[i].WorldToCell(tilePos));
        }

        tileHoverSprite.transform.position = tilePos;

        ShowDebugInfo();
    }

    private void ShowDebugInfo()
    {
        UIController.SetDebugStatistic("Mouse Pos XYZ", Input.mousePosition);

        string tileNames = "";
        for (int i = 0; i < hoveredTiles.Length; i++)
        {
            tileNames += "\t" + tileMaps[i].name + ": " + (hoveredTiles[i] != null ? hoveredTiles[i].name : "null") + "\n";
        }

        UIController.SetDebugStatistic("Hovered Tile XY", hoveredPos + "\n" + tileNames);
    }

    #endregion
}
