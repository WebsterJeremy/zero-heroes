using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System;

public class BuildingInspectMenu : MenuBase
{
    #region AccessVariables


    [Header("Buttons")]
    [SerializeField] private Button buttonClose;
    [SerializeField] private Button buttonStock;
    [SerializeField] private Button buttonMove;
    [SerializeField] private Button buttonSell;
    [SerializeField] private Button buttonPlant;

    [Header("Functionality")]
    [SerializeField] private TextMeshProUGUI textTitle;
    [SerializeField] private TextMeshProUGUI textDescription;
    [SerializeField] private TextMeshProUGUI textTime;
    [SerializeField] private RectTransform rectSummary;
    [SerializeField] private RectTransform rectTime;
    [SerializeField] private RectTransform rectPlant;

    [Header("Stock")]
    [SerializeField] private TextMeshProUGUI textStock;
    [SerializeField] private TextMeshProUGUI textStockQuantity;
    [SerializeField] private Image imgStock;
    [SerializeField] private RectTransform rectStock;

    #endregion
    #region PrivateVariables


    private Building selectedBuilding;

    private float produceTime;


    #endregion
    #region Initlization


    protected override void Start()
    {
        base.Start();
    }


    #endregion
    #region Core


    protected override void AddButtonListeners()
    {
        buttonClose.onClick.AddListener(() =>
        {
            SoundController.PlaySound("button");

            Close();
        });
        buttonStock.onClick.AddListener(() =>
        {
            SoundController.PlaySound("button");

            if (selectedBuilding == null) return;

            ItemAttributes sItem = Item.FindItemAttributes(selectedBuilding.GetStockedItem());
            if (sItem == null) return;

            if (GameController.Instance.GetInventory().TakeItem(sItem.GetID(), 5))
            {
                selectedBuilding.SetStockQuantity(Mathf.Clamp(selectedBuilding.GetStockQuantity() + 5, 0, sItem.GetMaxQuantity()));
                UpdateDisplay();
            }
        });
        buttonMove.onClick.AddListener(() =>
        {
            SoundController.PlaySound("button");

            if (selectedBuilding == null) return;

            Close();
        });
        buttonSell.onClick.AddListener(() =>
        {
            SoundController.PlaySound("button");

            if (selectedBuilding == null) return;
            if (selectedBuilding.GetSellPrice() == 0) return;

            UIController.Instance.GetPopup().Setup("Sell Building",
                "Are you sure you want to sell this building?",
                UIController.Instance.GetMoneyIcon(), "Sells for $"+ selectedBuilding.GetSellPrice(), () => {
                    GameController.Instance.GiveMoney((int) selectedBuilding.GetSellPrice());
                    SoundController.PlaySound("sell_buy_item");
                    GameController.Instance.RemoveBuilding(selectedBuilding);
                    Close();
            }, () => { }, () => { UIController.Instance.GetPopup().check = true; });
        });
        buttonPlant.onClick.AddListener(() =>
        {
            SoundController.PlaySound("button");

            if (selectedBuilding == null) return;

            rectPlant.gameObject.SetActive(true);
        });
    }

    protected override IEnumerator _Open()
    {
        if (IsOpened()) yield break;

        rectMenu.gameObject.SetActive(true);
        UIController.Instance.EnableBlur();

        UpdateDisplay(true);
    }

    protected override IEnumerator _Close()
    {
        if (!IsOpened()) yield break;

        rectMenu.gameObject.SetActive(false);
    }

    public void InspectBuilding(Building building)
    {
        selectedBuilding = building;

        Open();
    }

    public void UpdateDisplay() { if (gameObject.activeSelf) StartCoroutine(_UpdateDisplay(false)); }
    private void UpdateDisplay(bool resize) { if (gameObject.activeSelf) StartCoroutine(_UpdateDisplay(resize)); }
    private IEnumerator _UpdateDisplay(bool resize)
    {
        if (selectedBuilding == null) yield break;

        textTitle.text = selectedBuilding.GetTitle();
        textDescription.text = selectedBuilding.GetDescription();

        

        if (!selectedBuilding.GetStockedItem().Equals(string.Empty))
        {
            textStock.text = (selectedBuilding.GetStockQuantity() > 0 ? "Stocked" : "No Stock");
            textStockQuantity.text = (selectedBuilding.GetStockQuantity() > 0 ? "x" + selectedBuilding.GetStockQuantity() : "");

            ItemAttributes sItem = Item.FindItemAttributes(selectedBuilding.GetStockedItem());
            if (sItem != null)
            {
                imgStock.sprite = sItem.GetIcon();
            }
        }

        rectTime.gameObject.SetActive(selectedBuilding.GetProduces());
        rectStock.gameObject.SetActive(!selectedBuilding.GetStockedItem().Equals(string.Empty));

        buttonSell.gameObject.SetActive(selectedBuilding.GetSellPrice() != 0);

        buttonPlant.gameObject.SetActive(selectedBuilding.GetType() == typeof(BuildingFarm));

        if (resize)
        {
            yield return new WaitForEndOfFrame();

            float summaryHeight = textDescription.preferredHeight + 140 + (selectedBuilding.GetProduces() ? 80 : 0);
            rectSummary.sizeDelta = new Vector2(rectSummary.sizeDelta.x, summaryHeight);
            rectMenu.sizeDelta = new Vector2(rectMenu.sizeDelta.x, summaryHeight + (rectStock.gameObject.activeSelf ? 120 : 0) + 220);
        }
    }

    private void Update()
    {
        if (selectedBuilding == null || !selectedBuilding.GetProduces()) return;

        string timeStr;

        produceTime = selectedBuilding.GetNextProduceTime();

        if (produceTime - Time.time >= 3600) timeStr = Mathf.FloorToInt((produceTime - Time.time) / 3600) + " Hours";
        else if (produceTime - Time.time >= 60) timeStr = Mathf.FloorToInt((produceTime - Time.time) / 60) + " Minutes";
        else timeStr = (int)(produceTime - Time.time) + " Seconds";

        textTime.text = "Producing in " + timeStr;
    }

    public void SelectCrop(string crop)
    {
        if (selectedBuilding == null || selectedBuilding.GetType() != typeof(BuildingFarm)) return;
        BuildingFarm farm = (BuildingFarm)selectedBuilding;

        farm.SetCrop(crop);
        UpdateDisplay(true);
        rectPlant.gameObject.SetActive(false);
    }


    #endregion
}
