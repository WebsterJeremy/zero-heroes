using Assets.Scripts.ai.path;
using Assets.Scripts.world;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.SceneManagement;

#pragma warning disable 649

public class GameController : MonoBehaviour
{
    #region AccessVariables

    public enum GameState { PAUSED, PLAYING };
    public static GameState GAME_STATE = GameState.PAUSED;


    [Header("Containers")]
    public Transform gameplayContainer;
    public Transform ObjectContainer;
    public Transform ItemContainer;

    public GameObject objectPrefabTemplate;
    public GameObject itemPrefabTemplate;

    [Header("Player")]
    private Player player;
    public GameObject playerPrefab;
    public int currentZoneSceneID = 1;


    #endregion
    #region PrivateVariables


    private int money = 0;
    private Dictionary<string, string> stats = new Dictionary<string, string>();
    private Dictionary<string, Task> tasks = new Dictionary<string, Task>();
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

        EffectController.TweenFadeScene(1f, 0f, 5f, () => { }); // Fade in from White on start.

        yield return new WaitForSeconds(0.3f);

//        UIController.Instance.GetHUD().DisplayMoney(money);
    }

    private void OnApplicationQuit() { // Force save in SQL
        foreach (string key in stats.Keys) {
            PlayerPrefs.SetString(key, stats[key]);
        }
    }


    #endregion
    #region Getters & Setters

    public static bool PLAYING()
    {
        return GameController.Instance.CurrentGameState == GameController.GameState.PLAYING;
    }

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

    public Dictionary<string,Task> GetTasks()
    {
        return tasks;
    }

    #endregion
    #region Main


    public void StartGame() { StartCoroutine(_StartGame()); }
    IEnumerator _StartGame() {
        SceneManager.LoadScene(currentZoneSceneID, LoadSceneMode.Additive);

        EffectController.TweenFadeScene(0f, 1f, 0.4f, () => { }); // Fade to loading screen

        while (!SceneManager.GetSceneAt(currentZoneSceneID).isLoaded) // Force wait until it's loaded (Could add Loading screen if required)
        {
            yield return new WaitForSeconds(0.1f);
        }

        EffectController.TweenFadeScene(1f, 0f, 0.4f, () => { }); // Fade to playspace scene

        SceneManager.SetActiveScene(SceneManager.GetSceneAt(currentZoneSceneID));

        GAME_STATE = GameState.PLAYING;

        UIController.Instance.GetHUD().gameObject.SetActive(true);

        pathRequestManager = new PathRequestManager(); //set up the pathfinding manager and pathfinder
        pathFinder = new PathFinder();
        world = new World();

        collisionTilemap = GameObject.Find("Grid").transform.Find("Collision").GetComponent<Tilemap>();

        GenerateTestWorldData();

        yield return new WaitForSeconds(0.3f);

    }


    private void GenerateTestWorldData() {
        //generate world..
        world.GenerateWorld(collisionTilemap);

        //spawn player
        Position spawnPoint = new Position(15, 15);
        player = new Player("1", spawnPoint);


        //add items to inventory...
        player.Entity.Inventory.Add("buckets_3", 1);
        player.Entity.Inventory.Add("buckets_3", 1);
        player.Entity.Inventory.Add("farming_fishing_74", 1);
        player.Entity.Inventory.Add("farming_fishing_75", 3);
        player.Entity.Inventory.Add("farming_fishing_76", 4);

        //spawn items in world....
        World.SpawnItem("farming_fishing_71", new Position(18, 18), 3);
        World.SpawnItem("farming_fishing_72", new Position(17, 15), 1);
        World.SpawnItem("farming_fishing_73", new Position(20, 20), 1);
        World.SpawnItem("farming_fishing_60", new Position(14, 20), 1);
    }

    #endregion
    #region Tasks

    /*
    Task[] tasks =
    {
        new Task(0,"npc_mayor",null,null,@"The litter on the land is <b>harmful</b> to local wildlife. 
It also increases water and soil pollution and destroys animal habitats.
Will you help clean it up?",null,null),
        new Task(1,"npc_birdwatcher_0",null,null,@"Native birds have varying habitat needs. 
Will you plant some tall and medium trees, and some shrubs to help the conservation of native bird life?",null,null),
        new Task(2,"npc_gardner_0",null,null,@"Rainwater tanks can store water for many uses and reduces the need for infrastructure such as dams and desalination plants. 
Could you install one for me?",null,null)
    };*/

    public void ReceiveTask(Task task)
    {
        if (HasTask(task.taskName)) return;

        tasks.Add(task.taskName, task);
        task.OnBeginTask();
    }

    public bool HasTask(string id)
    {
        return tasks.ContainsKey(id);
    }

    public void CompleteTask()
    {

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

    private void OnDrawGizmos() //this is just a test gizmos
    {

        if (collisionTilemap != null)
        {
            foreach (var pos in collisionTilemap.cellBounds.allPositionsWithin)
            {
                Vector3Int localPlace = new Vector3Int(pos.x, pos.y, pos.z);
                Vector3 place = collisionTilemap.CellToWorld(localPlace);

                if (collisionTilemap.HasTile(localPlace))
                {
                    Gizmos.DrawCube(new Vector3(localPlace.x + 1, localPlace.y + 1), new Vector3(1, 1, 1));
                }
            }
        }
    }


    #endregion
}
