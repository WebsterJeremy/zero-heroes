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
    [SerializeField] private int producedItems = 0;
    [SerializeField] private float nextProduceTime = 999;
    [SerializeField] private bool restocked = true;
    [SerializeField] private float nextRestockTime = 999; // When Time.time is greater then ( lastRestockTime + GetRestockTime() ), then restock [GetRestockTime() is an attribute of the building]


    /*
    private float realTime; //placeholder so that timespan can be changed 
    [SerializeField]DateTime oldDate; //start time away interval0
    DateTime currentDate;  //
    */

    #endregion
    #region PrivateVariables


    [System.NonSerialized] private BuildingAttributes buildingAttributes;
    [System.NonSerialized] private Notify notify;


    #endregion
    #region Initlization

    protected override void Start()
    {
        base.Start();

        nextProduceTime = Time.time + GetProduceTime();
        nextRestockTime = Time.time + GetRestockTime();
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

    public float GetNextProduceTime()
    {
        return nextProduceTime;
    }

    public void SetNextProduceTime(float nextProduceTime)
    {
        this.nextProduceTime = nextProduceTime;
    }

    public bool GetRestocked()
    {
        return restocked;
    }

    public void SetRestocked(bool restocked)
    {
        this.restocked = restocked;
    }

    public float GetNextRestockTime()
    {
        return nextRestockTime;
    }

    public void SetNextRestockTime(float nextRestockTime)
    {
        this.nextRestockTime = nextRestockTime;
    }

    #endregion
    #region Core

    public void CopyTo(Building ent)
    {
        ent.SetProducedItems(ent.GetProducedItems());
        ent.SetNextProduceTime(ent.GetNextProduceTime());
        ent.SetRestocked(ent.GetRestocked());
        ent.SetNextRestockTime(ent.GetNextRestockTime());
    }

    private void OnMouseDown()
    {
        if (EventSystem.current.currentSelectedGameObject != null) return;

        if (GetProduces())
        {
            if (producedItems < 1 || GetProducedItem() == null || GetProducedItem().Equals(string.Empty))
            {
                Debug.Log("No produced items opening menu for " + GetTitle() +" : Next produce in "+ (nextProduceTime - Time.time) + " seconds");
            }
            else
            {
                GameController.Instance.GetInventory().GiveItem(GetProducedItem(), producedItems, -1); // Slot of -1, means it will find a new empty slot in the inventory instead
                SoundController.PlaySound("inventory_pickup");

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
        if (GetProduces() && nextProduceTime < Time.time) Produce();
    }

    private void Produce()
    {
        SetProducedItems(GetProducedItems() + (int)Random.Range(GetProduceQuantity().x, GetProduceQuantity().y));

        nextProduceTime = Time.time + GetProduceTime();
    }

    public void CalculateIdledProduces(int elapsedTime, float produceIn)
    {
        if (!GetProduces()) return;

        float offlineProductionTime = GetProduceTime() + (GetProduceTime() * OFFLINE_PENALTY_PERCENT); // Increased production time when offline
        float producedTime = elapsedTime + (offlineProductionTime - produceIn);
        int timesProduced = (int)(producedTime / offlineProductionTime);
        float nextTime = producedTime - (timesProduced * offlineProductionTime);

        Debug.Log("Time elapsed: "+ elapsedTime +" Base Produce In: "+ produceIn +", Produce time: "+ producedTime +", Produced "+ timesProduced + " Times, New next time: "+ nextTime);

        if (timesProduced > 0)
        {
            for (int i = 0; i < timesProduced; i++) Produce();
        }

        nextProduceTime = Time.time + nextTime;
    }

    public void OnDestroy()
    {
        Building.RemoveColliders(transform.position, GetSize());
    }

    #endregion
    #region Static Helpers

    public static Vector2 MAP_BOUNDS_MIN = new Vector2(-18,-18);
    public static Vector2 MAP_BOUNDS_MAX = new Vector2(20,20);
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
        if (position.x < MAP_BOUNDS_MIN.x || position.x > MAP_BOUNDS_MAX.x) return true;
        if (position.y < MAP_BOUNDS_MIN.y || position.y > MAP_BOUNDS_MAX.y) return true;


        if (size.x == 1 && size.y == 1)
        {
            return colliders[20 + position.x, 20 + position.y];
        }
        else
        {
            for (int x = 0; x < size.x; x++)
            {
                for (int y = 0; y < size.y; y++)
                {
                    if (colliders[20 + position.x + x, 20 + position.y + y]) return true;
                }
            }
        }

        return false;
    }

    #endregion

}
