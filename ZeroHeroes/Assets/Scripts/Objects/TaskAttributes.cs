using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;



[CreateAssetMenu(fileName = "New Task", menuName = "Tasks/Task")]
public class TaskAttributes : ScriptableObject
{
    public enum ObjectiveType { ITEM, BUILDING, STAT };
    public enum RewardType { ITEM, MONEY, POINTS };

    public string title = "Task";
    public NpcAttributes npc;
    [TextArea(20, 20)]
    public string description;

    [Serializable]
    public struct Objective
    {
        public string title;
        public int id;
        public string data;
        public ObjectiveType objectiveType;
        public int total;
    }
    [SerializeField] public Objective[] objectives;

    [Serializable]
    public struct Reward
    {
        public RewardType rewardType;
        public int quantity;
        public string data;
    }
    [SerializeField] public Reward[] rewards;

    public string GetTitle()
    {
        return title;
    }

    public NpcAttributes GetNpc()
    {
        return npc;
    }

    public string GetDescription()
    {
        return description;
    }

    public Objective[] GetObjectives()
    {
        return objectives;
    }

    public Reward[] GetRewards()
    {
        return rewards;
    }
}
