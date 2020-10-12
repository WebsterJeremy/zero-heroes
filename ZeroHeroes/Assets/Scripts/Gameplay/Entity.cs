using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Entity : MonoBehaviour
{
    #region AccessVariables


    [Header("Entity")]
    protected string title;
    protected GameObject prefab;


    #endregion
    #region PrivateVariables

    protected GameObject obj;

    #endregion
    #region Initlization

    protected virtual void Start()
    {

    }

    #endregion
    #region Getters and Setters

    public GameObject GetObject()
    {
        return obj;
    }

    #endregion
    #region Core

    public virtual void Spawn(Vector3 spawnPoint)
    {
        obj = GameObject.Instantiate(prefab);
        obj.transform.position = spawnPoint;
        obj.transform.SetParent(GameController.Instance.entitiesContainer, false);

        GameController.Instance.AddEntity(this);
    }

    #endregion
}
