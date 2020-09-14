using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PauseMenu : MenuBase
{
    #region AccessVariables


    [Header("Buttons")]
    [SerializeField] private Button buttonMusic;
    [SerializeField] private Button buttonSounds;
    [SerializeField] private Button buttonResume;
    [SerializeField] private Button buttonSettings;
    [SerializeField] private Button buttonExit;


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
        buttonMusic.onClick.AddListener(() =>
        {
            SoundController.PlaySound("button");

            SoundController.SetMusicMuted(!SoundController.MUTED_MUSIC);
        });
        buttonSounds.onClick.AddListener(() =>
        {
            SoundController.PlaySound("button");

            SoundController.SetSoundsMuted(!SoundController.MUTED_SOUNDS);
        });
        buttonResume.onClick.AddListener(() =>
        {
            SoundController.PlaySound("button");

            buttonResume.interactable = false;
            Close();
        });
        buttonSettings.onClick.AddListener(() =>
        {
            SoundController.PlaySound("button");

            TransitionOut();
            UIController.Instance.GetSettingsMenu().OpenFrom(this);
        });
        buttonExit.onClick.AddListener(() =>
        {
            SoundController.PlaySound("button");

            GameController.Instance.StopGame();
        });
    }

    protected override IEnumerator _Open()
    {
        if (IsOpened()) yield break;

        GameController.Instance.PauseGame();
        UIController.Instance.EnableBlur();
    }

    protected override IEnumerator _Close()
    {
        if (!IsOpened()) yield break;

        gameObject.SetActive(false);

        UIController.Instance.DisableBlur();
        GameController.Instance.UnpauseGame();

        buttonResume.interactable = true;
    }

    public override void TransitionOut()
    {
        EffectController.TweenAnchor(rectMenu, new Vector2(-(Screen.width / 2 + rectMenu.rect.width / 2 + 40), 0), 1f, false, () => {
            rectMenu.gameObject.SetActive(true);
        });
    }

    public override void TransitionIn()
    {
        rectMenu.gameObject.SetActive(true);
        EffectController.TweenAnchor(rectMenu, new Vector2(0, 0), 1f, false, () => { });
    }

    #endregion
}
