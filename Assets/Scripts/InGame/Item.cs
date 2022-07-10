using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : InteractableObject
{
    public override void Interact() {
        Destroy(gameObject);
    }

    public virtual void OnDamage() {        

    }

    public virtual void OnDamaged() {

    }
}
