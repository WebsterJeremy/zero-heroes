using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "new Animal Obj", menuName = "Inventory System/ItemObjs/Animal")]

public class animalObj : itemObjScriptTE
{
    public void Awake()
    {
        type = itemType.Animal;
    }
}
