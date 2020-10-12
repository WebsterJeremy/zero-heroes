using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Item", menuName = "Inventory/Item")]
public class ItemAttributes : ScriptableObject
{
    #region AccessVariables


    [Header("Item")]
    [SerializeField] private string title = "New Item";
    [SerializeField] private Sprite icon;
    [SerializeField] private float sellPrice = 10;
    [SerializeField] private float buyPrice = 10;
    [TextArea(20, 20)]
    [SerializeField] private string description = "None";


    #endregion
    #region PrivateVariables


    #endregion
    #region Initlization


    private void Awake()
    {

    }


    #endregion
    #region Getters and Setters

    public string GetTitle()
    {
        return title;
    }

    public Sprite GetIcon()
    {
        return icon;
    }

    public float GetSellPrice()
    {
        return sellPrice;
    }

    public float GetBuyPrice()
    {
        return buyPrice;
    }

    public string GetDescription()
    {
        return description;
    }

    public string GetID()
    {
        string id = name.ToLower();
        id.Replace(" ", "_");

        return id;
    }

    #endregion
    #region Core



    #endregion
}
