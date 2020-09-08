using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZoneTrigger : MonoBehaviour
{
    #region AccessVariables


    [Header("Trigger")]
    [SerializeField] private Zone zone;
    [SerializeField] private string entryPoint = "EntryPoint_1";


    #endregion
    #region PrivateVariables

    private Collider2D collider;

    #endregion
    #region Initlization


    void Start()
    {
        collider = GetComponent<Collider2D>();
    }


    #endregion
    #region Main


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            GameController.Instance.EnterZone(zone, entryPoint);
        }
    }


    #endregion
}
