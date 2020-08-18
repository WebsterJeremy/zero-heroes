using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Mainmenu : MenuBase
{
    #region AccessVariables


    [Header("Buttons")]
    [SerializeField] private Button buttonPlay;


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

    protected override void AddButtonListeners()
    {
        buttonPlay.onClick.AddListener(() =>
        {
            SoundController.PlaySound("button");

            buttonPlay.interactable = false;
            Close();
        });
    }

    protected override void Open()
    {
        rectMenu.gameObject.SetActive(true);
    }

    protected override void Close()
    {
        rectMenu.gameObject.SetActive(false);

        GameController.Instance.StartGame();
    }


    #endregion
}
