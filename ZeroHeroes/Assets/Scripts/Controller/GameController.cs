using Assets.Scripts.ai.path;
using Assets.Scripts.world;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

#pragma warning disable 649

public class GameController : MonoBehaviour
{
    #region AccessVariables

    public enum GameState { PAUSED, PLAYING };
    public static GameState GAME_STATE = GameState.PAUSED;


    [Header("Containers")]
    public Transform gameplayContainer;

    [Header("Player")]
    private Player player;
    public GameObject playerPrefab;


    #endregion
    #region PrivateVariables


    private int money = 0;
    private Dictionary<string, string> stats = new Dictionary<string, string>();
    private Texture2D screenshot;

    private PathRequestManager pathRequestManager;
    private PathFinder pathFinder;
    private World world;
    private Tilemap collisionTilemap;


    #endregion
    #region Initlization

    private static GameController instance;
    public static GameController Instance // Assign Singlton
    {
        get {
            if (instance == null) {
                instance = FindObjectOfType<GameController>();
                if (Instance == null) {
                    var instanceContainer = new GameObject("GameController");
                    instance = instanceContainer.AddComponent<GameController>();
                }
            }
            return instance;
        }
    }


    IEnumerator Start() {
        Input.simulateMouseWithTouches = true;

        yield return new WaitForFixedUpdate();

        money = GetStat("MONEY", 0);

        EffectController.TweenFadeScene(1f, 0f, 2f, () => { }); // Fade in from White on start.

        yield return new WaitForSeconds(0.3f);

        UIController.Instance.GetHUD().DisplayMoney(money);
        Debug.Log(money);

        //todo starting game here for a moment.. remove once menu UI is working
        StartGame();
    }

    private void OnApplicationQuit() { // Force save in SQL
        foreach (string key in stats.Keys) {
            PlayerPrefs.SetString(key, stats[key]);
        }
    }


    #endregion
    #region Getters & Setters

    public int GetMoney() {
        return money;
    }

    public void SetMoney(int amount) {
        UIController.Instance.GetHUD().ChangeMoney(this.money, amount, 5f); // Update's the money UI element
        this.money = amount;
        SetStat("MONEY", this.money.ToString());
    }

    public string GetStat(string key, string _default) {
        if (!stats.ContainsKey(key))
            SetStat(key, PlayerPrefs.GetString(key, _default));

        return stats.ContainsKey(key) ? stats[key] : _default;
    }
    public int GetStat(string key, int _default) { return int.Parse(GetStat(key, _default.ToString())); }

    public void SetStat(string key, string stat) {
        if (!stats.ContainsKey(key))
            stats.Add(key, stat);

        stats[key] = stat;
    }

    public GameState CurrentGameState
    {
        get { return GAME_STATE; }
    }


    public Player Player
    {
        get { return player; }
    }

    public PathRequestManager PathRequestManager
    {
        get { return pathRequestManager; }
    }

    public PathFinder PathFinder
    {
        get { return pathFinder; }
    }

    public World World
    {
        get { return world; }
    }

    #endregion
    #region Main

    private void OnDrawGizmos() //this is just a test gizmos
    { 

        if (collisionTilemap != null) {
            foreach (var pos in collisionTilemap.cellBounds.allPositionsWithin) {
                Vector3Int localPlace = new Vector3Int(pos.x, pos.y, pos.z);
                Vector3 place = collisionTilemap.CellToWorld(localPlace);

                if (collisionTilemap.HasTile(localPlace)) {
                    Gizmos.DrawCube(new Vector3(localPlace.x + 1, localPlace.y + 1), new Vector3(1, 1, 1));
                }
            }
        }
    }

    public void StartGame() { StartCoroutine(_StartGame()); }
    IEnumerator _StartGame() {

        pathRequestManager = new PathRequestManager(); //set up the pathfinding manager and pathfinder
        pathFinder = new PathFinder();
        world = new World();

        collisionTilemap = GameObject.Find("Grid").transform.Find("Collision").GetComponent<Tilemap>();

        world.GenerateWorld(collisionTilemap);

        Position spawnPoint = new Position(15, 15);
        player = new Player("1", spawnPoint);

        yield return new WaitForSeconds(0.3f);

        GAME_STATE = GameState.PLAYING;
    }

    #endregion
    #region Utility


    public Coroutine StartChildCoroutine(IEnumerator coroutineMethod)
    {
        return StartCoroutine(coroutineMethod);
    }

    public void StopChildCoroutine(Coroutine coroutineMethod)
    {
        StopCoroutine(coroutineMethod);
    }


    #endregion

    #region Testing
    /*
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
     */
    #endregion
}
