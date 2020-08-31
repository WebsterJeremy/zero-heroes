using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Objective : ScriptableObject
{
    public string objectiveDisplay = "Complete Objectieve";
    public Task task;
    public int total = 1;

    private int current = 0;
    private bool completed = false;

    private void OnEnable() // ScriptableObject's save data accross play throughs, you must reset values manually
    {
        current = 0;
        completed = false;
    }

    public bool Completed
    {
        get { return completed; }
    }

    public virtual void DoObjective()
    {
        current++;

        Debug.Log(objectiveDisplay + " : " + current + " / " + total);

        if (current >= total && !completed)
        {
            OnComplete();
        }
    }

    public abstract void OnBeginTask();
    public abstract void OnFailTask();
    public virtual void OnComplete()
    {
        completed = true;
        task.CompleteObjective(this);

        Debug.Log("You completed objective: "+ objectiveDisplay);
    }
}
