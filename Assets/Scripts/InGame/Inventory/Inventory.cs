using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Inventory {
    public List<InventorySlot> itemList = new List<InventorySlot>();
}



[System.Serializable]
public class InventorySlot {
    public int ID;
    public Item item;
    public int amount;

    public InventorySlot(int _id, Item _item, int _amount) {
        ID = _id;
        item = _item;
        amount = _amount;
    }

    public void AddAmount(int value) {
        amount += value;
    }
}
