using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class MainMenu : MenuBase
{
    #region AccessVariables


    [Header("Buttons")]
    [SerializeField] private Button buttonPlay;
    [SerializeField] private Button buttonOptions;
    [SerializeField] private Button buttonExit;
    [SerializeField] private Button buttonHelp;

    [Header("Background Scroller")]
    [SerializeField] private float scrollerSpeed = 1f;
    [SerializeField] private GameObject imageBackground;


    #endregion
    #region PrivateVariables

    private GameObject[] backgrounds;
    private Vector2 offsetMin, offsetMax;

    #endregion
    #region Initlization


    protected override void Start()
    {
        base.Start();

        backgrounds = new GameObject[3];
        backgrounds[0] = imageBackground;
        for (int i = 1;i < 3;i++)
        {
            backgrounds[i] = GameObject.Instantiate(imageBackground);
            backgrounds[i].name = "img_background_" + i;
            backgrounds[i].transform.SetParent(imageBackground.transform.parent);
            backgrounds[i].GetComponent<RectTransform>().offsetMin = new Vector2(i * Screen.width-2, 0f);
            backgrounds[i].GetComponent<RectTransform>().offsetMax = new Vector2(i * Screen.width-2, 0f);

            if (i % 2 != 0) backgrounds[i].transform.localRotation = Quaternion.Euler(0, 180, 0);
        }
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
        buttonOptions.onClick.AddListener(() =>
        {
            SoundController.PlaySound("button");

            OpenSettings();
        });
        buttonExit.onClick.AddListener(() =>
        {
            SoundController.PlaySound("button");

#if UNITY_EDITOR
            Debug.Log("Quitting Game");

            EditorApplication.ExecuteMenuItem("Edit/Play");
#else
            Application.Quit();
#endif
        });
    }
    
    protected override IEnumerator _Open()
    {
        if (IsOpened()) yield break;

        gameObject.SetActive(true);
    }

    protected override IEnumerator _Close()
    {
        if (!IsOpened()) yield break;

        gameObject.SetActive(false);

        GameController.Instance.StartGame();
        buttonPlay.interactable = true;
    }

    public override void TransitionOut()
    {
        EffectController.TweenAnchor(rectMenu, new Vector2(-(Screen.width/2 + rectMenu.rect.width/2 + 40), 0), 1f, false, () => {
            rectMenu.gameObject.SetActive(true);
        });
    }

    public override void TransitionIn()
    {
        rectMenu.gameObject.SetActive(true);
        EffectController.TweenAnchor(rectMenu, new Vector2(0, 0), 1f, false, () => { });
    }

    private void Update()
    {
        AnimateBackgroundScroller();
    }

    private void AnimateBackgroundScroller()
    {
        for (int i = 0; i < backgrounds.Length; i++)
        {
            offsetMin = backgrounds[i].GetComponent<RectTransform>().offsetMin;
            offsetMax = backgrounds[i].GetComponent<RectTransform>().offsetMax;

            offsetMin.x -= scrollerSpeed;
            offsetMax.x -= scrollerSpeed;

            backgrounds[i].GetComponent<RectTransform>().offsetMin = offsetMin;
            backgrounds[i].GetComponent<RectTransform>().offsetMax = offsetMax;

            if (offsetMin.x <= -Screen.width)
            {
                backgrounds[i].GetComponent<RectTransform>().offsetMin = new Vector2(2 * Screen.width-2, 0f);
                backgrounds[i].GetComponent<RectTransform>().offsetMax = new Vector2(2 * Screen.width-2, 0f);

                backgrounds[i].transform.localRotation = Quaternion.Euler(0, backgrounds[i].transform.localRotation.eulerAngles.y == 180 ? 0 : 180, 0);
            }
        }
    }

    private void OpenSettings()
    {
        TransitionOut();

        UIController.Instance.GetSettingsMenu().OpenFrom(this);
    }


    #endregion
}
