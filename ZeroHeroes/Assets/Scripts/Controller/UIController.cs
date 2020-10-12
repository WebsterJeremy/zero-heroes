﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;
using System;

#pragma warning disable 649

public class UIController : MonoBehaviour
{
    #region AccessVariables

    public Canvas canvas;
    public GameObject panelInteraction;
    public GameObject interactionItemPrefab;

    [Header("UI")]
    [SerializeField] private HUD hud;
    [SerializeField] private GameObject hudBlur;

    [Header("Menus")]
    [SerializeField] private MainMenu mainMenu;
    [SerializeField] private SettingsMenu settingsMenu;
    [SerializeField] private PauseMenu pauseMenu;
    [SerializeField] private InventoryMenu inventoryMenu;
    [SerializeField] private TasklogMenu tasklogMenu;
    [SerializeField] private BuildMenu buildMenu;

    [Header("Blur")]
    [SerializeField] Color blurColor = new Color(0.7f, 0.7f, 0.7f, 1f);
    [SerializeField] float blurIntensity = 10f;
    [SerializeField] float blurDuration = 1.5f;


    #endregion
    #region PrivateVariables


    private enum MenuState { NONE, MAINMENU, SETTINGS }
    private MenuState menuState = MenuState.NONE;

    private Material blurMaterial;

    private List<MenuBase> menus = new List<MenuBase>();

    #endregion
    #region Initlization

    private void Awake() {
        hud.gameObject.SetActive(false);

        menus.Add(settingsMenu);
        menus.Add(pauseMenu);
        menus.Add(inventoryMenu);
        menus.Add(tasklogMenu);
        menus.Add(buildMenu);

        CloseAllPanels();

        menus.Add(mainMenu);
        mainMenu.Open();

        blurMaterial = hudBlur.GetComponent<Image>().material;
    }

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

    public MainMenu GetMainMenu()
    {
        return mainMenu;
    }

    public SettingsMenu GetSettingsMenu()
    {
        return settingsMenu;
    }

    public PauseMenu GetPauseMenu()
    {
        return pauseMenu;
    }

    public InventoryMenu GetInventoryMenu()
    {
        return inventoryMenu;
    }

    public TasklogMenu GetTasklogMenu()
    {
        return tasklogMenu;
    }

    public BuildMenu GetBuildMenu()
    {
        return buildMenu;
    }

    public MenuBase[] GetMenus()
    {
        return menus.ToArray();
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
    #region Core


    public void CloseAllPanels() { CloseAllPanels(null); }
    public void CloseAllPanels(MenuBase except)
    {
        ShowHideInteractionPanel(false);

        if (menus.Count > 0)
        {
            foreach (MenuBase menu in menus)
            {
                if (menu == null) continue;
                if (except != null && except == menu) continue;
                menu.ForceClose();
            }
        }

        if (except != null && except.GetType() == typeof(MainMenu)) UIController.Instance.DisableBlur();
    }

    public void EnableBlur() { StartCoroutine(_EnableBlur()); }
    public IEnumerator _EnableBlur()
    {
        if (hudBlur == null || hudBlur.activeSelf) yield break;

        blurMaterial.SetColor("_Color", new Color(1, 1, 1, 1));
        blurMaterial.SetFloat("_Size", 0f);

        if (!hudBlur.activeSelf) hudBlur.SetActive(true);

        float animTime = 0f;
        float blurC;

        blurMaterial.color = new Color(1, 1, 1, 0f);

        while (animTime < blurDuration)
        {
            blurC = Mathf.Lerp(blurMaterial.color.r, blurColor.r, animTime / blurDuration);
            blurMaterial.color = new Color(blurC, blurC, blurC, blurC);
            blurMaterial.SetFloat("_Size", Mathf.Lerp(blurMaterial.GetFloat("_Size"), blurIntensity, animTime / blurDuration));

            yield return new WaitForEndOfFrame();
            animTime += Time.unscaledDeltaTime;
        }
    }

    public void DisableBlur() { StartCoroutine(_DisableBlur()); }
    public IEnumerator _DisableBlur()
    {
        if (hudBlur == null || !hudBlur.activeSelf) yield break;
        bool opened = false;

        foreach (MenuBase menu in menus)
        {
            if (menu.IsOpened() && menu.GetType() != typeof(PauseMenu) && menu.GetType() != typeof(BuildMenu)) opened = true;
        }

        if (opened) yield break; // Check if any other menu's are using the blur background still

        float animTime = 0f;
        float blurC;

        while (animTime < (blurDuration/2))
        {
            if (hudBlur == null) break;

            blurC = Mathf.Lerp(blurMaterial.color.r, 1f, animTime / (blurDuration/2));
            blurMaterial.color = new Color(blurC, blurC, blurC, blurC);
            blurMaterial.SetFloat("_Size", Mathf.Lerp(blurMaterial.GetFloat("_Size"), 0f, animTime / (blurDuration/2)));

            yield return new WaitForEndOfFrame();
            animTime += Time.unscaledDeltaTime;
        }

        if (hudBlur != null) hudBlur.SetActive(false);
    }


    #endregion
    #region Interaction Panel
    public void SetInteractionPanelPosition(Vector3 mousePos) {
        RectTransform rTransform = panelInteraction.GetComponent<RectTransform>();

        float width = rTransform.rect.width;
        float height = rTransform.rect.height;

        //                          offset so the toolbar is a litte off...
        Vector3 newPos = mousePos - new Vector3(width / 2, -16);

        newPos.z = 0f;

        //just know that this clamps the toolbar and dont mess with it
        if ((newPos.x + width * 2 * canvas.scaleFactor / 2) > Screen.width) {
            newPos.x = Screen.width - width;
        }

        if ((newPos.x * canvas.scaleFactor / 2) < 0) {
            newPos.x = 0;
        }

        if ((newPos.y + height * 2 * canvas.scaleFactor / 2) > Screen.height) {
            newPos.y = Screen.height;
        }

        if ((newPos.y * canvas.scaleFactor / 2) < 0) {
            newPos.y = height;
        }

        panelInteraction.transform.position = newPos;
    }
    public void ShowHideInteractionPanel(bool state) {
        panelInteraction.SetActive(state);
        //todo clear children
    }

    public void ClearInteractionPanelItems() {
        foreach (Transform child in panelInteraction.transform) {
            Destroy(child.gameObject);
        }
    }

    public void AddInteractionPanelItem(string _text, Action callback) {
        //spawn item
        GameObject item = Instantiate(interactionItemPrefab);
        item.transform.SetParent(panelInteraction.transform, false);

        //set text
        item.GetComponentInChildren<Text>().text = _text;

        //set callback
        item.GetComponent<Button>().onClick.AddListener(() => { callback.Invoke(); ShowHideInteractionPanel(false); });
    }


    #endregion
}
