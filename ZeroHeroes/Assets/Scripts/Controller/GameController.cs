using Assets.Scripts.ai.path;
using Assets.Scripts.world;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.XR.WSA;

#pragma warning disable 649

public class GameController : MonoBehaviour
{
    private PathRequestManager pathRequestManager;
    private PathFinder pathFinder;
    private World world;
    #region AccessVariables

    public enum GameState { PAUSED, PLAYING };
    public static GameState GAME_STATE = GameState.PAUSED;


    [Header("Containers")]
    public Transform gameplayContainer;

    [Header("Player")]
    private NewPlayer player;
    public GameObject playerPrefab;


    #endregion
    #region PrivateVariables


    private int money = 0;

    private Texture2D screenshot;

    private Dictionary<string, string> stats = new Dictionary<string, string>();


    #endregion
    #region Initlization

    /**
     * Creates an Perament(static) instance of this class, so on scene change data isn't lost
     */
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

    /**
     * Save's all the players stat's such as Money, when they exit the application (Mobile Version)
     * -- Todo --
     * - Instead of just saving to PlayerPrefs, also save to SQLite (Database)
     */
    private void OnApplicationQuit() {
        foreach (string key in stats.Keys) {
            PlayerPrefs.SetString(key, stats[key]);
        }
    }


    public GameState CurrentGameState {
        get { return GAME_STATE; }
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

    #endregion
    #region Main

    Tilemap tm;

    private void OnDrawGizmos() {
        //this is just a test gizmos
        if(tm != null) {
            foreach (var pos in tm.cellBounds.allPositionsWithin) {
                Vector3Int localPlace = new Vector3Int(pos.x, pos.y, pos.z);
                Vector3 place = tm.CellToWorld(localPlace);
                if (tm.HasTile(localPlace)) {
                    Gizmos.DrawCube(new Vector3(localPlace.x + 1, localPlace.y + 1), new Vector3(1, 1, 1));
                }
            }
        }
    }

    public void StartGame() { StartCoroutine(_StartGame()); }
    IEnumerator _StartGame() {

        //set up the pathfinding manager and pathfinder
        pathRequestManager = new PathRequestManager();
        pathFinder = new PathFinder();

        world = new World();

        //pass the tilemap into world to generate stuff about it.. just see the code to understand.

        //todo this could be done a better way to find it AND/OR reference a collision tilemap grid NOT the background....
         tm = GameObject.Find("Grid").transform.Find("Collision").GetComponent<Tilemap>();

        world.GenerateWorld(tm);


        //todo this is placed here as a test... it would be elsewhere..
        //need to spawn the player
        //position would be passed in from last save point...
        player = new NewPlayer("1", new Position(15, 15));


        yield return new WaitForSeconds(0.3f);

        GAME_STATE = GameState.PLAYING;
    }

    public NewPlayer Player {
        get { return player; }
    }
    #endregion

    



    //todo these below need to be moved to appropriate position within the class, but dont edit them.
    public PathRequestManager PathRequestManager {
        get { return pathRequestManager; }
    }

    public PathFinder PathFinder {
        get { return pathFinder; }
    }

    public World World {
        get { return world; }
    }

    public Coroutine StartChildCoroutine(IEnumerator coroutineMethod) {
        return StartCoroutine(coroutineMethod);
    }

    public void StopChildCoroutine(Coroutine coroutineMethod) {
        StopCoroutine(coroutineMethod);
    }

}
