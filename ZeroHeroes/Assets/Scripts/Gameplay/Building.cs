using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Building : Entity
{
    #region AccessVariables


    [Header("Entity")]
    public GameObject obj;


    #endregion
    #region PrivateVariables



    #endregion
    #region Initlization

    protected override void Start()
    {
        base.Start();

        title = "Building";
    }

    #endregion
    #region Core

    public override void Spawn(Vector3 spawnPoint)
    {
        obj = GameObject.Instantiate(prefab);
        obj.transform.position = spawnPoint;
        obj.transform.SetParent(GameController.Instance.buildingsContainer, false);

        GameController.Instance.AddBuilding(this);
    }

    #endregion
}
