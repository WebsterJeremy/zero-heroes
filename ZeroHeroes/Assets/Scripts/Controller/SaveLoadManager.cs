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

    public static void saveData(){saveData(defaultSaveFileName);}

    public static void saveData(string fileName){
        //Save currentSaveData to a file
        string destination = fileName;
        FileStream file;

        if (File.Exists(fileName)){
            file = File.OpenWrite(fileName);
        }else file = File.Create(fileName);
        
        BinaryFormatter bf = new BinaryFormatter();
        currentSaveData.QuickSave();
        bf.Serialize(file, currentSaveData);
        file.Close();
    }

    public static bool loadData() { return loadData(defaultSaveFileName); }

    public static bool loadData(string fileName){
        //Load currentSaveData from a file
        FileStream file;

        if (File.Exists(fileName)) {
            file = File.OpenRead(fileName);
            BinaryFormatter bf = new BinaryFormatter();
            currentSaveData = (GameSaveData)bf.Deserialize(file);
            currentSaveData.QuickLoad();
            file.Close();
        }
        else{
            Debug.LogWarning("File not found");
            currentSaveData = new GameSaveData();
            currentSaveData.QuickLoad();
            return true;
        }

        return false;
    }
}
