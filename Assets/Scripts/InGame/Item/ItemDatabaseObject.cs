using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Item Database", menuName = "Scriptable Object/Items/Database")]
public class ItemDatabaseObject : ScriptableObject, ISerializationCallbackReceiver
{
    public ItemData[] itemList;
    public Dictionary<int, ItemData> itemDict = new Dictionary<int, ItemData>();

    public void OnAfterDeserialize() {
        for(int i=0; i < itemList.Length; i++) {
            itemList[i].id = i;
            itemDict.Add(i, itemList[i]);
        }
    }

    public void OnBeforeSerialize() {
        itemDict = new Dictionary<int, ItemData>();
    }
}
