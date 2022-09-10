using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundItem : InteractableObject
{
    public ItemData itemData;
    private SpriteRenderer spriteRenderer;

    private void Start() {
        spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.sprite = itemData.Sprite;
        interactString = itemData.name + " 획득 " + "<color=yellow>" + "[E]" + "</color>";
    }

    public override void Interact() {
    }
}
