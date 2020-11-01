using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class Chapter
{
    [System.NonSerialized] ChapterAttributes attributes;

    [SerializeField] private bool completed = false;
    [SerializeField] private Task[] tasks;

    public void Setup(ChapterAttributes attributes)
    {
        this.attributes = attributes;
        this.completed = false;

        tasks = new Task[attributes.GetTasks().Length];

        for (int i = 0;i < attributes.GetTasks().Length;i++)
        {
            Task task = new Task();
            task.Setup(attributes.GetTasks()[i]);

            tasks[i] = task;
        }
    }
    
    public string GetTitle()
    {
        return attributes.GetTitle();
    }

    public Sprite GetGraphics()
    {
        return attributes.GetGraphics();
    }

    public int GetNumber()
    {
        return attributes.GetNumber();
    }

    public string GetDescription()
    {
        return attributes.GetDescription();
    }

    public Task[] GetTasks()
    {
        return tasks;
    }

    public bool GetCompleted()
    {
        if (tasks.Length < 1) return false;

        for (int i = 0;i < tasks.Length;i++)
        {
            if (!tasks[i].GetCompleted()) return false;
        }

        if (GetNumber() < 5) GameController.Instance.SetChapterStat(GetNumber() + 1);

        return true;
    }

    public int GetProgress()
    {
        if (tasks == null || tasks.Length < 1) return 0;
        int completed = 0;

        for (int i = 0; i < tasks.Length; i++)
        {
            if (tasks[i].GetCompleted()) completed++;
        }

        return completed;
    }

    public void CheckObjectivies(TaskAttributes.ObjectiveType objectiveType)
    {
        if (tasks.Length < 1) return;

        for (int i = 0; i < tasks.Length; i++)
        {
            tasks[i].CheckObjectives(objectiveType);
        }
    }
}
