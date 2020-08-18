using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

#pragma warning disable 649

public class UIController : MonoBehaviour
{
    #region AccessVariables


    [Header("UI")]
    [SerializeField] private HUD hud;

    [Header("Menus")]
    [SerializeField] private Mainmenu mainmenu;


    #endregion
    #region PrivateVariables


    private enum MenuState { NONE, MAINMENU, SETTINGS }
    private MenuState menuState = MenuState.NONE;

    #endregion
    #region Initlization


    private static UIController instance;
    public static UIController Instance // Assign Singlton
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<UIController>();
                if (Instance == null)
                {
                    var instanceContainer = new GameObject("UIController");
                    instance = instanceContainer.AddComponent<UIController>();
                }
            }
            return instance;
        }
    }


    #endregion
    #region Getters & Setters


    public HUD GetHUD()
    {
        return hud;
    }

    public static void SetDebugStatistic(string statistic, string value)
    {
        UIController.Instance.GetHUD().SetDebugStatistic(statistic, value);
    }
    public static void SetDebugStatistic(string statistic, float value) { SetDebugStatistic(statistic, value.ToString()); }
    public static void SetDebugStatistic(string statistic, int value) { SetDebugStatistic(statistic, value.ToString()); }
    public static void SetDebugStatistic(string statistic, Vector3 value) { SetDebugStatistic(statistic, value.ToString()); }
    public static void SetDebugStatistic(string statistic, Vector2 value) { SetDebugStatistic(statistic, value.ToString()); }


    #endregion
}
