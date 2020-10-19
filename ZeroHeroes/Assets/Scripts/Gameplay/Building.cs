using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Building : Entity
{
    #region AccessVariables

    public static BuildingAttributes[] buildingAttributesList;

    [Header("Entity")]
    [System.NonSerialized] public GameObject obj;
    [SerializeField] private int producedItems = 0;
    [SerializeField] private float lastRestockTime = 0; // When Time.time is greater then ( lastRestockTime + GetRestockTime() ), then restock [GetRestockTime() is an attribute of the building]


    #endregion
    #region PrivateVariables


    [System.NonSerialized] private BuildingAttributes buildingAttributes;


    #endregion
    #region Initlization

    protected override void Start()
    {
        base.Start();
    }

    public static void LoadBuildingAttributes()
    {
        buildingAttributesList = Resources.LoadAll<BuildingAttributes>("Entities/Buildings/");
    }

    public static BuildingAttributes FindBuildingAttributes(string entity_id)
    {
        if (buildingAttributesList == null) LoadBuildingAttributes();

        foreach (BuildingAttributes attr in buildingAttributesList)
        {
            if (attr.GetID().Equals(entity_id)) return attr;
        }

        return null;
    }

    #endregion
    #region Getters and Setters

    protected BuildingAttributes GetBuildingAttributes()
    {
        if (buildingAttributes == null) buildingAttributes = FindBuildingAttributes(GetID());

        return buildingAttributes;
    }

    public float GetProduceTime()
    {
        return GetBuildingAttributes().GetProduceTime();
    }

    public Vector2 GetProduceQuantity()
    {
        return GetBuildingAttributes().GetProduceQuantity();
    }

    public float GetRestockTime()
    {
        return GetBuildingAttributes().GetRestockTime();
    }

    public float GetRestockPrice()
    {
        return GetBuildingAttributes().GetRestockPrice();
    }

    public string GetProducedItem()
    {
        return GetBuildingAttributes().GetProducedItem();
    }

    #endregion
    #region Core

    private void OnMouseDown()
    {
        if (producedItems > 0)
        {
            Debug.Log("No produced items opening menu for " + GetTitle());
        }
        else
        {
            GameController.Instance.GetInventory().GiveItem(GetProducedItem(), 10, -1); // Slot of -1, means it will find a new empty slot in the inventory instead

            Debug.Log("Giving player stocked items!");
            producedItems = 0; // Remove old stock
        }
    }

    #endregion
}
