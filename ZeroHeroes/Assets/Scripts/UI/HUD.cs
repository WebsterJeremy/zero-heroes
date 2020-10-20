using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HUD : MonoBehaviour
{
    #region AccessVariables


    [Header("Buttons")]
    [SerializeField] private Button buttonAddMoney;

    [Header("Text")]
    [SerializeField] private TextMeshProUGUI textMoney;
    [SerializeField] private TextMeshProUGUI textPoints;
    [SerializeField] private TextMeshProUGUI textDebugStatistics;

    [Header("Containers")]
    [SerializeField] private RectTransform rectMoney;
    [SerializeField] private RectTransform rectPoints;
    [SerializeField] private RectTransform notifyContainer;

    [Header("Utility")]
    [SerializeField] private GameObject notifyPrefab;

    #endregion
    #region PrivateVariables


    private static Dictionary<string, string> debugStatistics = new Dictionary<string, string>();
    private float oldMoney = 0;
    private float oldPoints = 0;
    private List<Notify> notifyList = new List<Notify>();

    #endregion
    #region Initlization


    void Start()
    {
        debugStatistics = new Dictionary<string, string>();

        AddButtonListeners();
    }


    #endregion
    #region Core


    protected void AddButtonListeners()
    {
        buttonAddMoney.onClick.AddListener(() =>
        {
            SoundController.PlaySound("button");

            GameController.Instance.SetMoney(GameController.Instance.GetMoney() + Random.Range(20,35));
        });
    }

    public void ChangeMoney() { StartCoroutine(_ChangeMoney()); }
    IEnumerator _ChangeMoney()
    {
        float animTime = 0f;
        float money = oldMoney;
        float endValue = GameController.Instance.GetStat("Money", 0);
        float duration = 0.5f;

        while (animTime < duration)
        {
            money = Mathf.Lerp(money, endValue, animTime / duration);
            DisplayMoney((int)money);

            yield return new WaitForEndOfFrame();
            animTime += Time.deltaTime;
        }
    }

    public void DisplayMoney(int money)
    {
        textMoney.text = "$" + money.ToString();
        rectMoney.sizeDelta = new Vector2(0 + (textMoney.text.Length * 40), rectMoney.sizeDelta.y);
    }

    public void ChangePoints() { StartCoroutine(_ChangePoints()); }
    IEnumerator _ChangePoints()
    {
        float animTime = 0f;
        float points = oldPoints;
        float endValue = GameController.Instance.GetStat("Points", 0);
        float duration = 0.5f;

        while (animTime < duration)
        {
            points = Mathf.Lerp(points, endValue, animTime / duration);
            DisplayPoints((int)points);

            yield return new WaitForEndOfFrame();
            animTime += Time.deltaTime;
        }
    }

    public void DisplayPoints(int points)
    {
        textPoints.text = "$" + points.ToString();
        rectPoints.sizeDelta = new Vector2(0 + (textPoints.text.Length * 40), rectPoints.sizeDelta.y);
    }

    public Notify CreateNotify(string label, int quantity, Sprite icon, Transform follow, Vector3 offset)
    {
        GameObject obj = Instantiate(notifyPrefab);
        obj.transform.SetParent(notifyContainer);

        Notify notify = obj.GetComponent<Notify>();
        notify.Setup(label, quantity, icon, follow, offset);

        notifyList.Add(notify);

        return notify;
    }

    public void RemoveNotify(Notify notify)
    {
        notifyList.Remove(notify);
        Destroy(notify.gameObject);
    }

    #endregion
    #region Debug


    public void SetDebugStatistic(string statistic, string value)
    {
        if (debugStatistics.ContainsKey(statistic))
        {
            debugStatistics[statistic] = value;
        }
        else
        {
            debugStatistics.Add(statistic, value);
        }

        string debug = "<mark=#0000001A>\n"; // <mark> sets the text background color with RichText formatting
        foreach (string key in debugStatistics.Keys)
        {
            debug += key + ": " + debugStatistics[key] + "\n";
        }

        if (textDebugStatistics != null)
            textDebugStatistics.text = debug + "</mark>";
    }


    #endregion
}
