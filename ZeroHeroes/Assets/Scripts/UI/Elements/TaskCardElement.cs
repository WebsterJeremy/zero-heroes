using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class TaskCardElement : MonoBehaviour
{
    public TextMeshProUGUI textTitle;
    public Image imgCompleted;
    public Image imgPanel;

    [System.NonSerialized] public Task task;

    private Button button;

    private void Start()
    {
        button = GetComponent<Button>();
    }

    public void Setup(Task task)
    {
        if (task == null) return;
        textTitle.text = task.GetTitle();

        this.task = task;

        Check();
    }

    public void Select()
    {
        UIController.Instance.GetTasklogMenu().Select(this);
    }

    public bool Check()
    {

        imgCompleted.color = task.GetCompleted() ? new Color32(124, 180, 110, 255) : new Color32( 216, 215, 177, 255);

        imgPanel.color = new Color32(252, 247, 222, 255);

        return task.GetCompleted();
    }
}
