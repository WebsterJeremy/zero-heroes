using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Entity
{
    #region AccessVariables

    #endregion
    #region PrivateVariables

    #endregion
    #region Initlization

    private static Player instance;
    public static Player Instance // Assign Singlton
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<Player>();
                if (Instance == null)
                {
                    var instanceContainer = new GameObject("Player");
                    instance = instanceContainer.AddComponent<Player>();
                }
            }
            return instance;
        }
    }

    protected override void Start()
    {
        this.id = "player";

        base.Start();
    }

    #endregion
    #region Core

    public override void Spawn(Vector3 spawnPoint)
    {
        base.Spawn(spawnPoint);
    }

    #endregion
}
