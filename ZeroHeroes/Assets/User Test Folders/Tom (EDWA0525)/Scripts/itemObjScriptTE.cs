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


public abstract class itemObjScriptTE : ScriptableObject
{
    public int Id;
    public Sprite uiDisp;
    public itemType type;
    [TextArea(20, 20)]
    public string desc;


    public invItemTE CreateItem()
    {
        invItemTE newItem = new invItemTE(this);
        return newItem;
    }
}

[System.Serializable]

public class invItemTE
{
    public string Name;
    public int Id;
    public invItemTE(itemObjScriptTE item)
    {
        Name = item.name;
        Id = item.Id;
    }
}
