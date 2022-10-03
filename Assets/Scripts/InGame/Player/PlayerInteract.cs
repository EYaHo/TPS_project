using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;

public class PlayerInteract : MonoBehaviour
{
    public TextMeshProUGUI interactText;
    public float interactRange;
    public LayerMask itemLayerMask;

    private InventoryObject inventoryObject;
    private PlayerInputManager playerInputManager;
    private GameObject interactableObject;
    private bool prevInteractPressed;
    private readonly Collider[] _colliders = new Collider[3];

    private void Awake() {
        inventoryObject = GetComponent<Player>().inventoryObject;
        playerInputManager = GetComponent<PlayerInputManager>();
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
        if(playerInputManager.interact) {
            if(prevInteractPressed == false) {
                prevInteractPressed = true;
                if(interactableObject != null) {
                    interactableObject.transform.GetComponent<InteractableObject>().Interact();
                    GroundItem groundItem = interactableObject.transform.GetComponent<GroundItem>();
                    if(groundItem) {
                        inventoryObject.AddItem(new Item(groundItem.itemData), 1);
                        Destroy(interactableObject.transform.gameObject);
                    }
                }
            }
        } else {
            prevInteractPressed = false;
        }
    }
}
