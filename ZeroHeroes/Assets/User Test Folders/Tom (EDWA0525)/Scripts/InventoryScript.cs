using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

[CreateAssetMenu(fileName = "New INventory", menuName = "Inventory System/Inventory")]

public class InventoryScript : ScriptableObject, ISerializationCallbackReceiver
{
    public string savePath;

    private ItemsDatabase database;

    public List<InvSlot> Cont = new List<InvSlot>();

    private void OnEnable()
    {
#if UNITY_EDITOR
        database = (ItemsDatabase)AssetDatabase.LoadAssetAtPath("Assets/User Test Folders/Tom (EDWA0525)/scriptableObjs/Resources/Database.asset", typeof(ItemsDatabase));
#else
        database = Resources.Load<ItemsDatabase>("Database");
#endif
    }

    public void AddItem(itemObjScript _item, int _val)
    {

        for (int i = 0; i < Cont.Count; i++)
        {
            if(Cont[i].item == _item)
            {
                Cont[i].AddVal(_val);

                return;
            }
        }
        Cont.Add(new InvSlot(database.GetID[_item], _item, _val));
        
    }

    public void Save()
    {
        string saveData = JsonUtility.ToJson(this, true);
        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Create(string.Concat(Application.persistentDataPath, savePath));
        bf.Serialize(file, saveData);
        file.Close();
    }

    public void Load()
    {
        if(File.Exists(string.Concat(Application.persistentDataPath, savePath))){
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(string.Concat(Application.persistentDataPath, savePath), FileMode.Open);
            JsonUtility.FromJsonOverwrite(bf.Deserialize(file).ToString(), this);
            file.Close();
        }
    }

    public void OnAfterDeserialize()
    {
        if (Cont == null || Cont.Count == 0 || database == null) return;

        for (int i = 0; i < Cont.Count; i++)
        {
            if (Cont[i].item == null || database.GetObj == null || !database.GetObj.ContainsKey(Cont[i].ID)) continue; // Add more error checking!

            Cont[i].item = database.GetObj[Cont[i].ID];
        }
    }

    public void OnBeforeSerialize()
    {
    }
}

[System.Serializable]

public class InvSlot
{
    public int ID;
    public itemObjScript item;
    public int val;
    public InvSlot(int _id, itemObjScript _item, int _val)
    {
        ID = _id;
        item = _item;
        val = _val;
    }

    public void AddVal(int value)
    {
        val += value;
    }
}
