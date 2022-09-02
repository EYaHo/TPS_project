using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Attributes {
    AttackBonus,
    DamageReduction,
    MovementSpeed,
}

public class Item
{
    public string name;
    public int id;
    public ItemBuff[] buffs;

    public Item(ItemData itemData) {
        name = itemData.name;
        id = itemData.Id;
        buffs = new ItemBuff[itemData.Buffs.Length];
        for(int i=0; i < buffs.Length; i++) {
            buffs[i] = new ItemBuff();
        }
    }
}

[System.Serializable]
public class ItemBuff
{
    public Attributes attribute;
    public int value;

    public ItemBuff() {

    }
}
