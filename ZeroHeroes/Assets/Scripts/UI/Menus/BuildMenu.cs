using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BuildMenu : MenuBase
{
    #region AccessVariables


    [Header("Buttons")]
    [SerializeField] private Button buttonClose;


    #endregion
    #region PrivateVariables


    #endregion
    #region Initlization


    protected override void Start()
    {
        base.Start();
    }


    #endregion
    #region Core

    public void ToggleVisibility()
    {
        if (IsOpened()) Close(); else Open();
    }

    protected override void AddButtonListeners()
    {
        buttonClose.onClick.AddListener(() =>
        {
            SoundController.PlaySound("button");

            Close();
        });
    }

    protected override IEnumerator _Open()
    {
        if (IsOpened()) yield break;

        rectMenu.gameObject.SetActive(true);
//        UIController.Instance.EnableBlur();
    }

    protected override IEnumerator _Close()
    {
        if (!IsOpened()) yield break;

        rectMenu.gameObject.SetActive(false);
    }

    public void Build(string buildingId)
    {
        GameController.Instance.SpawnBuilding(buildingId, new Vector2(0, 5));
        Debug.Log("Built it!");
    }


    #endregion
}