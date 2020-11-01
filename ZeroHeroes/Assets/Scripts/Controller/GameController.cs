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
    public static bool NEW_GAME = false;


    [Header("Containers")]
    public Transform entitiesContainer;

    [Header("Player")]
    public int currentZoneSceneID = 1;
    public NpcAttributes[] npcs;
    public ChapterAttributes[] chapterList;


    #endregion
    #region PrivateVariables


    private Dictionary<string, string> stats = new Dictionary<string, string>();
    private List<Chapter> chapters = new List<Chapter>();
    private Texture2D screenshot;

    private List<Building> buildings = new List<Building>();

    private Inventory inventory = new Inventory();

    private Tilemap collisionTilemap;

    private DateTime saveTime;
    private float gameTime;

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

        EffectController.TweenFadeScene(1f, 0f, 10f, () => { }); // Fade in from White on start.

        for (int i = 0;i < SceneManager.sceneCount;i++) // Unload scene if opened while testing
        {
            if (SceneManager.GetSceneAt(i).buildIndex != 0) SceneManager.UnloadSceneAsync(SceneManager.GetSceneAt(i).buildIndex);
        }

        yield return new WaitForSeconds(0.3f);

    }

    private void OnApplicationPause(bool pause)
    {
        if (pause)
        {
            SaveGame();
        }
        else
        {
            if (PLAYING()) SaveLoadManager.loadData();
        }
    }

    private void OnApplicationQuit() {
        
    }

    public void SaveGame() { StartCoroutine(_SaveGame()); }
    private IEnumerator _SaveGame()
    {
        SaveLoadManager.saveData();

        yield return new WaitForFixedUpdate();

        for (int i = 0;i < entitiesContainer.childCount;i++)
        {
            if (entitiesContainer.GetChild(i) != null && entitiesContainer.GetChild(i).gameObject != null)
            {
                Destroy(entitiesContainer.GetChild(i).gameObject);
            }
        }
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

    public string GetStat(string key, string _default)
    {
        key = key.ToLower();

        return stats.ContainsKey(key) ? stats[key] : _default;
    }
    public int GetStat(string key, int _default) {
        int number = _default;

        bool success = Int32.TryParse(GetStat(key, _default.ToString()), out number);

        return number;
    }

    public void SetStat(string key, string stat)
    {
        key = key.ToLower();

        if (!stats.ContainsKey(key))
            stats.Add(key, stat);

        stats[key] = stat;

        CheckObjectivies(TaskAttributes.ObjectiveType.STAT);
    }
    public void SetStat(string key, int stat) { SetStat(key, stat.ToString()); }
    public void SetStat(string key, float stat) { SetStat(key, ((int) stat).ToString()); }

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
        UIController.Instance.GetBuildMenu().UpdateCardPrices();
    }
    public void GiveMoney(int money) { SetMoney(GetMoney() + money); }

    public int GetPoints()
    {
        return GetStat("Points", 0);
    }

    public void SetPoints(int points)
    {
        SetStat("Points", points);
        UIController.Instance.GetHUD().ChangePoints();
        UIController.Instance.GetBuildMenu().UpdateCardPrices();
    }
    public void GivePoints(int points) { SetPoints(GetPoints() + points); }

    public int GetChapterStat()
    {
        return GetStat("Chapter", 1);
    }

    public void SetChapterStat(int chapter)
    {
        SetStat("Chapter", chapter);
    }

    public Chapter GetChapter(int key)
    {
        return GetChapters() != null ? GetChapters()[key] : null;
    }

    public Chapter GetCurrentChapter()
    {
        return GetChapter(GetChapterStat() - 1);
    }

    public List<Chapter> GetChapters()
    {
        if (chapters == null || chapters.Count != 5)
        {
            chapters = new List<Chapter>();

            foreach (ChapterAttributes attributes in chapterList)
            {
                Chapter chapter = new Chapter();
                chapter.Setup(attributes);

                chapters.Add(chapter);
            }
        }

        return chapters;
    }

    public void SetChapters(List<Chapter> chapters)
    {
        this.chapters = chapters;
    }

    public void AddBuilding(Building building)
    {
        buildings.Add(building);
    }

    public void RemoveBuilding(Building building)
    {
        if (!buildings.Contains(building)) return;

        buildings.Remove(building);
        Destroy(building.gameObject);
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

    public void SetGameTime(float time)
    {
        gameTime = time;
    }

    public float GetGameTime()
    {
        return gameTime;
    }

    public int GetTimeSinceSave()
    {
        return ((int)(DateTime.UtcNow - saveTime).TotalSeconds);
    }

    public NpcAttributes GetMayor()
    {
        return npcs[0];
    }


    #endregion
    #region Main

    public void StartGame() { StartCoroutine(_StartGame()); }
    IEnumerator _StartGame() {

        EffectController.TweenFadeScene(0f, 1f, 0.1f, () => {
            EffectController.Instance.fadeImage.color = new Color(1, 1, 1, 1);
        }); // Fade to loading screen

        yield return new WaitForSeconds(0.1f);

        EffectController.Instance.fadeImage.color = new Color(1, 1, 1, 1);

        SceneManager.LoadScene(currentZoneSceneID, LoadSceneMode.Additive);

        while (!SceneManager.GetSceneByBuildIndex(currentZoneSceneID).isLoaded) // Force wait until it's loaded (Could add Loading screen if required)
        {
            yield return new WaitForSeconds(0.1f);
        }

        saveTime = DateTime.UtcNow; // If new save, since 0 by default would mean a long time

        yield return new WaitForSeconds(0.1f);

        SceneManager.SetActiveScene(SceneManager.GetSceneByBuildIndex(currentZoneSceneID));

        if (SaveLoadManager.loadData()) // If new Game without any save data
        {
            Debug.LogWarning("New World Loaded!");
        }

        yield return new WaitForSeconds(0.2f);

        EffectController.TweenFadeScene(1f, 0f, 1f, () => { }); // Fade to playspace scene

        GAME_STATE = GameState.PLAYING;

        UIController.Instance.GetHUD().gameObject.SetActive(true);
        UIController.Instance.GetHUD().DisplayMoney(GetMoney());
        UIController.Instance.GetHUD().DisplayPoints(GetPoints());

        GetChapters();

        if (NEW_GAME)
        {
            yield return new WaitForSeconds(0.5f);

            UIController.Instance.GetDialog().StartReading("" +
                "Welcome to Zero Heros!\nOur goal here in this town is to achieve a zero emissions lifestyle in our everyday life's." +
                "Thanks to people like you who have volunteered  for this lifestyle were getting one step closer." +
                "This plot of land here is yours for free along as you achieve a zero emissions lifestyle within the first year." +
                "To help you get started we've given you $500 dollars, and 15 eco-points, to earn more complete tasks and sell your produced items." +
                "Now I better get going, but I've filled your task log with the first chapter's tasks." +
                "When you haved completed all the tasks for a given chapter the next one is opened, with a total of five chapters to complete before reaching a zero emissions lifestyle." +
                "Once all the chapters have been completed, you will have completed this challenge and have achieved a zero emissions lifestyle." +
                "Well was great meeting you, and I hope you enjoy yourself in our town!", GetMayor());
        }
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
        UIController.Instance.GetHUD().RemoveAllNotifys();
        UIController.Instance.GetMainMenu().Open();

        SceneManager.SetActiveScene(SceneManager.GetSceneByBuildIndex(0));

        SaveGame();

        NEW_GAME = false;

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

        foreach (BuildingSave buildingSave in buildingsSave)
        {
            Building building = SpawnBuilding(buildingSave.id, new Vector2(buildingSave.positionX, buildingSave.positionY));
            building.GetLoad(buildingSave);
        }
    }

    public Building GetBuilding(string building_id)
    {
        foreach (Building building in buildings)
        {
            if (building.GetID() == building_id) return building;
        }

        return null;
    }

    #endregion
    #region Tasks


    public void CheckObjectivies(TaskAttributes.ObjectiveType objectiveType)
    {
        if (!PLAYING()) return;

        GetCurrentChapter().CheckObjectivies(objectiveType);
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
