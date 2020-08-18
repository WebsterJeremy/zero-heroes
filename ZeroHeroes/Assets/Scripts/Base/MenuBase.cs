using System.Collections;
using System.Collections.Generic;
using UnityEngine;

abstract public class MenuBase : MonoBehaviour
{
    #region AccessVariables


    [Header("Display")]
    [SerializeField] protected bool opened;

    [Header("Containers")]
    [SerializeField] protected RectTransform rectMenu;


    #endregion
    #region PrivateVariables


    private bool inTransistion = false;


    #endregion
    #region Initlization

    protected virtual void Start()
    {
        AddButtonListeners();
    }

    #endregion
    #region Getters & Setters


    public bool IsOpened()
    {
        return opened;
    }

    public bool IsInTransition()
    {
        return inTransistion;
    }


    #endregion
    #region Core


    protected virtual void AddButtonListeners() {}
    protected abstract void Open();
    protected abstract void Close();


    #endregion
}
