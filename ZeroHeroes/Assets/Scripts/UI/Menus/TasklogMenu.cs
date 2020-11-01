using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TasklogMenu : MenuBase
{
    #region AccessVariables


    [Header("Buttons")]
    [SerializeField] private Button buttonClose;

    [Header("Functionality")]
    [SerializeField] private TextMeshProUGUI textTitle;
    [SerializeField] private TextMeshProUGUI textNpc;
    [SerializeField] private TextMeshProUGUI textDescription;
    [SerializeField] private Image imgAvatar;
    [SerializeField] private GameObject prefabTaskElement;
    [SerializeField] private GameObject prefabObjectiveElement;
    [SerializeField] private GameObject prefabRewardElement;
    [SerializeField] private RectTransform rectContent;
    [SerializeField] private RectTransform rectObjectives;
    [SerializeField] private RectTransform rectReward;

    [Header("Setup")]
    [SerializeField] private TextMeshProUGUI textChapter;

    #endregion
    #region PrivateVariables

    List<TaskCardElement> cards = new List<TaskCardElement>();
    List<ObjectiveCardElement> objectiveCards = new List<ObjectiveCardElement>();
    List<GameObject> rewardCards = new List<GameObject>();
    TaskCardElement selectedCard;

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

        rectMenu.gameObject.SetActive(true);
        UIController.Instance.EnableBlur();

        Populate();
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
            selectedCard.GetComponent<Image>().color = new Color32(252, 247, 222, 255);
        }

        if (cards != null && cards.Count > 0)
        {
            for (int i = 0; i < cards.Count; i++)
            {
                Destroy(cards[i].gameObject);
            }

            cards.Clear();
        }

        if (GameController.Instance.GetCurrentChapter() == null) return;
        Task[] tasks = GameController.Instance.GetCurrentChapter().GetTasks();

        if (tasks.Length > 0)
        {
            for (int i = 0; i < tasks.Length; i++)
            {
                GameObject element = Instantiate(prefabTaskElement);
                element.transform.SetParent(rectContent);

                TaskCardElement card = element.GetComponent<TaskCardElement>();

                card.Setup(tasks[i]);
                cards.Add(card);

                if (selectedCard == null || GameController.Instance.GetChapters()[i].GetTitle() == oldSelected)
                {
                    Select(card);
                }
            }
        }
    }

    public void Select(TaskCardElement select)
    {
        if (select == null) return;
        if (selectedCard != null) selectedCard.GetComponent<Image>().color = new Color32(252, 247, 222, 255);

        selectedCard = select;

        textTitle.text = select.task.GetTitle();
        textDescription.text = select.task.GetDescription().ToString();
        textNpc.text = select.task.GetNpc().GetTitle();
        imgAvatar.sprite = select.task.GetNpc().GetIcon();

        textChapter.text = "Chapter " + GameController.Instance.GetChapterStat();

        selectedCard.GetComponent<Image>().color = new Color32(255, 255, 232, 255);

        select.task.CheckObjectives();

        PopulateObjectives();
        PopulateRewards();
    }

    public void PopulateObjectives()
    {
        if (objectiveCards != null && objectiveCards.Count > 0)
        {
            for (int i = 0; i < objectiveCards.Count; i++)
            {
                if (objectiveCards[i] != null && objectiveCards[i].gameObject != null) Destroy(objectiveCards[i].gameObject);
            }

            objectiveCards.Clear();
        }

        TaskAttributes.Objective[] objectives = selectedCard.task.GetObjectives();

        if (objectives.Length > 0)
        {
            for (int i = 0; i < objectives.Length; i++)
            {
                GameObject element = Instantiate(prefabObjectiveElement);
                element.transform.SetParent(rectObjectives);

                ObjectiveCardElement card = element.GetComponent<ObjectiveCardElement>();

                card.Setup(selectedCard.task, objectives[i]);
                objectiveCards.Add(card);
            }
        }
    }

    public void PopulateRewards()
    {
        if (rewardCards != null && rewardCards.Count > 0)
        {
            for (int i = 0; i < rewardCards.Count; i++)
            {
                if (rewardCards[i] != null) Destroy(rewardCards[i]);
            }

            rewardCards.Clear();
        }

        TaskAttributes.Reward[] rewards = selectedCard.task.GetRewards();

        if (rewards.Length > 0)
        {
            for (int i = 0; i < rewards.Length; i++)
            {
                GameObject element = Instantiate(prefabRewardElement);
                element.transform.SetParent(rectReward);

                Sprite icon = null;

                switch (rewards[i].rewardType)
                {
                    case TaskAttributes.RewardType.ITEM:
                        ItemAttributes attributes = Item.FindItemAttributes(rewards[i].data);
                        if (attributes == null) return;

                        icon = attributes.GetIcon();

                        break;
                    case TaskAttributes.RewardType.MONEY:
                        icon = UIController.Instance.GetMoneyIcon();

                        break;
                    default:
                        icon = UIController.Instance.GetPointsIcon();

                        break;
                }

                element.transform.GetComponentInChildren<Image>().sprite = icon;
                element.transform.GetComponentInChildren<TextMeshProUGUI>().text = "x" + rewards[i].quantity;

                rewardCards.Add(element);
            }
        }
    }

    #endregion
}
