using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Building : Entity
{
    #region AccessVariables

    [System.NonSerialized] public static BuildingAttributes[] buildingAttributesList;

    [Header("Entity")]
    [SerializeField] private int producedItems = 0;
    [SerializeField] private float lastProduceTime = 0;
    [SerializeField] private bool restocked = true;
    [SerializeField] private float lastRestockTime = 0; // When Time.time is greater then ( lastRestockTime + GetRestockTime() ), then restock [GetRestockTime() is an attribute of the building]


    #endregion
    #region PrivateVariables


    [System.NonSerialized] private BuildingAttributes buildingAttributes;


    #endregion
    #region Initlization

    protected override void Start()
    {
        base.Start();

        Debug.Log("Buidling Start : "+ this);
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

    public bool GetEcoFriendly()
    {
        return GetBuildingAttributes().GetEcoFriendly();
    }

    public Vector2 GetEcoFriendlyPoints()
    {
        return GetBuildingAttributes().GetEcoFriendlyPoints();
    }


    public int GetProducedItems()
    {
        return producedItems;
    }

    public void SetProducedItems(int producedItems)
    {
        this.producedItems = producedItems;
    }

    public float GetLastProduceTime()
    {
        return lastProduceTime;
    }

    public void SetLastProduceTime(float lastProduceTime)
    {
        this.lastProduceTime = lastProduceTime;
    }

    public bool GetRestocked()
    {
        return restocked;
    }

    public void SetRestocked(bool restocked)
    {
        this.restocked = restocked;
    }

    public float GetLastRestockTime()
    {
        return lastRestockTime;
    }

    public void SetLastRestockTime(float lastRestockTime)
    {
        this.lastRestockTime = lastRestockTime;
    }

    #endregion
    #region Core

    public void CopyTo(Building ent)
    {
        ent.SetProducedItems(ent.GetProducedItems());
        ent.SetLastProduceTime(ent.GetLastProduceTime());
        ent.SetRestocked(ent.GetRestocked());
        ent.SetLastRestockTime(ent.GetLastRestockTime());
    }

    private void OnMouseDown()
    {
        if (producedItems > 0)
        {
            Debug.Log("No produced items opening menu for " + GetTitle());
        }
        else
        {
            GameController.Instance.GetInventory().GiveItem(GetProducedItem(), 10, -1); // Slot of -1, means it will find a new empty slot in the inventory instead

            if (GetEcoFriendly())
            {
                GameController.Instance.SetPoints(GameController.Instance.GetPoints() + Random.Range((int)GetEcoFriendlyPoints().x, (int)GetEcoFriendlyPoints().y));
            }

            // How many seconds have pasted / GetProduceTime()

            Debug.Log("Giving player stocked items!");
            producedItems = 0; // Remove old stock
        }
    }

    #endregion

}
