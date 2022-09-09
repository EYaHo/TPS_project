using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ItemTier {
    Normal,
    Rare,
    Unique,
    Legendary,
}

[CreateAssetMenu(fileName = "Item Data", menuName = "Scriptable Object/Items/Item Data")]
public class ItemData : ScriptableObject
{
    [SerializeField]
    public int id;

    [SerializeField]
    private ItemTier tier;
    public ItemTier Tier { get { return tier; } }

    [SerializeField]
    private Sprite sprite;
    public Sprite Sprite { get { return sprite; } }

    [SerializeField]
    [TextArea(15, 20)]
    private string description;
    public string Description { get { return description; } }

    [SerializeField]
    private ItemBuff[] buffs;
    public ItemBuff[] Buffs { get { return buffs; } }

    public Item CreateItem() {
        Item newItem = new Item(this);
        return newItem;
    }
}
