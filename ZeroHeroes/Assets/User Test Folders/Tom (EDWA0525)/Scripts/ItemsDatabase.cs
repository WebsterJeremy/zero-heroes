using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Item Database", menuName = "Inventory System/Items/Database")]

public class ItemsDatabase : ScriptableObject, ISerializationCallbackReceiver
{
    public itemObjScript[] items;

    public Dictionary<itemObjScript, int> GetID = new Dictionary<itemObjScript, int>();
    public Dictionary<int, itemObjScript> GetObj = new Dictionary<int, itemObjScript>();

    public void OnAfterDeserialize()
    {
        GetID = new Dictionary<itemObjScript, int>();
        GetObj = new Dictionary<int, itemObjScript>();
        for (int i = 0; i < items.Length; i++)
        {
            GetID.Add(items[i], i);
            GetObj.Add(i, items[i]);
        }
    }

    public void OnBeforeSerialize()
    {
        
    }
}
