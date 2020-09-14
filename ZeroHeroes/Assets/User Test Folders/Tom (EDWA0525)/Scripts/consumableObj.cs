using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "new Consumable Obj", menuName = "Inventory System/ItemObjs/Consumable")]

public class consumableObj : itemObjScriptTE
{
    public void Awake()
    {
        type = itemType.Consumable;
    }
}
