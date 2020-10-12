using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    #region AccessVariables


    [Header("Inventory")]
    [SerializeField] private int slots = 40;
    [SerializeField] private string savePath = "/inventory_save";


    #endregion
    #region PrivateVariables

    private Dictionary<string, Item> items = new Dictionary<string, Item>();

    #endregion
    #region Initlization


    private static Inventory instance;
    public static Inventory Instance // Assign Singlton
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<Inventory>();
                if (Instance == null)
                {
                    var instanceContainer = new GameObject("Inventory");
                    instance = instanceContainer.AddComponent<Inventory>();
                }
            }
            return instance;
        }
    }

    private void Start()
    {

    }


    #endregion
    #region Getters and Setters

    public Item GetItem(string id)
    {
        return items.ContainsKey(id) ? items[id] : null;
    }

    public bool GiveItem(Item item, int amount)
    {
        if (!items.ContainsKey(item.GetID()))
        {
            if (items.Count < slots)
            {
                item.SetAmount(amount);
                items.Add(item.GetID(), item);

                UIController.Instance.GetInventoryMenu().UpdateDisplay();

                return true;
            }
        }
        else
        {
            items[item.GetID()].GiveAmount(amount);

            UIController.Instance.GetInventoryMenu().UpdateDisplay();

            return true;
        }

        return false; // Return false if item couldn't be added because not enough slots left (Inventory Full)
    }

    public bool GiveItem(string item_id, int amount)
    {
        if (!items.ContainsKey(item_id))
        {
            if (items.Count < slots)
            {
                items.Add(item_id, new Item(item_id, amount));

                UIController.Instance.GetInventoryMenu().UpdateDisplay();

                return true;
            }
        }
        else
        {
            items[item_id].GiveAmount(amount);

            UIController.Instance.GetInventoryMenu().UpdateDisplay();

            return true;
        }

        return false;
    }

    public bool TakeItem(string id)
    {
        return false;
    }

    public Item[] GetItems()
    {
        if (items.Count <= 0) return null;

        Item[] copy = new Item[items.Count];
        items.Values.CopyTo(copy, 0);

        return copy;
    }
    #endregion
    #region Core


    public bool Load()
    {
        Debug.Log("Loaded Inventory");

        return false;
    }

    public bool Save()
    {
        Debug.Log("Saved Inventory");

        return false;
    }

    #endregion
}
