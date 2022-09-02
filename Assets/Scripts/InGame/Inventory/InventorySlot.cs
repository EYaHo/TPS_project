using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class InventorySlot {
    public ItemData item;
    public int amount;

    public InventorySlot(ItemData _item, int _amount) {
        item = _item;
        amount = _amount;
    }

    public void AddAmount(int value) {
        amount += value;
    }
}
