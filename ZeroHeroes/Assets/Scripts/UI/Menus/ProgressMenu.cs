using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System;

public class ProgressMenu : MenuBase
{
    #region AccessVariables


    [Header("Buttons")]
    [SerializeField] private Button buttonClose;

    [Header("Functionality")]
    [SerializeField] private TextMeshProUGUI textTitle;
    [SerializeField] private TextMeshProUGUI textDescription;
    [SerializeField] private TextMeshProUGUI textTasksCompleted;
    [SerializeField] private Image imgGraphics;
    [SerializeField] private GameObject prefabChapterElement;
    [SerializeField] private RectTransform rectContent;
    [SerializeField] private TMP_FontAsset titleFont;

    [Header("Bar")]
    [SerializeField] private RectTransform rectBar;
    [SerializeField] private RectTransform rectMarker;
    [SerializeField] private TextMeshProUGUI textPercentage;


    #endregion
    #region PrivateVariables

    List<ChapterCardElement> cards = new List<ChapterCardElement>();
    ChapterCardElement selectedCard;

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
        buttonClose.onClick.AddListener(() =>
        {
            SoundController.PlaySound("button");

            Close();
        });
    }

    protected override IEnumerator _Open()
    {
        if (IsOpened()) yield break;

        ResizeBar();
        Populate();

        rectMenu.gameObject.SetActive(true);
        UIController.Instance.EnableBlur();
    }

    protected override IEnumerator _Close()
    {
        if (!IsOpened()) yield break;

        rectMenu.gameObject.SetActive(false);
    }

    private void Populate()
    {
        string oldSelected = (selectedCard != null ? selectedCard.textTitle.text : "");
        selectedCard = null;

        if (selectedCard != null)
        {
            selectedCard.GetComponent<Image>().color = (selectedCard.chapter.GetNumber() > GameController.Instance.GetChapterStat() ? new Color32(183, 178, 149, 255) : new Color32(252, 247, 222, 255));
        }

        if (cards != null && cards.Count > 0)
        {
            for (int i = 0; i < cards.Count; i++)
            {
                Destroy(cards[i].gameObject);
            }

            cards.Clear();
        }

        if (GameController.Instance.GetChapters() != null && GameController.Instance.GetChapters().Count > 0)
        {
            for (int i = 0; i < GameController.Instance.GetChapters().Count; i++)
            {
                GameObject element = Instantiate(prefabChapterElement);
                element.transform.SetParent(rectContent);

                ChapterCardElement card = element.GetComponent<ChapterCardElement>();

                card.Setup(GameController.Instance.GetChapters()[i]);
                cards.Add(card);

                if (selectedCard == null || GameController.Instance.GetChapters()[i].GetTitle() == oldSelected)
                {
                    Select(card);
                }
            }
        }
    }

    public void Select(ChapterCardElement select)
    {
        if (select == null) return;
        if (selectedCard != null) selectedCard.GetComponent<Image>().color = (selectedCard.chapter.GetNumber() > GameController.Instance.GetChapterStat() ? new Color32(183, 178, 149, 255) : new Color32(252, 247, 222, 255));

        selectedCard = select;

        textTitle.text = "Chapter "+ select.chapter.GetNumber() +": "+ select.chapter.GetTitle();
        textDescription.text = select.chapter.GetDescription().ToString();
        textTasksCompleted.text = GameController.Instance.GetCurrentChapter().GetProgress() + " <size=70%>of</size> 5";
        imgGraphics.sprite = select.chapter.GetGraphics();

        selectedCard.GetComponent<Image>().color = new Color32(255, 255, 232, 255);
    }

    private void ResizeBar()
    {
        rectBar.GetComponent<GridLayoutGroup>().cellSize = new Vector2(rectBar.rect.width / 5, rectBar.rect.height);
        UpdatePercentage();
    }

    public void UpdatePercentage()
    {
        if (GameController.Instance.GetCurrentChapter() == null) return;

       float percent = ((GameController.Instance.GetChapterStat() - 1) * 0.2f) + ((GameController.Instance.GetCurrentChapter().GetProgress() / 5f) * 0.2f);
        
        textPercentage.text = (int) (percent * 100) + "%";
        rectMarker.anchoredPosition = new Vector2((rectBar.rect.width - 30) * percent + 70, -50);
    }

    #endregion
}
