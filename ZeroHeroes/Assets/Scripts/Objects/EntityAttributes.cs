using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Entity", menuName = "Gameplay/Entity")]
public class EntityAttributes : ScriptableObject
{
    #region AccessVariables


    [Header("Entity")]
    [SerializeField] private string title = "New Entity";
    [SerializeField] private string id = "entity";
    [SerializeField] private Sprite icon;
    [TextArea(20, 20)]
    [SerializeField] private string description = "None";
    [SerializeField] private Vector2 size = new Vector2(2, 2); // Pivot top left
    [SerializeField] private GameObject prefab;


    #endregion
    #region PrivateVariables


    #endregion
    #region Initlization


    protected virtual void Awake()
    {

    }


    #endregion
    #region Getters and Setters

    public string GetTitle()
    {
        return title;
    }

    public Sprite GetIcon()
    {
        return icon;
    }

    public string GetDescription()
    {
        return description;
    }

    public GameObject GetPrefab()
    {
        return prefab;
    }

    public Vector2 GetSize()
    {
        return size;
    }

    public string GetID()
    {
        return id;
    }

    #endregion
    #region Core



    #endregion
}
