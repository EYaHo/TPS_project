using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Item Database", menuName = "Scriptable Object/Items/Database")]
public class ItemDatabaseObject : ScriptableObject, ISerializationCallbackReceiver
{
    public ItemData[] itemArray;
    public Dictionary<int, ItemData> itemDict = new Dictionary<int, ItemData>();

    public void OnAfterDeserialize() {
        for(int i=0; i < itemArray.Length; i++) {
            itemArray[i].id = i;
            itemDict.Add(i, itemArray[i]);
        }
    }

    public void OnBeforeSerialize() {
        itemDict = new Dictionary<int, ItemData>();
    }
}
