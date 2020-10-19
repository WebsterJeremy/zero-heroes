﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Entity : MonoBehaviour
{
    #region AccessVariables

    public static EntityAttributes[] entityAttributesList;

    [Header("Entity")]
    [SerializeField] protected string id;
    [SerializeField] private Vector2 position;


    #endregion
    #region PrivateVariables

    [System.NonSerialized] protected GameObject obj;
    [System.NonSerialized] private EntityAttributes entityAttributes;

    #endregion
    #region Initlization

    protected virtual void Start()
    {

    }

    public static void LoadEntityAttributes()
    {
        entityAttributesList = Resources.LoadAll<EntityAttributes>("Entities/");
    }

    public static EntityAttributes FindEntityAttributes(string entity_id)
    {
        if (entityAttributesList == null) LoadEntityAttributes();

        foreach (EntityAttributes attr in entityAttributesList)
        {
            if (attr.GetID().Equals(entity_id)) return attr;
        }

        return null;
    }

    #endregion
    #region Getters and Setters

    public GameObject GetObject()
    {
        return obj;
    }

    protected virtual EntityAttributes GetEntityAttributes()
    {
        if (entityAttributes == null) entityAttributes = FindEntityAttributes(id);

        return entityAttributes;
    }

    public string GetTitle()
    {
        return GetEntityAttributes().GetTitle();
    }

    public Sprite GetIcon()
    {
        return GetEntityAttributes().GetIcon();
    }

    public string GetDescription()
    {
        return GetEntityAttributes().GetDescription();
    }

    public Vector2 GetSize()
    {
        return GetEntityAttributes().GetSize();
    }

    public GameObject GetPrefab()
    {
        return GetEntityAttributes().GetPrefab();
    }

    public string GetID()
    {
        return GetEntityAttributes().GetID();
    }

    #endregion
    #region Core

    public virtual void Spawn(Vector3 spawnPoint)
    {
        obj = GameObject.Instantiate(GetPrefab());
        obj.transform.position = spawnPoint;
        obj.transform.SetParent(GameController.Instance.entitiesContainer, false);

        GameController.Instance.AddEntity(this);
    }

    #endregion
}
