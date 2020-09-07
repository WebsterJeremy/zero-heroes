﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

abstract public class MenuBase : MonoBehaviour
{
    #region AccessVariables


    [Header("Display")]
    [SerializeField] protected bool opened = false;

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
    protected abstract IEnumerator _Open();
    protected abstract IEnumerator _Close();

    public void Open()
    {
        gameObject.SetActive(true);
        StartCoroutine(_Open());
        opened = true;
    }

    public void Close()
    {
        gameObject.SetActive(true);
        StartCoroutine(_Close());
        opened = false;
    }

    public void ForceClose()
    {
        opened = false;
        rectMenu.gameObject.SetActive(false);
    }


    #endregion
}
