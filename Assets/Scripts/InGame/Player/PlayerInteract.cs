using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayerInteract : MonoBehaviour
{
    public TextMeshProUGUI interactText;
    public float interactRange;
    public LayerMask itemLayerMask;

    private InventoryObject inventoryObject;
    private PlayerInput playerInput;
    private GameObject interactableObject;
    private readonly Collider[] _colliders = new Collider[3];

    private void Awake() {
        inventoryObject = GetComponent<Player>().inventoryObject;
        playerInput = GetComponent<PlayerInput>();
    }

    private void Update() {
        CheckInteractableObject();
        CheckInteract();
    }

    private void CheckInteractableObject() {
        interactText.text = "";
        interactableObject = null;
        int numFound = Physics.OverlapSphereNonAlloc(transform.position, interactRange, _colliders, itemLayerMask);
        
        if(numFound > 0) {
            interactableObject = _colliders[0].gameObject;
            if(interactableObject != null) {
                interactText.text = interactableObject.transform.GetComponent<InteractableObject>().GetInteractString();
            }
        }
    }

    private void CheckInteract() {
        if(interactableObject.transform != null) {
            if(playerInput.interact) {
                interactableObject.transform.GetComponent<InteractableObject>().Interact();
                GroundItem groundItem = interactableObject.transform.GetComponent<GroundItem>();
                if(groundItem) {
                    inventoryObject.AddItem(new Item(groundItem.itemData), 1);
                    Destroy(interactableObject.transform.gameObject);
                }
            }
        }
    }
}
