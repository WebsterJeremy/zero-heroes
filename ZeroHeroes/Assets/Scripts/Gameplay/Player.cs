using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Entity
{
    #region AccessVariables


    [Header("Entity")]
    public GameObject obj;


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
        base.Start();

        title = "Player";
    }

    #endregion
    #region Core

    public override void Spawn(Vector3 spawnPoint)
    {
        prefab = GameController.Instance.playerPrefab;

        base.Spawn(spawnPoint);
    }

    #endregion
}
