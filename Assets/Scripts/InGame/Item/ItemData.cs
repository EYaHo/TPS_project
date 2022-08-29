using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Item Data", menuName = "Scriptable Object/Item Data")]
public class ItemData : ScriptableObject
{
    public enum ItemTier {
        Normal,
        Rare,
        Unique,
        Legendary,
    }

    [SerializeField]
    private string itemName;
    public string ItemName { get { return itemName; } }

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
    private Item itemScript;
    public Item ItemScript { get { return itemScript; } }
}
