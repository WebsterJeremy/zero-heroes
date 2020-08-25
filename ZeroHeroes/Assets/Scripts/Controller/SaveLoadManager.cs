using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

//This is a static manager class to interact with the Game's saving system
//This should be attached to the game manager
public class SaveLoadManager : MonoBehaviour
{
    public static GameSaveData currentSaveData = new GameSaveData();

    //This is the name of the file where the data will be stored to
    private static string defaultSaveFileName;

    // Start is called before the first frame update
    void Start(){

        //This sets the save location of the game's data to an external folder accessible on Windows and Android
        defaultSaveFileName = Application.persistentDataPath + "/save.dat";
    }

    // Update is called once per frame
    void Update(){
        currentSaveData.updateRegularSaveData();
    }

    public static void updateCheckpointData(){
        currentSaveData.UpdateCheckpoint();
    }

    public static void saveData(){saveData(defaultSaveFileName);}

    public static void saveData(string fileName){
        //Save currentSaveData to a file
        Debug.Log("Saving Game Data");
        string destination = fileName;
        FileStream file;

        if (File.Exists(fileName)) file = File.OpenWrite(fileName);
        else file = File.Create(fileName);
        
        BinaryFormatter bf = new BinaryFormatter();
        bf.Serialize(file, currentSaveData);
        file.Close();
    }

    public static void loadData() { loadData(defaultSaveFileName); }

    public static void loadData(string fileName){
        //Load currentSaveData from a file
        Debug.Log("Loading Game Data");
        FileStream file;

        if (File.Exists(fileName)) file = File.OpenRead(fileName);
        else
        {
            Debug.LogError("File not found");
            return;
        }

        BinaryFormatter bf = new BinaryFormatter();
        currentSaveData = (GameSaveData)bf.Deserialize(file);
        file.Close();
    }
}
