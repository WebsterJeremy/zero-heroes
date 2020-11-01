using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Collections;

public class PopupElement : MonoBehaviour
{
    [Header("Text")]
    public TextMeshProUGUI textTitle;
    public TextMeshProUGUI textMessage;

    [Header("Notice")]
    public RectTransform rectNotice;
    public RectTransform rectModula;
    public Image iconNotice;
    public TextMeshProUGUI textNotice;

    [Header("Buttons")]
    public Button buttonAccept;
    public Button buttonDecline;

    public bool check;

    private System.Action onAccept;
    private System.Action onDecline;
    private System.Action onCheck;

    private float delayInput;

    private void Start()
    {
        AddButtonListeners();
    }

    private void AddButtonListeners()
    {
        buttonAccept.onClick.AddListener(() =>
        {
            SoundController.PlaySound("button");

            if (Check())
            {
                onAccept?.Invoke();

                gameObject.SetActive(false);
            }
        });
        buttonDecline.onClick.AddListener(() =>
        {
            SoundController.PlaySound("button");

            onDecline?.Invoke();

            gameObject.SetActive(false);
        });
    }

    public void Setup(string title, string message, Sprite noticeIcon, string notice, System.Action onAccept, System.Action onDecline, System.Action onCheck) { gameObject.SetActive(true); StartCoroutine(_Setup(title, message, noticeIcon, notice, onAccept, onDecline, onCheck)); }
    public IEnumerator _Setup(string title, string message, Sprite noticeIcon, string notice, System.Action onAccept, System.Action onDecline, System.Action onCheck)
    {
        if (delayInput > Time.time) yield break;

        gameObject.SetActive(false);

        textTitle.text = title;
        textMessage.text = message;
        iconNotice.sprite = noticeIcon;
        textNotice.text = notice;

        rectModula.sizeDelta = new Vector2(500, textMessage.preferredHeight + 260);

        this.onAccept = onAccept;
        this.onDecline = onDecline;
        this.onCheck = onCheck;

//        buttonAccept.interactable = false;
//        buttonDecline.interactable = false;

        gameObject.SetActive(true);
        delayInput = Time.time + 0.1f;

        Check();

        yield return new WaitForSeconds(0.01f);

//        buttonAccept.interactable = true;
//        buttonDecline.interactable = true;
    }

    private bool Check()
    {
        onCheck?.Invoke();

        ColorBlock colorVar = buttonAccept.colors;

        if (!check)
        {
            colorVar.pressedColor = new Color32(255, 150, 150, 255);
            buttonAccept.colors = colorVar;

            rectNotice.GetComponent<Image>().color = new Color32(255, 0, 0, 40);

            return false;
        }

        colorVar.pressedColor = new Color32(200, 200, 200, 255);
        buttonAccept.colors = colorVar;

        rectNotice.GetComponent<Image>().color = new Color32(0, 0, 0, 20);

        return true;
    }
}
