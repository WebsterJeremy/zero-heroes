using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestLine
{
    public string title;
    //        public Task[] tasks;
}

public class Condition
{
    public bool passed = false;
}

public class Reward
{

}

[CreateAssetMenu(fileName = "New Task", menuName = "Tasks/Task")]
public class Task : ScriptableObject
{
    public string taskName = "Task";
    public int id = -1;
    public string npc;
    public QuestLine questLine;
    [TextArea(20,20)]
    public string dialog;
    public Objective[] objectives;
    public Condition[] conditions;
    public Reward[] rewards;

    private Dictionary<string, Objective> _objectives = new Dictionary<string, Objective>();

    public Dictionary<string, Objective> Objectives
    {
        get { return _objectives; }
    }

    public Objective GetObjective(string id)
    {
        return _objectives[id];
    }

    public void Awake()
    {
        if (objectives.Length < 1) return;

        foreach (Objective obj in objectives)
        {
            _objectives.Add(obj.objectiveDisplay, obj);
        }
    }

    public void ReadTask()
    {
        Dialogue.Instance.StartReading(dialog);
    }

    public void OnBeginTask() // When the player clicks accept
    {
        ReadTask(); // Remove later, instead this is called when they are only getting the task from NPC not once they have accepted it.

        if (_objectives.Count < objectives.Length)
        {
            foreach (Objective obj in objectives)
            {
                obj.OnBeginTask();
                _objectives.Add(obj.objectiveDisplay, obj);
            }
        }
    }

    public void CompleteObjective(Objective objective)
    {
        foreach (Objective obj in _objectives.Values)
        {
            if (!obj.Completed) return;
        }

        Debug.Log("You have completed this task!");
        // Remove from Task Log
        // Reward Player
    }
}
