using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum itemType
{
    Building,
    Environment,
    Consumable,
    Animal,
    Default
}

public abstract class itemObjScript : ScriptableObject
{
    public GameObject preFab;
    public itemType type;
    [TextArea(20, 20)]
    public string desc;

}
