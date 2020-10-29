using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public class SaveLoadManager : MonoBehaviour
{
    public static GameSaveData currentSaveData = new GameSaveData();
    private static string defaultSaveFileName;

    void Start(){
        defaultSaveFileName = Application.persistentDataPath + "/save.dat";
    }

    public static void saveData(){saveData(defaultSaveFileName);}

    public static void saveData(string fileName){
        //Save currentSaveData to a file
        string destination = fileName;
        FileStream file;

        if (File.Exists(fileName)){
            file = File.OpenWrite(fileName);
        } else file = File.Create(fileName);
        
        BinaryFormatter bf = new BinaryFormatter();
        currentSaveData.QuickSave();
        bf.Serialize(file, currentSaveData);
        file.Close();
    }

    public static bool loadData() { return loadData(defaultSaveFileName); }

    public static bool loadData(string fileName){
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

    public static void resetData() { resetData(defaultSaveFileName); }
    public static void resetData(string fileName)
    {
        if (File.Exists(fileName))
        {
            File.Delete(fileName);
        }
    }
}
