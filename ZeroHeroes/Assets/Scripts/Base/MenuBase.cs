using System.Collections;
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


    protected bool inTransistion = false;


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
    public virtual void TransitionIn() {}
    public virtual void TransitionOut() {}

    public void Open()
    {
        if (inTransistion) return;

        gameObject.SetActive(true);
        StartCoroutine(_Open());
        opened = true;
    }

    public void Close()
    {
        if (inTransistion) return;

        gameObject.SetActive(true);
        StartCoroutine(_Close());
        opened = false;

        if (this.GetType() != typeof(SettingsMenu) && this.GetType() != typeof(BuildMenu)) UIController.Instance.DisableBlur();
    }

    public void ForceClose()
    {
        opened = false;
        rectMenu.gameObject.SetActive(false);

        if (this.GetType() != typeof(SettingsMenu) && this.GetType() != typeof(BuildMenu)) UIController.Instance.DisableBlur();
    }


    #endregion
}
