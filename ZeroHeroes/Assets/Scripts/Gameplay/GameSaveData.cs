using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//This class will contain the game's save data
//When the game is saved, an instance of this class will be written to the user's device
//When the game is loaded, an instance of this class will will be leaded into ram to update game values
[System.Serializable]
public class GameSaveData {

    //Add any data to be saved here (make sure its public and not static)
    public Dictionary<int, Item> inventoryItems = new Dictionary<int, Item>();

    //Initialise any game data here
    void Start(){
        
    }

    public void QuickSave()
    {
        inventoryItems = GameController.Instance.GetInventory().GetItemsForSave();
    }

    public void QuickLoad()
    {
        GameController.Instance.GetInventory().SetItemsFromSave(inventoryItems);
    }
}
