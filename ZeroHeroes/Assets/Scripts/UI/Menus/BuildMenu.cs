using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BuildMenu : MenuBase
{
    #region AccessVariables


    [Header("Buttons")]
    [SerializeField] private Button buttonNext;
    [SerializeField] private Button buttonPrev;
    [SerializeField] private Button buttonBuild;
    [SerializeField] private Button buttonCancel;

    [Header("Functionality")]
    [SerializeField] private GameObject prefabBuildingElement;
    [SerializeField] private BuildingMarker marker;
    [SerializeField] private RectTransform rectContent;
    [SerializeField] private RectTransform rectMarkerHUD;
    [SerializeField] private string[] buildings;


    #endregion
    #region PrivateVariables

    private float offset = 0;
    private string selectedBuilding = null;
    private List<BuildingCardElement> cards = new List<BuildingCardElement>();

    private float lastCardPriceUpdate = 0;

    #endregion
    #region Initlization


    protected override void Start()
    {
        base.Start();

        rectMenu.anchoredPosition = new Vector2(0, -300);
        Populate();
    }

    public bool GetMarkerActive()
    {
        return (marker.gameObject.activeSelf);
    }

    #endregion
    #region Core

    public void ToggleVisibility()
    {
        if (IsOpened()) Close(); else Open();
    }

    protected override void AddButtonListeners()
    {
        buttonNext.onClick.AddListener(() =>
        {
            SoundController.PlaySound("button");

            ShiftCards(true);
        });
        buttonPrev.onClick.AddListener(() =>
        {
            SoundController.PlaySound("button");

            ShiftCards(false);
        });
        buttonBuild.onClick.AddListener(() =>
        {
            SoundController.PlaySound("button");

            Build();
        });
        buttonCancel.onClick.AddListener(() =>
        {
            SoundController.PlaySound("button");

            Cancel();
        });
    }

    protected override IEnumerator _Open()
    {
        if (IsOpened()) yield break;

        rectMenu.gameObject.SetActive(true);
        inTransistion = true;

        EffectController.TweenAnchor(rectMenu, new Vector2(0, 20), 1f, false, () => {});

        UpdateCardPrices();

        yield return new WaitForSeconds(0.2f);

        inTransistion = false;
    }

    protected override IEnumerator _Close()
    {
        if (!IsOpened()) yield break;
        inTransistion = true;

        EffectController.TweenAnchor(rectMenu, new Vector2(0, -300), 0.3f, false, () => {});

        yield return new WaitForSeconds(0.5f);

        rectMenu.gameObject.SetActive(false);
        inTransistion = false; 
    }

    private void Populate()
    {
        if (buildings.Length < 1) return;
        cards.Clear();

        for (int i = 0;i < buildings.Length;i++)
        {
            GameObject element = Instantiate(prefabBuildingElement);
            element.transform.SetParent(rectContent);

            BuildingCardElement card = element.GetComponent<BuildingCardElement>();

            card.Setup(Building.FindBuildingAttributes(buildings[i]));
            cards.Add(card);
        }
    }

    private void ShiftCards(bool next)
    {
        if (next && offset == 0) return;
        if (!next && -offset == (buildings.Length - 5) * 210) return;

        offset += (next ? 210f : -210f);
        buttonNext.interactable = false;
        buttonPrev.interactable = false;

        EffectController.TweenPosition(rectContent, new Vector2(offset, 0), 0.2f, () => {
            buttonNext.interactable = true;
            buttonPrev.interactable = true;
        });
    }

    public void Select(string buildingId)
    {
        marker.gameObject.SetActive(true);
        rectMarkerHUD.gameObject.SetActive(true);
        selectedBuilding = buildingId;

        BuildingAttributes attributes = Building.FindBuildingAttributes(buildingId);

        if (attributes == null) return;

        marker.Setup(attributes);
        marker.StartDragging();
    }

    public void Build()
    {
        if (selectedBuilding == null) return;

        BuildingAttributes attributes = Building.FindBuildingAttributes(selectedBuilding);
        if (attributes == null) return;

        if (GameController.Instance.GetMoney() >= attributes.GetBuyPrice() && GameController.Instance.GetPoints() >= attributes.GetBuyEcoPrice())
        {
            GameController.Instance.GiveMoney((int) -attributes.GetBuyPrice());
            GameController.Instance.GivePoints((int) -attributes.GetBuyEcoPrice());

            SoundController.PlaySound("sell_buy_item");
            SoundController.PlaySound("building_place");

            GameController.Instance.SpawnBuilding(selectedBuilding, new Vector3(marker.transform.position.x - 1f, marker.transform.position.y - 1f, 0));

            GameController.Instance.CheckObjectivies(TaskAttributes.ObjectiveType.BUILDING);

            Cancel();
        }
    }

    public void Cancel()
    {
        marker.gameObject.SetActive(false);
        rectMarkerHUD.gameObject.SetActive(false);
        selectedBuilding = null;
    }

    public void UpdateCardPrices()
    {
        if (!rectMenu.gameObject.activeSelf || lastCardPriceUpdate > (Time.time - 5)) return;
        lastCardPriceUpdate = Time.time;

        foreach (BuildingCardElement card in cards)
        {
            card.CheckBuyable();
        }
    }

    #endregion
}