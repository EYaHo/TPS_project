using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : InteractableObject
{
    public override void Interact() {
        Destroy(gameObject);
    }

    public virtual float OnAttack(float damage) {
        return damage;
    }

    public virtual void OnDamage() {

    }
}
