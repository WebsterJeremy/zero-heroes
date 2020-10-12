using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using System.Runtime.Serialization;

[CreateAssetMenu(fileName = "New Inventory", menuName = "Inventory System/Inventory")]

public class InventoryScriptTE : ScriptableObject
{
    public string savePath;

    public ItemsDatabaseTE database;

    public InventoryTE Cont;

    private void OnEnable()
    {
/*#if UNITY_EDITOR
        database = (ItemsDatabaseTE)AssetDatabase.LoadAssetAtPath("Assets/User Test Folders/Tom (EDWA0525)/scriptableObjs/Resources/Database.asset", typeof(ItemsDatabaseTE));
#else
        database = Resources.Load<ItemsDatabase>("Database");
#endif*/
    }

    public void AddItem(invItemTE _item, int _amount)
    {

        for (int i = 0; i < Cont.Items.Length; i++)
        {
            if(Cont.Items[i].ID == _item.Id)
            {
                Cont.Items[i].AddAmount(_amount);
                return;
            }
        }
        SetEmptySlot(_item, _amount);

    }

    public InvSlotTE SetEmptySlot(invItemTE _item, int _val)
    {
        for (int i = 0; i < Cont.Items.Length; i++)
        {
            if(Cont.Items[i].ID <= -1)
            {
                Cont.Items[i].UpdateSlot(_item.Id, _item, _val);
                return Cont.Items[i];
            }
        }

        return null;
    }

    public void MoveItem(InvSlotTE slot1, InvSlotTE slot2)
    {
        InvSlotTE temp = new InvSlotTE(slot2.ID, slot2.item, slot2.amount);
        slot2.UpdateSlot(slot1.ID, slot1.item, slot1.amount);
        slot1.UpdateSlot(temp.ID, temp.item, temp.amount);
    }

    public void RemoveItem(invItemTE _item)
    {
        for (int i = 0; i < Cont.Items.Length; i++)
        {
            if(Cont.Items[i].item == _item)
            {
                Cont.Items[i].UpdateSlot(-1, null, 0);

            }
        }
    }

    [ContextMenu("Save")]
    public void Save()
    {
        IFormatter formatter = new BinaryFormatter();
        Stream stream = new FileStream(string.Concat(Application.persistentDataPath, savePath), FileMode.Create, FileAccess.Write);
        formatter.Serialize(stream, Cont);
        stream.Close();
    }


    [ContextMenu("Load")]
    public void Load()
    {
        if(File.Exists(string.Concat(Application.persistentDataPath, savePath))){
            /*BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(string.Concat(Application.persistentDataPath, savePath), FileMode.Open);
            JsonUtility.FromJsonOverwrite(bf.Deserialize(file).ToString(), this);
            file.Close();*/
            IFormatter formatter = new BinaryFormatter();
            Stream stream = new FileStream(string.Concat(Application.persistentDataPath, savePath), FileMode.Open, FileAccess.Read);
            InventoryTE newContainer = (InventoryTE)formatter.Deserialize(stream);
            for (int i = 0; i < Cont.Items.Length; i++)
            {
                Cont.Items[i].UpdateSlot(newContainer.Items[i].ID, newContainer.Items[i].item, newContainer.Items[i].amount);
            }
            stream.Close();
        }
    }

    [ContextMenu("Clear")]
    public void Clear()
    {
        Cont = new InventoryTE();
    }

}

[System.Serializable]

public class InventoryTE
{
    public InvSlotTE[] Items = new InvSlotTE[16];
}

[System.Serializable]

public class InvSlotTE
{
    public int ID = -1;
    public invItemTE item;
    public int amount;
    public InvSlotTE()
    {
        ID = -1;
        item = null;
        amount = 0;
    }

    public InvSlotTE(int _id, invItemTE _item, int _amount)
    {
        ID = _id;
        item = _item;
        amount = _amount;

    }

    public void UpdateSlot(int _id, invItemTE _item, int _amount)
    {
        ID = _id;
        item = _item;
        amount = _amount;
    }

    public void AddAmount(int val)
    {
        amount += val;
    }
}
