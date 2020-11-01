using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class ChapterCardElement : MonoBehaviour
{
    public TextMeshProUGUI textTitle;
    public Image imgCompleted;
    public Image imgPanel;

    public Chapter chapter;

    public Button button;

    private void Start()
    {
        button = GetComponent<Button>();
    }

    public void Setup(Chapter chapter)
    {
        if (chapter == null || textTitle == null) return;

        textTitle.text = chapter.GetTitle();

        this.chapter = chapter;

        Check();
    }

    public void Select()
    {
        UIController.Instance.GetProgressMenu().Select(this);
    }

    public bool Check()
    {
        imgCompleted.color = (GameController.Instance.GetChapterStat() == chapter.GetNumber() ? new Color32(128, 200, 224, 255) : 
            (GameController.Instance.GetChapterStat() > chapter.GetNumber() ? new Color32(124, 180, 110, 255) : new Color32(216, 215, 177, 255)));
        imgPanel.color = (chapter.GetNumber() > GameController.Instance.GetChapterStat() ? new Color32(183, 178, 149, 255) : new Color32(252, 247, 222, 255));

        return GameController.Instance.GetChapterStat() == chapter.GetNumber();
    }
}
