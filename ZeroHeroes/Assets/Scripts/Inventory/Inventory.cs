using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    #region AccessVariables


    [Header("Inventory")]
    [SerializeField] private int slots = 25;
    [SerializeField] private string savePath = "/inventory_save";


    #endregion
    #region PrivateVariables

    private Dictionary<int, Item> items;

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
        items = new Dictionary<int, Item>();
    }


    #endregion
    #region Getters and Setters

    public Item GetItem(int slot)
    {
        return items.ContainsKey(slot) ? items[slot] : null;
    }

    public List<Item> FindItem(string id)
    {
        List<Item> found = new List<Item>();

        if (items == null || items.Count < 1) return found;

        foreach (Item itemFound in items.Values)
        {
            if (itemFound != null && itemFound.GetID().Equals(id)) found.Add(itemFound);
        }

        return found;
    }

    public int FindEmptySlot()
    {
        for (int i = 0;i < slots;i++)
        {
            if (GetItem(i) == null) return i;
        }

        return -1;
    }

    public bool GiveItem(Item item, int quantity, int slot)
    {
        List<Item> sameItem = FindItem(item.GetID());

        if (items == null) items = new Dictionary<int, Item>();

        if (sameItem.Count < 1) // Doesn't already have the item
        {
            if (items.Count < slots)
            {
                item.SetQuantity(quantity);

                if (slot == -1) slot = FindEmptySlot();

                items[slot] = item;
                item.SetSlot(slot);

                UIController.Instance.GetInventoryMenu().UpdateDisplay();

                return true;
            }
        }
        else // Has the item
        {
            int quantityLeft = quantity;

            foreach (Item foundItem in sameItem)
            {
                if (foundItem.GetQuantity() < foundItem.GetQuantityMax())
                {
                    if (foundItem.GetQuantity() + quantityLeft <= foundItem.GetQuantityMax())
                    {
                        foundItem.GiveQuantity(quantityLeft);

                        UIController.Instance.GetInventoryMenu().UpdateDisplay();

                        return true;
                    }
                    else
                    {
                        foundItem.SetQuantity(foundItem.GetQuantityMax());
                        quantityLeft -= foundItem.GetQuantityMax();
                    }
                }
            }

            if (quantityLeft > 0)
            {
                item.SetQuantity(quantityLeft);

                slot = FindEmptySlot();

                items[slot] = item;
                item.SetSlot(slot);

                UIController.Instance.GetInventoryMenu().UpdateDisplay();

                return true;
            }
        }

        return false; // Return false if item couldn't be added because not enough slots left (Inventory Full)
    }

    public bool GiveItem(string item_id, int quantity, int slot)
    {
        return GiveItem(new Item(item_id, 0), quantity, slot);
    }

    public bool MoveItem(int oldSlot, int newSlot)
    {
        if (GetItem(oldSlot) != null)
        {
            if (GetItem(newSlot) == null)
            {
                items.Add(newSlot, items[oldSlot]);
                items.Remove(oldSlot);

                GetItem(newSlot).SetSlot(newSlot);

                UIController.Instance.GetInventoryMenu().UpdateDisplay();

                return true;
            }
            else // Item already in slot, so swap or merge instead
            {
                if (GetItem(oldSlot).GetID().Equals(GetItem(newSlot).GetID()))
                {
                    return MergeItem(oldSlot, newSlot);
                }
                else
                {
                    return SwapItem(oldSlot, newSlot);
                }
            }
        }

        return false;
    }

    public bool SwapItem(int oldSlot, int newSlot)
    {
        if (GetItem(oldSlot) != null && GetItem(newSlot) != null)
        {
            Item tempItem = GetItem(newSlot);
            items.Remove(newSlot);

            items.Add(newSlot, GetItem(oldSlot));
            items.Remove(oldSlot);

            items.Add(oldSlot, tempItem);

            GetItem(newSlot).SetSlot(newSlot);
            GetItem(oldSlot).SetSlot(oldSlot);

            UIController.Instance.GetInventoryMenu().UpdateDisplay();

            return true;
        }

        return false;
    }

    public bool MergeItem(int oldSlot, int newSlot)
    {

        return false;
    }

    public int TakeItem(int slot, int quantity)
    {
        if (GetItem(slot) != null)
        {
            if (quantity <= items[slot].GetQuantity())
            {
                items[slot].SetQuantity(items[slot].GetQuantity() - quantity);

                if (items[slot].GetQuantity() <= 0)
                {
                    items[slot] = null;
                }

                UIController.Instance.GetInventoryMenu().UpdateDisplay();

                return 0;
            }
            else
            {
                items[slot] = null;

                UIController.Instance.GetInventoryMenu().UpdateDisplay();

                return quantity - items[slot].GetQuantity();
            }
        }

        return -1;
    }

    public bool TakeItem(string id, int quantity)
    {
        

        return false;
    }

    public bool RemoveItem(int slot)
    {
        items.Remove(slot);
        UIController.Instance.GetInventoryMenu().UpdateDisplay();

        return (!items.ContainsKey(slot));
    }

    public Item[] GetItems()
    {
        List<Item> temp = new List<Item>();

        if (items == null || items.Count < 1) return temp.ToArray();

        foreach (Item i in items.Values)
        {
            if (i != null) temp.Add(i);
        }

        return temp.ToArray();
    }

    public Dictionary<int, Item> GetItemsForSave()
    {
        return items;
    }

    public void SetItemsFromSave(Dictionary<int, Item> savedItems)
    {
        items = savedItems;

        UIController.Instance.GetInventoryMenu().UpdateDisplay();
    }

    #endregion
    #region Core


    #endregion
}
