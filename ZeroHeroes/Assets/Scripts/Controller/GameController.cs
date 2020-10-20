using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.SceneManagement;
using System;

#pragma warning disable 649

public class GameController : MonoBehaviour
{
    #region AccessVariables

    public enum GameState { PAUSED, PLAYING };
    public static GameState GAME_STATE = GameState.PAUSED;
    public static int WIN_CONDITION = 500;


    [Header("Containers")]
    public Transform entitiesContainer;

    [Header("Player")]
    public int currentZoneSceneID = 1;


    #endregion
    #region PrivateVariables


    private Dictionary<string, string> stats = new Dictionary<string, string>();
    private Dictionary<string, Task> tasks = new Dictionary<string, Task>();
    private Texture2D screenshot;

    private List<Building> buildings = new List<Building>();

    private Inventory inventory = new Inventory();

    private Tilemap collisionTilemap;

    private DateTime saveTime;

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

        EffectController.TweenFadeScene(1f, 0f, 5f, () => { }); // Fade in from White on start.

        for (int i = 0;i < SceneManager.sceneCount;i++) // Unload scene if opened while testing
        {
            if (SceneManager.GetSceneAt(i).buildIndex != 0) SceneManager.UnloadSceneAsync(SceneManager.GetSceneAt(i).buildIndex);
        }

        yield return new WaitForSeconds(0.3f);

    }

    private void OnApplicationQuit() {
        SaveLoadManager.saveData();
    }


    #endregion
    #region Getters & Setters


    public static bool PLAYING()
    {
        return GameController.Instance.CurrentGameState == GameController.GameState.PLAYING;
    }

    public GameState CurrentGameState
    {
        get { return GAME_STATE; }
    }

    public Dictionary<string, Task> GetTasks()
    {
        return tasks;
    }

    public void SetTasks(Dictionary<string, Task> tasks)
    {
        this.tasks = tasks;
    }

    public string GetStat(string key, string _default)
    {
        key = key.ToLower();

        return stats.ContainsKey(key) ? stats[key] : _default;
    }
    public int GetStat(string key, int _default) { return int.Parse(GetStat(key, _default.ToString())); }

    public void SetStat(string key, string stat)
    {
        key = key.ToLower();

        if (!stats.ContainsKey(key))
            stats.Add(key, stat);

        stats[key] = stat;
    }
    public void SetStat(string key, int stat) { SetStat(key, stat.ToString()); }

    public Dictionary<string, string> GetStats()
    {
        return stats;
    }

    public void SetStats(Dictionary<string, string> stats)
    {
        this.stats = stats;
    }

    public int GetMoney() {
        return GetStat("Money", 0);
    }

    public void SetMoney(int money) {
        SetStat("Money", money);
        UIController.Instance.GetHUD().ChangeMoney();
    }

    public int GetPoints()
    {
        return GetStat("Points", 0);
    }

    public void SetPoints(int points)
    {
        SetStat("Points", points);
        UIController.Instance.GetHUD().ChangePoints();
    }

    public void AddBuilding(Building building)
    {
        buildings.Add(building);
    }

    public List<Building> GetBuildings()
    {
        return buildings;
    }

    public Inventory GetInventory()
    {
        return inventory;
    }

    public void SetSaveTime(DateTime time)
    {
        saveTime = time;
    }

    public DateTime GetSaveTime()
    {
        return saveTime;
    }

    public int GetTimeSinceSave()
    {
        return ((int)(DateTime.UtcNow - saveTime).TotalSeconds);
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

        saveTime = DateTime.UtcNow; // If new save, since 0 by default would mean a long time

        yield return new WaitForSeconds(0.1f);

        SceneManager.SetActiveScene(SceneManager.GetSceneByBuildIndex(currentZoneSceneID));

        if (SaveLoadManager.loadData()) // If new Game without any save data
        {
            GenerateNewWorld();
        }

        yield return new WaitForSeconds(0.1f);

        EffectController.TweenFadeScene(1f, 0f, 0.4f, () => { }); // Fade to playspace scene

        GAME_STATE = GameState.PLAYING;

        UIController.Instance.GetHUD().gameObject.SetActive(true);

        /*
        collisionTilemap = GameObject.Find("Grid").transform.Find("Collision").GetComponent<Tilemap>();
        */

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

    public void StopGame()
    {
        UIController.Instance.CloseAllPanels(UIController.Instance.GetMainMenu());
        UIController.Instance.GetMainMenu().Open();

        SceneManager.SetActiveScene(SceneManager.GetSceneByBuildIndex(0));

        SaveLoadManager.saveData();

        for (int i = 0;i < SceneManager.sceneCount;i++)
        {
            if (SceneManager.GetSceneAt(i).buildIndex != 0) SceneManager.UnloadSceneAsync(i);
        }

        GAME_STATE = GameState.PAUSED;
        Time.timeScale = 1f;
    }

    public Building SpawnBuilding(string building_id, Vector3 position)
    {
        BuildingAttributes data = Building.FindBuildingAttributes(building_id);

        if (data == null) return null;

        GameObject obj = GameObject.Instantiate(data.GetPrefab());
        obj.transform.SetParent(GameController.Instance.entitiesContainer, false);
        obj.transform.position = position;

        Building building = obj.GetComponent<Building>();
        buildings.Add(building);

        return building;
    }

    public void LoadBuildings(List<BuildingSave> buildingsSave)
    {
        if (buildingsSave == null || buildingsSave.Count < 1) return;

        Debug.Log("Time elapsed since last save " + GetTimeSinceSave() + " seconds");

        foreach (BuildingSave buildingSave in buildingsSave)
        {
            Building building = SpawnBuilding(buildingSave.id, new Vector2(buildingSave.positionX, buildingSave.positionY));
            building.SetProducedItems(buildingSave.producedItems);
            building.SetLastProduceTime(buildingSave.lastProduceTime); // Check with the GetSaveTime() for currect
            building.SetRestocked(buildingSave.restocked);
            building.SetLastRestockTime(buildingSave.lastRestockTime);
            building.CalculateIdledProduces(GetTimeSinceSave());
        }
    }

    public void GenerateNewWorld() // World is -21, 19 to 21, -19
    {
        SpawnBuilding("tree_1", new Vector2(-2,-2));
        SpawnBuilding("tree_1", new Vector2(-10,2));
        SpawnBuilding("tree_1", new Vector2(-17,3));
        SpawnBuilding("tree_1", new Vector2(-18,9));
        SpawnBuilding("tree_1", new Vector2(-9,13));
        SpawnBuilding("tree_1", new Vector2(1,14));
        SpawnBuilding("tree_1", new Vector2(8,13));
        SpawnBuilding("tree_1", new Vector2(8,0));
        SpawnBuilding("tree_1", new Vector2(11,4));
        SpawnBuilding("tree_1", new Vector2(-18,-7));
        SpawnBuilding("tree_1", new Vector2(-18,-18));
        SpawnBuilding("tree_1", new Vector2(-13,-20));
        SpawnBuilding("tree_1", new Vector2(-12,-6));
        SpawnBuilding("tree_1", new Vector2(-8,-8));
        SpawnBuilding("tree_1", new Vector2(-4,-11));
        SpawnBuilding("tree_1", new Vector2(-6,-17));
        SpawnBuilding("tree_1", new Vector2(-1,-19));
        SpawnBuilding("tree_1", new Vector2(-1,-10));
        SpawnBuilding("tree_1", new Vector2(4,-2));
        SpawnBuilding("tree_1", new Vector2(7,-6));
        SpawnBuilding("tree_1", new Vector2(13,-6));
        SpawnBuilding("tree_1", new Vector2(17,0));
        SpawnBuilding("tree_1", new Vector2(6,-14));
        SpawnBuilding("tree_1", new Vector2(13,-17));
        SpawnBuilding("tree_1", new Vector2(15,-10));

        SpawnBuilding("rock_1", new Vector2(-11,15));
        SpawnBuilding("rock_1", new Vector2(-7,4));
        SpawnBuilding("rock_1", new Vector2(-12,-14));
        SpawnBuilding("rock_1", new Vector2(14,10));
    }

    #endregion
    #region Tasks

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
    #region Zones

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
    }

    #endregion
}
