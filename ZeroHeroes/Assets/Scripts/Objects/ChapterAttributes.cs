using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Chapter", menuName = "Tasks/Chapter")]
public class ChapterAttributes : ScriptableObject
{
    public string title;
    public Sprite graphics;
    public int number;
    public TaskAttributes[] tasks;

    [TextArea(20, 20)]
    public string description;
    
    public string GetTitle()
    {
        return title;
    }

    public Sprite GetGraphics()
    {
        return graphics;
    }

    public int GetNumber()
    {
        return number;
    }

    public string GetDescription()
    {
        return description;
    }

    public TaskAttributes[] GetTasks()
    {
        return tasks;
    }
}
