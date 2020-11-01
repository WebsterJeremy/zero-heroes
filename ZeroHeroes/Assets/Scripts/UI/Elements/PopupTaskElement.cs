using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class PopupTaskElement : MonoBehaviour
{
    [Header("Text")]
    public TextMeshProUGUI textMessage;

    [Header("Notice")]
    public RectTransform rectModula;
    public RectTransform rectReward;
    public GameObject prefabRewardElement;
    public float waitTime = 5f;

    private List<GameObject> rewardCards = new List<GameObject>();
    private Task task;


    public void Setup(Task task) { gameObject.SetActive(true); StartCoroutine(_Setup(task)); }
    public IEnumerator _Setup(Task task)
    {
        gameObject.SetActive(true);

        this.task = task;

        textMessage.text = "You have completed the '"+ task.GetTitle() +"' task!";

        rectModula.anchoredPosition = new Vector2(0, 300);

        PopulateRewards();

        EffectController.TweenAnchor(rectModula, new Vector2(0, -40), 2f, () => { });

        yield return new WaitForSeconds(3f);

        EffectController.TweenAnchor(rectModula, new Vector2(0, 300), 2f, () => {});

        yield return new WaitForSeconds(1.2f);

        gameObject.SetActive(false);
    }

    public void ForceClose()
    {
        StopCoroutine("_Setup");

        EffectController.TweenAnchor(rectModula, new Vector2(0, 300), 1f, () => {
            gameObject.SetActive(false);
        });
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

        TaskAttributes.Reward[] rewards = task.GetRewards();

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
}
