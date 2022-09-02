using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInventory : MonoBehaviour
{
    public List<InventorySlot> inventory = new List<InventorySlot>();

    public void AddItem(ItemData _item, int _amount) {
        bool hasItem = false;
        for (int i=0; i < inventory.Count; i++) {
            if(inventory[i].item == _item) {
                inventory[i].AddAmount(_amount);
                hasItem = true;
                break;
            }
        }

        if(!hasItem) {
            inventory.Add(new InventorySlot(_item, _amount));
        }
    }
    
    private void OnApplicationQuit() {
        inventory.Clear();
    }

    
    // public float OnAttack(float damage) {
    //     // for(int i=0; i < items.Count; i++) {
    //     //     damage = items[i].OnAttack(damage);
    //     // }

    //     return damage;
    // }
}
