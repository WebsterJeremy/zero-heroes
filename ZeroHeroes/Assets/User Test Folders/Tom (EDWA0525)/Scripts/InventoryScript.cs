using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New INventory", menuName = "Inventory System/Inventory")]

public class InventoryScript : ScriptableObject
{
    public List<InvSlot> Cont = new List<InvSlot>();

    public void AddItem(itemObjScript _item, int _val)
    {
        bool hasItem = false;
        for (int i = 0; i < Cont.Count; i++)
        {
            if(Cont[i].item == _item)
            {
                Cont[i].AddVal(_val);
                hasItem = true;
                break;
            }
        }

        if (!hasItem)
        {
            Cont.Add(new InvSlot(_item, _val));
        }
    }



}

[System.Serializable]

public class InvSlot
{
    public itemObjScript item;
    public int val;
    public InvSlot(itemObjScript _item, int _val)
    {
        item = _item;
        val = _val;
    }

    public void AddVal(int value)
    {
        val += value;
    }
}
