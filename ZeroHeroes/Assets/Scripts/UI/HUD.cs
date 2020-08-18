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
    [SerializeField] private TextMeshProUGUI textDebugStatistics;

    [Header("Containers")]
    [SerializeField] private RectTransform rectMoney;

    #endregion
    #region PrivateVariables


    private static Dictionary<string, string> debugStatistics = new Dictionary<string, string>();


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

    public void ChangeMoney(int startValue, int endValue, float duration) { StartCoroutine(_ChangeMoney(startValue, endValue, duration)); }
    IEnumerator _ChangeMoney(int startValue, int endValue, float duration)
    {
        float animTime = 0f;
        float money = startValue;

        while (animTime < duration)
        {
            money = Mathf.Lerp(money, endValue, animTime / duration);
            DisplayMoney((int)money);

            yield return new WaitForEndOfFrame();
            animTime += Time.deltaTime;
        }
    }

    private void DisplayMoney(int money)
    {
        textMoney.text = money.ToString();
        rectMoney.sizeDelta = new Vector2(100 + (textMoney.text.Length * 40), rectMoney.sizeDelta.y);
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
