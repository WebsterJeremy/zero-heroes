using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Item
{
    #region AccessVariables

    public static ItemAttributes[] itemAttributesList;

    [Header("Item")]
    [SerializeField] private string id;
    [SerializeField] private int quantity;
    [SerializeField] private int slot;


    #endregion
    #region PrivateVariables

    [System.NonSerialized] private ItemAttributes itemAttributes;

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
            if (attr.GetID().Equals(item_id)) return attr;
        }

        return null;
    }

    public Item(string item_id, int quantity)
    {
        this.id = item_id;
        itemAttributes = FindItemAttributes(id);
        this.quantity = quantity;
    }


    #endregion
    #region Getters and Setters

    private ItemAttributes GetItemAttributes()
    {
        if (itemAttributes == null) itemAttributes = FindItemAttributes(id);

        return itemAttributes;
    }

    public string GetTitle()
    {
        return GetItemAttributes().GetTitle();
    }

    public Sprite GetIcon()
    {
        return GetItemAttributes().GetIcon();
    }

    public float GetSellPrice()
    {
        return GetItemAttributes().GetSellPrice();
    }

    public float GetBuyPrice()
    {
        return GetItemAttributes().GetBuyPrice();
    }

    public string GetDescription()
    {
        return GetItemAttributes().GetDescription();
    }

    public string GetID()
    {
        return GetItemAttributes().GetID();
    }

    public int GetQuantityMax()
    {
        return GetItemAttributes().GetMaxQuantity();
    }

    public int GetQuantity()
    {
        return quantity;
    }

    public void SetQuantity(int quantity)
    {
        this.quantity = quantity;
    }

    public void GiveQuantity(int quantity)
    {
        this.quantity += quantity;
    }

    public int GetSlot()
    {
        return slot;
    }

    public void SetSlot(int slot)
    {
        this.slot = slot;
    }

    #endregion
    #region Core

    public bool Delete()
    {
        return GameController.Instance.GetInventory().RemoveItem(GetSlot());
    }

    public bool Sell()
    {
        if (Delete())
        {
            GameController.Instance.GiveMoney((int)(GetSellPrice() * GetQuantity()));
            SoundController.PlaySound("sell_buy_item");
        }

        return true;
    }

    public bool Use()
    {
        return false;
    }

    #endregion
}
