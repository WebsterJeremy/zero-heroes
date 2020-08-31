using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Object", menuName = "World/Object")]
public class ObjectData : ScriptableObject
{

    [Header("Display")]
    public string objectName = "Object";
    public Sprite sprite;
    public Color32 color = new Color32(255, 255, 255, 255);
    
    [Header("Animation")]
    public bool IsAnimatable = false;
    public Sprite[] sprites;

    [Header("Movement")]
    public bool immobile = true;

    [Header("Objective")]
    public Objective objective;

    public virtual string OnInteractionText()
    {
        return "Pickup " + objectName;
    }

    public virtual void OnInteraction()
    {
        if (objective != null)
        {
            objective.DoObjective();
        }
    }
}
