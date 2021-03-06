﻿using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SettingsMenu : MenuBase
{
    #region AccessVariables


    [Header("Buttons")]
    [SerializeField] private Button buttonClose;
    [SerializeField] private Button buttonSound;
    [SerializeField] private Button buttonMusic;
    [SerializeField] private Button buttonJoystick;
    [SerializeField] private Button buttonHints;
    [SerializeField] private Button buttonHelp;


    #endregion
    #region PrivateVariables


    private MenuBase backMenu;


    #endregion
    #region Initlization


    protected override void Start()
    {
        base.Start();
        rectMenu.anchoredPosition = new Vector2((Screen.width / 2 + rectMenu.rect.width / 2 + 20), 0);
    }


    #endregion
    #region Core


    protected override void AddButtonListeners()
    {
        buttonClose.onClick.AddListener(() =>
        {
            SoundController.PlaySound("button");

            Close();
        });
    }

    public void OpenFrom(MenuBase menu)
    {
        backMenu = menu;
        base.Open();
    }

    protected override IEnumerator _Open()
    {
        if (IsOpened()) yield break;

        rectMenu.gameObject.SetActive(true);
        EffectController.TweenAnchor(rectMenu, new Vector2(0, 0), 1f, false, () => { });
    }

    protected override IEnumerator _Close()
    {
        if (!IsOpened()) yield break;

        backMenu.TransitionIn();

        EffectController.TweenAnchor(rectMenu, new Vector2((Screen.width / 2 + rectMenu.rect.width / 2 + 80), 0), 1f, false, () => {});

        yield return new WaitForSeconds(0.3f);

        rectMenu.gameObject.SetActive(false);
    }


    #endregion
}
