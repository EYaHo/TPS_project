using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class GroundItem : InteractableObject, ISerializationCallbackReceiver
{
    public ItemData itemData;

    private void Start() {
        interactString = itemData.name + " 획득 " + "<color=yellow>" + "[E]" + "</color>";
    }

    public override void Interact() {
    }

    public void OnAfterDeserialize()
    {
    }

    public void OnBeforeSerialize()
    {
// #if UNITY_EDITOR
//         GetComponentInChildren<SpriteRenderer>().sprite = itemData.Sprite;
//         EditorUtility.SetDirty(GetComponentInChildren<SpriteRenderer>());
// #endif
    }
}
