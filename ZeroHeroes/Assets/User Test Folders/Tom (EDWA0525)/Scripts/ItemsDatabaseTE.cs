using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Item Database", menuName = "Inventory System/Items/Database")]

public class ItemsDatabaseTE : ScriptableObject, ISerializationCallbackReceiver
{
    public itemObjScriptTE[] Items;

    public Dictionary<int, itemObjScriptTE> GetItem = new Dictionary<int, itemObjScriptTE>();

    public void OnAfterDeserialize()
    {
        for (int i = 0; i < Items.Length; i++)
        {
            Items[i].Id = i;
            GetItem.Add(i, Items[i]);
        }
    }

    public void OnBeforeSerialize()
    {
        GetItem = new Dictionary<int, itemObjScriptTE>();
    }
}
