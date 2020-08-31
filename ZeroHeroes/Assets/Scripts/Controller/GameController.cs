using Assets.Scripts.ai.path;
using Assets.Scripts.Gameplay;
using Assets.Scripts.Utility;
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
    public Transform ItemContainer;

    [Header("Player")]
    private Player player;
    public GameObject playerPrefab;

    public GameObject itemPrefabTemplate;


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
        GAME_STATE = GameState.PLAYING;

        pathRequestManager = new PathRequestManager(); //set up the pathfinding manager and pathfinder
        pathFinder = new PathFinder();
        world = new World();

        collisionTilemap = GameObject.Find("Grid").transform.Find("Collision").GetComponent<Tilemap>();

        SetupTestWorld();

        
        yield return new WaitForSeconds(0.3f);

    }

    private void SetupTestWorld() {
        //this is a test method.. later on we would either generate a new world.. with nothing..
        //or load from data...
        //but since were doing a techdemo tomorrow(1/09) lets just populate some data ;) ;) HACKTHEPLANET.

        world.GenerateWorld(collisionTilemap);

        Position spawnPoint = new Position(20, 20);
        player = new Player("1", spawnPoint);
        //fill the player's inventory with a few items..
        player.Entity.Inventory.Add("buckets_3", 2);

        
        player.Entity.Inventory.Add("farming_fishing_71", 1);
        player.Entity.Inventory.Add("farming_fishing_73", 1);
        player.Entity.Inventory.Add("farming_fishing_112", 1);
        player.Entity.Inventory.Add("farming_fishing_112", 1);
        player.Entity.Inventory.Add("farming_fishing_106", 2);
        player.Entity.Inventory.Add("farming_fishing_105", 1);

        //fill teh world with a few items...

        World.SpawnItem("farming_fishing_71", new Position(25, 17), 1);
        World.SpawnItem("farming_fishing_72", new Position(22, 22), 3);
        World.SpawnItem("farming_fishing_73", new Position(33, 25), 1);
        World.SpawnItem("farming_fishing_0", new Position(33, 26), 1);

        //todo this is a test to show you how items are spawned in the world...                                 
        World.SpawnItem("buckets_3", new Position(17, 17), 1);//id buckets_3 is a bucket obj for example... (located:assets/resources/tiles/buckets_3.asset
        //                                                                                                    //however these should be moved to a more appropriate location. discuss tomorrow (1/9)

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
