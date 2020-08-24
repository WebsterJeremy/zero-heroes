using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "new Building Obj", menuName = "Inventory System/ItemObjs/Building")]

public class buildingObj : itemObjScript
{
    // Start is called before the first frame update
    public void Awake()
    {
        type = itemType.Building;
    }
}
