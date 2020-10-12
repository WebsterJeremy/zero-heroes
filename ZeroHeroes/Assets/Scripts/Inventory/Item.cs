using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item
{
    #region AccessVariables

    public static ItemAttributes[] itemAttributesList;

    [Header("Item")]
    [SerializeField] private string id;
    [SerializeField] private int amount;


    #endregion
    #region PrivateVariables

    private ItemAttributes itemAttributes;

    #endregion
    #region Initlization


    private void Awake()
    {
        itemAttributes = FindItemAttributes(id);
    }

    public static void LoadItemAttributes()
    {
        itemAttributesList = Resources.LoadAll<ItemAttributes>("Items/");
    }

    public static ItemAttributes FindItemAttributes(string item_id)
    {
        if (itemAttributesList == null) LoadItemAttributes();

        foreach (ItemAttributes attr in itemAttributesList)
        {
            if (attr.GetID() == item_id) return attr;
        }

        return null;
    }

    public Item(string item_id, int amount)
    {
        this.id = item_id;
        itemAttributes = FindItemAttributes(id);
        this.amount = amount;
    }


    #endregion
    #region Getters and Setters

    public string GetTitle()
    {
        return itemAttributes.GetTitle();
    }

    public Sprite GetIcon()
    {
        return itemAttributes.GetIcon();
    }

    public float GetSellPrice()
    {
        return itemAttributes.GetSellPrice();
    }

    public float GetBuyPrice()
    {
        return itemAttributes.GetBuyPrice();
    }

    public string GetDescription()
    {
        return itemAttributes.GetDescription();
    }

    public string GetID()
    {
        return itemAttributes.GetID();
    }

    public int GetAmount()
    {
        return amount;
    }

    public void SetAmount(int amount)
    {
        this.amount = amount;
    }

    public void GiveAmount(int amount)
    {
        this.amount += amount;
    }

    #endregion
    #region Core



    #endregion
}
