using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Building", menuName = "Gameplay/Building")]
public class BuildingAttributes : EntityAttributes
{
    #region AccessVariables


    [Header("Building")]
    [SerializeField] private float sellPrice = 10; // Price to sell [In Money]
    [SerializeField] private float buyPrice = 10; // Price to buy [In Money]
    [SerializeField] private float buyEcoPrice = 1; // Price to buy [In Eco Points]
    [SerializeField] private bool produces = false; // Does this item produce items
    [SerializeField] private float produceTime = 120; // How often does it produce items [In Seconds]
    [SerializeField] private Vector2 produceQuantity = new Vector2(20,30); // How many items does it produce each time [Random between Min,Max]
    [SerializeField] private string stockedItem; // What item stocks it?
    [SerializeField] private string producedItem; // Which item does it produce?
    [SerializeField] private bool ecoFriendly = false;
    [SerializeField] private Vector2 ecoFriendlyPoints = new Vector2(1, 3);

    #endregion
    #region PrivateVariables


    #endregion
    #region Initlization


    protected override void Awake()
    {
        base.Awake();
    }


    #endregion
    #region Getters and Setters

    public float GetSellPrice()
    {
        return sellPrice;
    }

    public float GetBuyPrice()
    {
        return buyPrice;
    }

    public float GetBuyEcoPrice()
    {
        return buyEcoPrice;
    }

    public float GetProduceTime()
    {
        return produceTime;
    }

    public bool GetProduces()
    {
        return produces;
    }

    public Vector2 GetProduceQuantity()
    {
        return produceQuantity;
    }

    public string GetStockedItem()
    {
        return stockedItem;
    }

    public string GetProducedItem()
    {
        return producedItem;
    }

    public bool GetEcoFriendly()
    {
        return ecoFriendly;
    }

    public Vector2 GetEcoFriendlyPoints()
    {
        return ecoFriendlyPoints;
    }

    #endregion
    #region Core



    #endregion
}
