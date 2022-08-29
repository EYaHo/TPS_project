using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractableObject : MonoBehaviour
{
    protected string interactString = "";

    public string GetInteractString() {
        return interactString;
    }

    public virtual void Interact() {
        
    }
}
