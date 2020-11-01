using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class ObjectiveCardElement : MonoBehaviour
{
    public TextMeshProUGUI textTitle;
    public TextMeshProUGUI textProgress;
    public Image imgCompleted;

    [System.NonSerialized] public Task task;
    [System.NonSerialized] public TaskAttributes.Objective objective;

    private void Start()
    {

    }

    public void Setup(Task task, TaskAttributes.Objective objective)
    {
        textTitle.text = objective.title;
        textProgress.text = Mathf.Clamp(task.GetObjectiveProgress(objective.id),0, objective.total) + " / " + objective.total;

        this.task = task;
    }
}
