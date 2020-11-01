using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class Task
{
    [System.NonSerialized] private TaskAttributes attributes;
    [SerializeField] private int[] progress;
    [SerializeField] private bool completed = false;

    public void Setup(TaskAttributes attributes)
    {
        this.attributes = attributes;

        progress = new int[GetObjectives().Length];
    }

    public string GetTitle()
    {
        return attributes.GetTitle();
    }

    public NpcAttributes GetNpc()
    {
        return attributes.GetNpc();
    }

    public string GetDescription()
    {
        return attributes.GetDescription();
    }

    public TaskAttributes.Objective[] GetObjectives()
    {
        return attributes.GetObjectives();
    }

    public TaskAttributes.Objective GetObjective(int id)
    {
        return attributes.GetObjectives()[id];
    }

    public TaskAttributes.Reward[] GetRewards()
    {
        return attributes.GetRewards();
    }

    public bool GetCompleted()
    {
        if (completed) return true;
        if (this == null || attributes == null || GetObjectives() == null) return false;

        for (int i = 0; i < progress.Length; i++) if (progress[i] < GetObjectives()[i].total) return false;
        completed = true;

        UIController.Instance.GetPopupTaskElement().Setup(this);

        if (GetRewards().Length > 0)
        {
            for (int i = 0;i < GetRewards().Length;i++)
            {
                TaskAttributes.Reward reward = GetRewards()[i];

                switch (reward.rewardType)
                {
                    case TaskAttributes.RewardType.ITEM:
                        GameController.Instance.GetInventory().GiveItem(reward.data, reward.quantity, -1);

                        break;
                    case TaskAttributes.RewardType.POINTS:
                        GameController.Instance.GivePoints(reward.quantity);

                        break;
                    default:
                        GameController.Instance.GiveMoney(reward.quantity);

                        break;
                }
            }
        }

        return true;
    }

    public int GetObjectiveProgress(int id)
    {
        return progress[id];
    }

    public void CheckObjectives(TaskAttributes.ObjectiveType type)
    {
        if (completed || GetObjectives() == null) return;

        foreach (TaskAttributes.Objective objective in GetObjectives())
        {
            if (objective.objectiveType == type) Check(objective.id);
        }

        GetCompleted();
    }

    public void CheckObjectives()
    {
        if (completed || GetObjectives() == null) return;

        foreach (TaskAttributes.Objective objective in GetObjectives())
        {
            Check(objective.id);
        }

        GetCompleted();
    }

    public void Check(int id)
    {
        if (completed || progress[id] == GetObjectives()[id].total) return;

        TaskAttributes.Objective objective = GetObjective(id);
        int change = 0;

        switch (objective.objectiveType)
        {
            case TaskAttributes.ObjectiveType.ITEM:
                List<Item> foundItems = GameController.Instance.GetInventory().FindItem(objective.data);
                int quantity = 0;

                if (foundItems != null && foundItems.Count > 0)
                {
                    foreach (Item item in foundItems) quantity += item.GetQuantity();
                }

                change = quantity;

                break;
            case TaskAttributes.ObjectiveType.BUILDING:
                List<Building> foundBuildings = GameController.Instance.GetBuildings();
                int count = 0;

                if (foundBuildings != null && foundBuildings.Count > 0)
                {
                    foreach (Building building in foundBuildings) if (building.GetID() == objective.data) count++;
                }

                change = count;

                break;
            default:
                change = GameController.Instance.GetStat(objective.data, 0);

                break;
        }

        progress[id] = change;
    }
}
