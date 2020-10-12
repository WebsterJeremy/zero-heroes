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
    public Transform entitiesContainer;
    public Transform buildingsContainer;

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

    private List<Entity> entities = new List<Entity>();
    private List<Building> buildings = new List<Building>();

    private Inventory inventory = new Inventory();

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

        inventory.Load();

        money = GetStat("MONEY", 0);

        EffectController.TweenFadeScene(1f, 0f, 5f, () => { }); // Fade in from White on start.

        for (int i = 0;i < SceneManager.sceneCount;i++) // Unload scene if opened while testing
        {
            if (SceneManager.GetSceneAt(i).buildIndex != 0) SceneManager.UnloadSceneAsync(SceneManager.GetSceneAt(i).buildIndex);
        }

        yield return new WaitForSeconds(0.3f);

//        UIController.Instance.GetHUD().DisplayMoney(money);
    }

    private void OnApplicationQuit() {
        SaveGame();
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

    public Dictionary<string,Task> GetTasks()
    {
        return tasks;
    }

    public void AddEntity(Entity entity)
    {
        entities.Add(entity);
    }

    public void AddBuilding(Building building)
    {
        buildings.Add(building);
    }

    public Inventory GetInventory()
    {
        return inventory;
    }

    #endregion
    #region Main


    public void StartGame() { StartCoroutine(_StartGame()); }
    IEnumerator _StartGame() {
        SceneManager.LoadScene(currentZoneSceneID, LoadSceneMode.Additive);

        EffectController.TweenFadeScene(0f, 1f, 0.4f, () => { }); // Fade to loading screen

        while (!SceneManager.GetSceneByBuildIndex(currentZoneSceneID).isLoaded) // Force wait until it's loaded (Could add Loading screen if required)
        {
            yield return new WaitForSeconds(0.1f);
        }

        EffectController.TweenFadeScene(1f, 0f, 0.4f, () => { }); // Fade to playspace scene

        SceneManager.SetActiveScene(SceneManager.GetSceneByBuildIndex(currentZoneSceneID));

        GAME_STATE = GameState.PLAYING;

        UIController.Instance.GetHUD().gameObject.SetActive(true);

        /*
        collisionTilemap = GameObject.Find("Grid").transform.Find("Collision").GetComponent<Tilemap>();
        */

        GenerateTestWorldData();
        

        yield return new WaitForSeconds(0.3f);

    }


    public void PauseGame() { StartCoroutine(_PauseGame()); }
    IEnumerator _PauseGame()
    {
        yield return null;

        GAME_STATE = GameState.PAUSED;

        Time.timeScale = 0f;
    }

    public void UnpauseGame() { StartCoroutine(_UnpauseGame()); }
    IEnumerator _UnpauseGame()
    {
        yield return null;

        GAME_STATE = GameState.PLAYING;

        Time.timeScale = 1f;
    }

    public void StopGame() // Open Mainmenu / Unload Zones / Save Progress
    {
        UIController.Instance.CloseAllPanels(UIController.Instance.GetMainMenu());
        UIController.Instance.GetMainMenu().Open();

        SceneManager.SetActiveScene(SceneManager.GetSceneByBuildIndex(0));

        SaveGame();

        for (int i = 0;i < SceneManager.sceneCount;i++)
        {
            if (SceneManager.GetSceneAt(i).buildIndex != 0) SceneManager.UnloadSceneAsync(i);
        }

        GAME_STATE = GameState.PAUSED;
        Time.timeScale = 1f;
    }

    
    private void GenerateTestWorldData() {

        player = new Player();
        player.Spawn(GameObject.Find("SpawnPoint").transform.position);
        
        /*
        //generate world..
        world.GenerateWorld(collisionTilemap);

        //spawn player
        Position spawnPoint = new Position(GameObject.Find("SpawnPoint").transform.position); // new Position(15, 15);
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
        */
    }


    public void EnterZone(Zone zone, string entryPoint) { StartCoroutine(_EnterZone(zone, entryPoint)); }
    public void EnterZone(Zone zone) { StartCoroutine(_EnterZone(zone, "SpawnPoint")); }
    public IEnumerator _EnterZone(Zone zone, string entryPoint)
    {
        SceneManager.LoadScene(zone.sceneId, LoadSceneMode.Additive);

        EffectController.TweenFadeScene(0f, 1f, 0.4f, () => { });

        while (!SceneManager.GetSceneByBuildIndex(zone.sceneId).isLoaded)
        {
            yield return new WaitForSeconds(0.1f);
        }

        EffectController.TweenFadeScene(1f, 0f, 0.4f, () => { });
        
        SceneManager.SetActiveScene(SceneManager.GetSceneByBuildIndex(zone.sceneId));

        SceneManager.UnloadSceneAsync(currentZoneSceneID); // Unload Old Scene/Zone

        currentZoneSceneID = zone.sceneId;

        yield return new WaitForSeconds(0.1f);

        collisionTilemap = GameObject.Find("Grid").transform.Find("Collision").GetComponent<Tilemap>();

//        world.GenerateWorld(collisionTilemap);

        yield return new WaitForSeconds(0.3f);

        /*
        Position entryPos = new Position(GameObject.Find(entryPoint).transform.position);
        player.Entity.UpdatePosition(entryPos, true);
        player.Entity.MovementHelper.StopFollowingCurrentPath();

        yield return new WaitForSeconds(0.1f);

        player.Entity.FSM.EnterState(new FSMStateIdle(player.Entity));
        */

        //        GenerateTestWorldData();

        // After 5 minutes of old zone not being re-entered remove the scene (instead of keeping it unloaded)
    }

    public void SaveGame()
    {
        Debug.Log("Saving the game");

        foreach (string key in stats.Keys)
        {
            PlayerPrefs.SetString(key, stats[key]);
        }
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

    /*
    private void OnDrawGizmos() // Draw Debug Colliders in scene view
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
    */


    #endregion
}
