using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Tilemaps;

#pragma warning disable 649

public class GameController : MonoBehaviour
{
    #region AccessVariables

    public enum GameState { PAUSED, PLAYING };
    public static GameState GAME_STATE = GameState.PAUSED;

    [Header("Player")]
    [SerializeField] private GameObject player;

    [Header("Setup")]
    [SerializeField] private GameObject tileHoverSprite;

    #endregion
    #region PrivateVariables


    private int money = 0;

    private Texture2D screenshot;

    private Dictionary<string, string> stats = new Dictionary<string, string>();
    private Tilemap[] tileMaps;


    #endregion
    #region Initlization

    /**
     * Creates an Perament(static) instance of this class, so on scene change data isn't lost
     */
    private static GameController instance;
    public static GameController Instance // Assign Singlton
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<GameController>();
                if (Instance == null)
                {
                    var instanceContainer = new GameObject("GameController");
                    instance = instanceContainer.AddComponent<GameController>();
                }
            }
            return instance;
        }
    }

    IEnumerator Start()
    {
        Input.simulateMouseWithTouches = true;

        yield return new WaitForFixedUpdate();

        money = GetStat("MONEY", 0);

        tileMaps = FindObjectsOfType<Tilemap>();

        EffectController.TweenFadeScene(1f, 0f, 2f, () => {}); // Fade in from White on start.

        yield return new WaitForSeconds(0.3f);

//        SpawnPlayer();
    }

    /**
     * Save's all the players stat's such as Money, when they exit the application (Mobile Version)
     * -- Todo --
     * - Instead of just saving to PlayerPrefs, also save to SQLite (Database)
     */
    private void OnApplicationQuit()
    {
        foreach (string key in stats.Keys)
        {
            PlayerPrefs.SetString(key, stats[key]);
        }
    }


    #endregion
    #region Getters & Setters

    public int GetMoney()
    {
        return money;
    }

    public void SetMoney(int amount)
    {
        UIController.Instance.GetHUD().ChangeMoney(this.money, amount, 5f); // Update's the money UI element
        this.money = amount;
        SetStat("MONEY", this.money.ToString());
    }

    public GameObject GetPlayer()
    {
        return player;
    }

    public string GetStat(string key, string _default)
    {
        if (!stats.ContainsKey(key))
            SetStat(key, PlayerPrefs.GetString(key, _default));

        return stats.ContainsKey(key) ? stats[key] : _default;
    }
    public int GetStat(string key, int _default) { return int.Parse(GetStat(key, _default.ToString())); }

    public void SetStat(string key, string stat)
    {
        if (!stats.ContainsKey(key))
            stats.Add(key, stat);

        stats[key] = stat;
    }

    #endregion
    #region Main


    public void StartGame() { StartCoroutine(_StartGame()); }
    IEnumerator _StartGame()
    {
        yield return new WaitForSeconds(0.3f);

        GAME_STATE = GameState.PLAYING;
    }


    void Update()
    {
        TrackMouse();
    }

    private void TrackMouse()
    {
        if (tileMaps == null || tileMaps.Length == 0) return;

        UIController.SetDebugStatistic("Mouse Pos XYZ", Input.mousePosition);

        Vector3 mousePos = Input.mousePosition;
        mousePos.z = -Camera.main.transform.position.z;
        Vector3 pos = Camera.main.ScreenToWorldPoint(mousePos);
        pos.x += 1.0f;
        pos.y += 1.0f;

        UIController.SetDebugStatistic("World Pos XYZ", pos);
        Vector3Int tilePos = tileMaps[0].WorldToCell(pos);

        string tileNames = "";
        foreach (Tilemap tileMap in tileMaps)
        {
            TileBase tile = tileMap.GetTile(tileMap.WorldToCell(pos));

            tileNames += "\t"+ tileMap.name + ": " + (tile != null ? tile.name : "null") +"\n";
        }

        UIController.SetDebugStatistic("Hovered Tile XY", tilePos.x + " " + tilePos.y + "\n" + tileNames);

        tileHoverSprite.transform.position = tilePos;
    }

    #endregion
}
