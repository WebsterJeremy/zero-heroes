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
    public float gameTime;
    public Dictionary<string, string> stats;
    public List<Chapter> chapters;
    public Dictionary<int, Item> inventory;
    public List<BuildingSave> buildings;

    //Initialise any game data here
    void Start(){

    }

    public void QuickSave()
    {
        saveTime = DateTime.UtcNow;
        gameTime = Time.time;
        stats = GameController.Instance.GetStats();
        chapters = GameController.Instance.GetChapters();
        inventory = GameController.Instance.GetInventory().GetItemsForSave();
        buildings = BuildingSave.ConvertToSave(GameController.Instance.GetBuildings());
    }

    public void QuickLoad()
    {
        if (stats == null) NewSave();

        GameController.Instance.SetSaveTime(saveTime);
        GameController.Instance.SetGameTime(gameTime);
        GameController.Instance.SetStats(stats);
        GameController.Instance.SetChapters(chapters);
        GameController.Instance.GetInventory().SetItemsFromSave(inventory);
        GameController.Instance.LoadBuildings(buildings);
    }

    private void NewSave()
    {
        saveTime = DateTime.UtcNow;
        gameTime = Time.time;
        stats = new Dictionary<string, string>();
        chapters = new List<Chapter>();
        inventory = new Dictionary<int, Item>();
        buildings = new List<BuildingSave>();

        stats.Add("money", "500");
        stats.Add("points", "15");
        stats.Add("chapter", "1");

        buildings.Add(new BuildingSave("house_tier1", new Vector2(-2, 7)));

        buildings.Add(new BuildingSave("tree_1", new Vector2(-2, -2)));
        buildings.Add(new BuildingSave("tree_1", new Vector2(-10, 2)));
        buildings.Add(new BuildingSave("tree_1", new Vector2(-17, 3)));
        buildings.Add(new BuildingSave("tree_1", new Vector2(-18, 9)));
        buildings.Add(new BuildingSave("tree_1", new Vector2(-9, 13)));
        buildings.Add(new BuildingSave("tree_1", new Vector2(1, 14)));
        buildings.Add(new BuildingSave("tree_1", new Vector2(8, 13)));
        buildings.Add(new BuildingSave("tree_1", new Vector2(8, 0)));
        buildings.Add(new BuildingSave("tree_1", new Vector2(11, 4)));
        buildings.Add(new BuildingSave("tree_1", new Vector2(-18, -7)));
        buildings.Add(new BuildingSave("tree_1", new Vector2(-18, -18)));
        buildings.Add(new BuildingSave("tree_1", new Vector2(-13, -20)));
        buildings.Add(new BuildingSave("tree_1", new Vector2(-12, -6)));
        buildings.Add(new BuildingSave("tree_1", new Vector2(-8, -8)));
        buildings.Add(new BuildingSave("tree_1", new Vector2(-4, -11)));
        buildings.Add(new BuildingSave("tree_1", new Vector2(-6, -17)));
        buildings.Add(new BuildingSave("tree_1", new Vector2(-1, -19)));
        buildings.Add(new BuildingSave("tree_1", new Vector2(-1, -10)));
        buildings.Add(new BuildingSave("tree_1", new Vector2(4, -2)));
        buildings.Add(new BuildingSave("tree_1", new Vector2(7, -6)));
        buildings.Add(new BuildingSave("tree_1", new Vector2(13, -6)));
        buildings.Add(new BuildingSave("tree_1", new Vector2(17, 0)));
        buildings.Add(new BuildingSave("tree_1", new Vector2(6, -14)));
        buildings.Add(new BuildingSave("tree_1", new Vector2(13, -17)));
        buildings.Add(new BuildingSave("tree_1", new Vector2(15, -10)));

        buildings.Add(new BuildingSave("rock_1", new Vector2(-11, 15)));
        buildings.Add(new BuildingSave("rock_1", new Vector2(-7, 4)));
        buildings.Add(new BuildingSave("rock_1", new Vector2(-12, -14)));
        buildings.Add(new BuildingSave("rock_1", new Vector2(14, 10)));

        saveTime = DateTime.UtcNow;

        GameController.NEW_GAME = true;
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
    public float nextProduceTime = 0;
    public int stockQuantity = 0;
    public Type buildingType;

    public BuildingSave(string id, Vector2 position)
    {
        this.id = id;
        this.positionX = position.x;
        this.positionY = position.y;
    }

    public static List<BuildingSave> ConvertToSave(List<Building> buildings)
    {
        List<BuildingSave> save = new List<BuildingSave>();

        if (buildings != null && buildings.Count > 0)
        {
            foreach (Building building in buildings)
            {
                if (building == null) continue;

                save.Add(building.GetSave());

                if (building != null) GameObject.Destroy(building);
            }
        }

        return save;
    }
}