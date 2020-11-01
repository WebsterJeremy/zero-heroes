using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System;

public class MapMenu : MenuBase
{
    #region AccessVariables

    [Header("Buttons")]
    [SerializeField] private Button buttonClose;
    [SerializeField] private Button buttonTravel;

    [SerializeField] private Button buttonTransport;
    [SerializeField] private Button buttonStopWaiting;

    [Header("Functionality")]
    [SerializeField] private GameObject prefabVehicleElement;
    [SerializeField] private RectTransform rectContent;

    [Serializable]
    public struct TransportMode
    {
        public Sprite icon;
        public string title;
        public int price;
        public int points;
        public int time;
        public bool requireGarage;
    }
    [SerializeField] private TransportMode[] transportModes;

    [Header("Traveling Popup")]
    [SerializeField] private TextMeshProUGUI textTimeLeft;
    [SerializeField] private RectTransform rectTraveling;


    #endregion
    #region PrivateVariables

    private VehicleCardElement selectedCard;
    private List<VehicleCardElement> cards = new List<VehicleCardElement>();

    private float travelTime = -1;

    #endregion
    #region Initlization


    protected override void Start()
    {
        base.Start();

        Populate();

        if (GameController.Instance.GetStat("TravelWaitTimer", -1) == -1)
        {
            travelTime = -1;
        }
        else
        {
            travelTime = (float)(Time.time + (GameController.Instance.GetStat("TravelWaitTimer", 0) - GameController.Instance.GetGameTime() - GameController.Instance.GetTimeSinceSave()));
            GameController.Instance.SetStat("TravelWaitTimer", travelTime);
        }
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
        buttonTravel.onClick.AddListener(() =>
        {
            SoundController.PlaySound("button");

            Travel();
        });
        buttonTransport.onClick.AddListener(() =>
        {
            SoundController.PlaySound("button");

            if (travelTime != -1 && travelTime <= Time.time) Transport();
        });
        buttonStopWaiting.onClick.AddListener(() =>
        {
            SoundController.PlaySound("button");

            StopWaiting();
        });
    }

    protected override IEnumerator _Open()
    {
        if (IsOpened()) yield break;

        rectMenu.gameObject.SetActive(true);
        UIController.Instance.EnableBlur();
    }

    protected override IEnumerator _Close()
    {
        if (!IsOpened()) yield break;

        rectMenu.gameObject.SetActive(false);
    }

    private void Populate()
    {
        string oldSelected = (selectedCard != null ? selectedCard.textTitle.text : "");
        selectedCard = null;

        if (selectedCard != null)
        {
            selectedCard.GetComponent<Image>().color = new Color(1, 1, 1, 1);
        }

        if (cards != null && cards.Count > 0)
        {
            for (int i = 0; i < cards.Count; i++)
            {
                Destroy(cards[i].gameObject);
            }

            cards.Clear();
        }

        if (transportModes != null && transportModes.Length > 0)
        {
            for (int i = 0; i < transportModes.Length; i++)
            {
                GameObject element = Instantiate(prefabVehicleElement);
                element.transform.SetParent(rectContent);

                VehicleCardElement card = element.GetComponent<VehicleCardElement>();

                card.Setup(transportModes[i]);
                cards.Add(card);

                if (selectedCard == null || transportModes[i].title == oldSelected)
                {
                    Select(card);
                }
            }
        }
    }

    public void Select(VehicleCardElement select)
    {
        if (select == null || !select.CheckBuyable()) return;
        if (selectedCard != null) selectedCard.GetComponent<Image>().color = new Color(1, 1, 1);

        selectedCard = select;

        selectedCard.GetComponent<Image>().color = new Color32(200, 200, 200, 255);
    }

    private void Travel()
    {
        if (selectedCard == null) return;
        if (!selectedCard.CheckBuyable()) return;


        GameController.Instance.GiveMoney(-selectedCard.transportMode.price);
        GameController.Instance.GivePoints(-selectedCard.transportMode.points);

        SoundController.PlaySound("sell_buy_item");

        travelTime = Time.time + selectedCard.transportMode.time;
        GameController.Instance.SetStat("TravelWaitTimer", travelTime);

        UpdatePrices();
    }

    private void Update()
    {
        if (travelTime != -1)
        {
            if (!rectTraveling.gameObject.activeSelf) rectTraveling.gameObject.SetActive(true);

            if (travelTime > Time.time)
            {
                string timeStr;

                if (travelTime - Time.time >= 3600) timeStr = Mathf.FloorToInt((travelTime - Time.time) / 3600) + " Hours";
                else if (travelTime - Time.time >= 60) timeStr = Mathf.FloorToInt((travelTime - Time.time) / 60) + " Minutes";
                else timeStr = (int)(travelTime - Time.time) + " Seconds";

                textTimeLeft.text = "Arriving in " + timeStr;
                if (buttonTransport.interactable)
                {
                    buttonTransport.interactable = false;
                    buttonTransport.GetComponent<Image>().color = new Color32(200, 200, 200, 200);
                }
            }
            else
            {
                textTimeLeft.text = "Transport has arrived!";

                if (!buttonTransport.interactable)
                {
                    buttonTransport.interactable = true;
                    buttonTransport.GetComponent<Image>().color = new Color(1, 1, 1, 1);
                }
            }
        } else if (rectTraveling.gameObject.activeSelf) rectTraveling.gameObject.SetActive(false);
    }

    private void Transport()
    {
        travelTime = -1;
        GameController.Instance.SetStat("TravelWaitTimer", travelTime);

        Debug.Log("You have been transported to the City");
        GameController.Instance.StopGame();
    }

    private void StopWaiting()
    {
        travelTime = -1;
        GameController.Instance.SetStat("TravelWaitTimer", travelTime);
    }

    private float lastCardPriceUpdate;
    private void UpdatePrices()
    {
        if (!rectMenu.gameObject.activeSelf || lastCardPriceUpdate > (Time.time - 5)) return;
        lastCardPriceUpdate = Time.time;

        foreach (VehicleCardElement card in cards)
        {
            card.CheckBuyable();
        }
    }

    #endregion
}
