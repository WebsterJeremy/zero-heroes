﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "new Environment Obj", menuName = "Inventory System/ItemObjs/Environment")]

public class environmentObj : itemObjScriptTE
{
    public int natureVal;
    public void Awake()
    {
        type = itemType.Environment;
    }
}
