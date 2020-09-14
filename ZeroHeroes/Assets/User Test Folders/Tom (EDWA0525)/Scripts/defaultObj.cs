using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "new Default Obj", menuName = "Inventory System/ItemObjs/Default")]

public class defaultObj : itemObjScriptTE
{
    public void Awake()
    {
        type = itemType.Default;
    }
}
