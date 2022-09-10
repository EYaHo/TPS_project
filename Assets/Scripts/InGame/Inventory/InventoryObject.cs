using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

[CreateAssetMenu(fileName = "Inventory", menuName = "Scriptable Object/Inventory")]
public class InventoryObject : ScriptableObject
{
    public string savePath;
    public ItemDatabaseObject database;
    public Inventory inventory;

    private void OnEnable() {

    }

    public void AddItem(Item _item, int _amount) {
        for (int i=0; i < inventory.itemList.Count; i++) {
            if(inventory.itemList[i].item.id == _item.id) {
                inventory.itemList[i].AddAmount(_amount);
                return;
            }
        }

        inventory.itemList.Add(new InventorySlot(_item.id, _item, _amount));
    }

    [ContextMenu("Save")]
    public void Save() {
        IFormatter formatter = new BinaryFormatter();
        Stream stream = new FileStream(string.Concat(Application.persistentDataPath, savePath), FileMode.Create, FileAccess.Write);
        formatter.Serialize(stream, inventory);
        stream.Close();
    }

    public void Load() {
        if(File.Exists(string.Concat(Application.persistentDataPath, savePath))) {
            IFormatter formatter = new BinaryFormatter();
            Stream stream = new FileStream(string.Concat(Application.persistentDataPath, savePath), FileMode.Open, FileAccess.Read);
            inventory = (Inventory)formatter.Deserialize(stream);
            stream.Close();
        }
    }

    [ContextMenu("Clear")]
    public void Clear() {
        inventory = new Inventory();
    }
}
