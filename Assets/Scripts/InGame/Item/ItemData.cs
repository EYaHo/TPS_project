using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ItemTier {
    Normal,
    Rare,
    Unique,
    Legendary,
}

[CreateAssetMenu(fileName = "Item Data", menuName = "Scriptable Object/Item Data")]
public class ItemData : ScriptableObject
{
    [SerializeField]
    private int id;
    public int Id { get { return id; } }

    [SerializeField]
    private ItemTier tier;
    public ItemTier Tier { get { return tier; } }

    [SerializeField]
    private GameObject prefab;
    public GameObject Prefab { get { return prefab; } }

    [SerializeField]
    private Sprite sprite;
    public Sprite Sprite { get { return sprite; } }

    [SerializeField]
    private GroundItem groundItem;
    public GroundItem GroundItem { get { return groundItem; } }

    [SerializeField]
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
