using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

//This class will contain the game's save data
//When the game is saved, an instance of this class will be written to the user's device
//When the game is loaded, an instance of this class will will be leaded into ram to update game values
[System.Serializable]
public class GameSaveData {

    //Add any data to be saved here (make sure its public and not static)

    public DateTime saveTime;
    public Dictionary<string, string> stats;
    public Dictionary<string, Task> tasks;
    public Dictionary<int, Item> inventory;
    public List<BuildingSave> buildings;

    //Initialise any game data here
    void Start(){
        stats = new Dictionary<string, string>();
        tasks = new Dictionary<string, Task>();
        inventory = new Dictionary<int, Item>();
        buildings = new List<BuildingSave>();
    }

    public void QuickSave()
    {
        saveTime = DateTime.UtcNow;
        stats = GameController.Instance.GetStats();
        tasks = GameController.Instance.GetTasks();
        inventory = GameController.Instance.GetInventory().GetItemsForSave();
        buildings = BuildingSave.ConvertToSave(GameController.Instance.GetBuildings());
    }

    public void QuickLoad()
    {
        GameController.Instance.SetSaveTime(saveTime);
        GameController.Instance.SetStats(stats);
        GameController.Instance.SetTasks(tasks);
        GameController.Instance.GetInventory().SetItemsFromSave(inventory);
        GameController.Instance.LoadBuildings(buildings);
    }
}

[System.Serializable]
public class EntitySave
{
    public string id;
    public float positionX, positionY;
}

[System.Serializable]
public class BuildingSave : EntitySave
{
    public int producedItems = 0;
    public float lastProduceTime = 0;
    public bool restocked = true;
    public float lastRestockTime = 0;

    public static List<BuildingSave> ConvertToSave(List<Building> buildings)
    {
        List<BuildingSave> save = new List<BuildingSave>();

        if (buildings != null && buildings.Count > 0)
        {
            foreach (Building building in buildings)
            {
                BuildingSave buildingSave = new BuildingSave();
                buildingSave.id = building.GetID();
                buildingSave.positionX = building.transform.position.x;
                buildingSave.positionY = building.transform.position.y;
                buildingSave.producedItems = building.GetProducedItems();
                buildingSave.lastProduceTime = building.GetLastProduceTime();
                buildingSave.restocked = building.GetRestocked();
                buildingSave.lastRestockTime = building.GetLastRestockTime();

                save.Add(buildingSave);
            }
        }

        return save;
    }
}