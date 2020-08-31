﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;
using System;
using Assets.Scripts.Gameplay;

#pragma warning disable 649

public class UIController : MonoBehaviour
{
    #region AccessVariables

    public Canvas canvas;
    public GameObject panelInteraction;
    public GameObject panelInventory;
    public GameObject interactionItemPrefab;
    public GameObject inventoryItemPrefab;

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

    private void Awake() {
        CloseAllPanels();
    }

    public void CloseAllPanels() {
        ShowHideInteractionPanel(false);
        ShowHideInventoryPanel(false);
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

    public static void SetDebugStatistic(string statistic, string value)
    {
        UIController.Instance.GetHUD().SetDebugStatistic(statistic, value);
    }
    public static void SetDebugStatistic(string statistic, float value) { SetDebugStatistic(statistic, value.ToString()); }
    public static void SetDebugStatistic(string statistic, int value) { SetDebugStatistic(statistic, value.ToString()); }
    public static void SetDebugStatistic(string statistic, Vector3 value) { SetDebugStatistic(statistic, value.ToString()); }
    public static void SetDebugStatistic(string statistic, Vector2 value) { SetDebugStatistic(statistic, value.ToString()); }


    #endregion


    #region Inventory Panel

    public void ChangeStateInventoryPanel() {
        panelInventory.SetActive(!panelInventory.activeSelf);
    }
    public void ShowHideInventoryPanel(bool _state) {
        panelInventory.SetActive(_state);
    }

    public void UpdateInventory(List<InventoryItem> _items) {
        //called in inventory when an item is added or removed...
        //remove all inventory items...
        RemoveAllInventoryUIItems();
        //readd them.. now with updated value...
        foreach(InventoryItem ii in _items) {
            //todo add..
            AddInventoryPanelItem(ii);
        }
    }

    public void RemoveAllInventoryUIItems() {
        foreach (Transform child in panelInventory.transform.Find("Content").transform) {
            Destroy(child.gameObject);
        }
    }

    public void AddInventoryPanelItem(InventoryItem _item) {
        //spawn item
        GameObject item = Instantiate(inventoryItemPrefab);
        item.transform.SetParent(panelInventory.transform.Find("Content").transform, false);

        //set text
        item.transform.Find("Amount").GetComponent<Text>().text = _item.Amount.ToString();//this is the amount
        item.transform.Find("Image").GetComponent<Image>().sprite = _item.Sprite();//this is the amount

        //set callback
        //todo set to actually show an inventory context menu.. from there choose to drop, action, examine, etc....
        item.GetComponent<Button>().onClick.AddListener(() => { GameController.Instance.Player.AttemptDropInventoryItem(_item.Id); });
    }



    #endregion

    #region interaction panel
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
