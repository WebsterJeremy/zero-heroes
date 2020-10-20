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
    [System.NonSerialized] private Notify notify;


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

    public bool GetProduces()
    {
        return GetBuildingAttributes().GetProduces();
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

        if (producedItems > 0)
        {
            if (notify == null)
            {
                Sprite icon = GetIcon();
                if (GetProducedItem() != null && !GetProducedItem().Equals(string.Empty))
                {
                    ItemAttributes attr = Item.FindItemAttributes(GetProducedItem());
                    if (attr != null)
                    {
                        icon = attr.GetIcon();
                    }
                }

                notify = UIController.Instance.GetHUD().CreateNotify(GetTitle(), this.producedItems, icon, transform, new Vector3(GetSize().x / 2, GetSize().y, 0));
            }

            notify.SetQuantity(this.producedItems);
        }
        else
        {
            if (notify != null)
            {
                UIController.Instance.GetHUD().RemoveNotify(notify);
                notify = null;
            }
        }
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
        if (GetProduces())
        {
            if (producedItems < 1 || GetProducedItem() == null || GetProducedItem().Equals(string.Empty))
            {
                Debug.Log("No produced items opening menu for " + GetTitle() +" : Next produce in "+ (lastProduceTime - Time.time) + " seconds");
            }
            else
            {
                GameController.Instance.GetInventory().GiveItem(GetProducedItem(), producedItems, -1); // Slot of -1, means it will find a new empty slot in the inventory instead

                if (GetEcoFriendly())
                {
                    GameController.Instance.SetPoints(GameController.Instance.GetPoints() + Random.Range((int)GetEcoFriendlyPoints().x, (int)GetEcoFriendlyPoints().y));
                }

                // How many seconds have pasted / GetProduceTime()

                Debug.Log("Giving player stocked items!");
                SetProducedItems(0);
            }
        }
        else
        {
            Debug.Log("Opening menu!");
        }
    }

    private void Update()
    {
        if (GetProduces() && lastProduceTime < Time.time)
        {
            SetProducedItems((int) Random.Range(GetProduceQuantity().x, GetProduceQuantity().y));

            lastProduceTime = Time.time + GetProduceTime();
        }
    }

    public void CalculateIdledProduces(int secondsSinceSave)
    {
        if (!GetProduces()) return;

        // Debug.Log("Time elapsed since last save " + secondsSinceSave + " seconds");
    }

    #endregion

}
