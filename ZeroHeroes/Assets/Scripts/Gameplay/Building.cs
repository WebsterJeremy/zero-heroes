using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;


public class Building : Entity
{
    public static float OFFLINE_PENALTY_PERCENT = 0.1f; // 10% increased times per production when offline

    #region AccessVariables

    [System.NonSerialized] public static BuildingAttributes[] buildingAttributesList;

    [Header("Entity")]
    [SerializeField] protected int producedItems = 0;
    [SerializeField] protected float nextProduceTime = 999;
    [SerializeField] protected int stockQuantity = 0;

    /*
    private float realTime; //placeholder so that timespan can be changed 
    [SerializeField]DateTime oldDate; //start time away interval0
    DateTime currentDate;  //
    */

    #endregion
    #region PrivateVariables


    [System.NonSerialized] protected BuildingAttributes buildingAttributes;
    [System.NonSerialized] protected Notify notify;


    #endregion
    #region Initlization

    protected override void Start()
    {
        base.Start();

        SetNextProduceTime(GetProduceTime());

        Building.AddColliders(transform.position, GetSize());
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

    public float GetSellPrice()
    {
        return GetBuildingAttributes().GetSellPrice();
    }

    public float GetBuyPrice()
    {
        return GetBuildingAttributes().GetBuyPrice();
    }

    public float GetBuyEcoPrice()
    {
        return GetBuildingAttributes().GetBuyEcoPrice();
    }

    public float GetProduceTime()
    {
        return GetBuildingAttributes().GetProduceTime();
    }

    public virtual bool GetProduces()
    {
        return GetBuildingAttributes().GetProduces();
    }

    public Vector2 GetProduceQuantity()
    {
        return GetBuildingAttributes().GetProduceQuantity();
    }

    public virtual string GetStockedItem()
    {
        return GetBuildingAttributes().GetStockedItem();
    }

    public virtual string GetProducedItem()
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
        if (!GetProduces() || GetProducedItem() == null) return;

        ItemAttributes attr = Item.FindItemAttributes(GetProducedItem());
        if (attr == null) return;

        this.producedItems = Mathf.Clamp(producedItems,0,attr.GetMaxQuantity());

        if (producedItems > 0)
        {
            if (notify == null)
            {
                Sprite icon = GetIcon();
                if (GetProducedItem() != null && !GetProducedItem().Equals(string.Empty))
                {
                    icon = attr.GetIcon();
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

    public float GetNextProduceTime()
    {
        return nextProduceTime;
    }

    public void SetNextProduceTime(float nextProduceTime)
    {
        this.nextProduceTime = Time.time + Mathf.Clamp(nextProduceTime, 0, GetProduceTime());
    }

    public int GetStockQuantity()
    {
        return stockQuantity;
    }

    public void SetStockQuantity(int stock)
    {
        stockQuantity = stock;
    }


    #endregion
    #region Core

    public void CopyTo(Building ent)
    {
        ent.SetProducedItems(ent.GetProducedItems());
        ent.SetNextProduceTime(ent.GetNextProduceTime());
        ent.SetStockQuantity(ent.GetStockQuantity());
    }

    private void OnMouseDown()
    {
        if (EventSystem.current.currentSelectedGameObject != null || !CameraFollower.PANNING_ENABLED || !GameController.PLAYING() || !CameraFollower.ENABLED || UIController.Instance.GetBuildMenu().GetMarkerActive()) return;

        if (GetProduces())
        {
            if (producedItems < 1 || GetProducedItem() == null || GetProducedItem().Equals(string.Empty))
            {
                OpenMenu();
            }
            else
            {
                Collect();
            }
        }
        else
        {
            OpenMenu();
        }
    }

    protected virtual void Collect()
    {
        GameController.Instance.GetInventory().GiveItem(GetProducedItem(), producedItems, -1); // Slot of -1, means it will find a new empty slot in the inventory instead
        SoundController.PlaySound("inventory_pickup");

        if (GetEcoFriendly())
        {
            GameController.Instance.SetPoints(GameController.Instance.GetPoints() + Random.Range((int)GetEcoFriendlyPoints().x, (int)GetEcoFriendlyPoints().y));
        }

        SetProducedItems(0);
    }

    protected virtual void Update()
    {
        if (GetProduces() && nextProduceTime < Time.time) Produce();
    }

    protected virtual void Produce()
    {
        SetProducedItems(GetProducedItems() + (int)Random.Range(GetProduceQuantity().x, GetProduceQuantity().y));

        SetNextProduceTime(GetProduceTime() * (GetStockQuantity() > 0 ? 0.75f : 1f));

        if (GetStockQuantity() > 0)
        {
            SetStockQuantity(GetStockQuantity() - 1);
            UIController.Instance.GetBuildingInspectMenu().UpdateDisplay();
        }
    }

    protected virtual void OpenMenu()
    {
        switch (GetID())
        {
            case "tree_1":
                UIController.Instance.GetPopup().Setup("Cut down tree", 
                    "Are you sure you want to cut down this tree, as deforestation contributes 4.8 billion tonnes of carbon dioxide per year.",
                    UIController.Instance.GetPointsIcon(), "Takes 5 eco-points", () => {
                        GameController.Instance.GivePoints(-5);
                        SoundController.PlaySound("sell_buy_item");
                        GameController.Instance.RemoveBuilding(this);
                    }, () => { }, () => { UIController.Instance.GetPopup().check = (GameController.Instance.GetPoints() >= 5); });
                break;
            case "rock_1":
                UIController.Instance.GetPopup().Setup("Smash boulder",
                    "Are you sure you want to smash this boulder, as it could be home to local widelife and plantlife.",
                    UIController.Instance.GetPointsIcon(), "Takes 12 eco-points", () => {
                        GameController.Instance.GivePoints(-12);
                        SoundController.PlaySound("sell_buy_item");
                        GameController.Instance.RemoveBuilding(this);
                    }, () => { }, () => { UIController.Instance.GetPopup().check = (GameController.Instance.GetPoints() >= 12); });
                break;
            default:
                UIController.Instance.GetBuildingInspectMenu().InspectBuilding(this);

                break;
        }
    }

    public void CalculateIdledProduces(int elapsedTime, float produceIn)
    {
        if (!GetProduces()) return;

        float offlineProductionTime = GetProduceTime() + (GetProduceTime() * OFFLINE_PENALTY_PERCENT); // Increased production time when offline
        float producedTime = elapsedTime + (offlineProductionTime - produceIn);
        int timesProduced = (int)(producedTime / offlineProductionTime);
        float nextTime = producedTime - (timesProduced * offlineProductionTime);

        if (timesProduced > 0)
        {
            for (int i = 0; i < timesProduced; i++) Produce();
        }

        SetNextProduceTime(nextTime);
    }

    public virtual BuildingSave GetSave()
    {
        BuildingSave buildingSave = new BuildingSave(GetID(), transform.position);

        buildingSave.producedItems = GetProducedItems();
        buildingSave.nextProduceTime = GetNextProduceTime() - Time.time;
        buildingSave.stockQuantity = stockQuantity;
        buildingSave.buildingType = GetType();

        return buildingSave;
    }

    public virtual void GetLoad(BuildingSave save)
    {
        SetProducedItems(save.producedItems);
        SetStockQuantity(save.stockQuantity);
        CalculateIdledProduces(GameController.Instance.GetTimeSinceSave(), save.nextProduceTime);
    }

    public void OnDestroy()
    {
        Building.RemoveColliders(transform.position, GetSize());
    }


    #endregion
    #region Static Helpers

    public static Vector2 MAP_BOUNDS_MIN = new Vector2(-19,-19);
    public static Vector2 MAP_BOUNDS_MAX = new Vector2(21,21);
    public static bool[,] colliders = new bool[50,50];

    public static void AddColliders(Vector2 position, Vector2 size) { AddColliders(new Vector2Int((int)position.x, (int)position.y), new Vector2Int((int)size.x, (int)size.y)); }
    public static void AddColliders(Vector2Int position, Vector2Int size)
    {
        for (int x = 0; x < size.x; x++)
        {
            for (int y = 0; y < size.y; y++)
            {
                colliders[20 + position.x + x, 20 + position.y + y] = true;
            }
        }
    }

    public static void RemoveColliders(Vector2 position, Vector2 size) { RemoveColliders(new Vector2Int((int)position.x, (int)position.y), new Vector2Int((int)size.x, (int)size.y)); }
    public static void RemoveColliders(Vector2Int position, Vector2Int size)
    {
        for (int x = 0; x < size.x; x++)
        {
            for (int y = 0; y < size.y; y++)
            {
                colliders[20 + position.x + x, 20 + position.y + y] = false;
            }
        }
    }

    public static bool IsBlocked(Vector2 position, Vector2 size) { return IsBlocked(new Vector2Int((int)position.x, (int)position.y), new Vector2Int((int)size.x, (int)size.y)); }
    public static bool IsBlocked(Vector2Int position, Vector2Int size)
    {
        if (position.x < MAP_BOUNDS_MIN.x || position.x + size.x - 1 > MAP_BOUNDS_MAX.x) return true;
        if (position.y < MAP_BOUNDS_MIN.y || position.y + size.y - 1 > MAP_BOUNDS_MAX.y) return true;


        if (size.x == 1 && size.y == 1)
        {
            return colliders[20 + position.x - 1, 20 + position.y - 1];
        }
        else
        {
            for (int x = 0; x < size.x; x++)
            {
                for (int y = 0; y < size.y; y++)
                {
                    if (colliders[20 + position.x + x - 1, 20 + position.y + y - 1]) return true;
                }
            }
        }

        return false;
    }

    #endregion

}
