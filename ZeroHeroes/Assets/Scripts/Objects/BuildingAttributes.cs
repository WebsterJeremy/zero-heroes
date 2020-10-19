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
    [SerializeField] private float produceTime = 120; // How often does it produce items [In Seconds]
    [SerializeField] private Vector2 produceQuantity = new Vector2(20,30); // How many items does it produce each time [Random between Min,Max]
    [SerializeField] private float restockTime = 300; // How often does the player need to restock [In Minutes]
    [SerializeField] private float restockPrice = 300; // How much does it cost to restock [In Money]
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

    public float GetProduceTime()
    {
        return produceTime;
    }

    public Vector2 GetProduceQuantity()
    {
        return produceQuantity;
    }

    public float GetRestockTime()
    {
        return restockTime;
    }

    public float GetRestockPrice()
    {
        return restockPrice;
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
