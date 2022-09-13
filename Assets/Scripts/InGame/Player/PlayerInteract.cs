using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayerInteract : MonoBehaviour
{
    public TextMeshProUGUI interactText;
    public float interactRange;
    public LayerMask itemLayerMask;
    public Transform interactStartTransform;

    private InventoryObject inventoryObject;
    private PlayerInput playerInput;
    private RaycastHit hitInfo;
    private float interactVectorYOffset = 2f;

    private void Awake() {
        inventoryObject = GetComponent<Player>().inventoryObject;
        playerInput = GetComponent<PlayerInput>();
    }

    private void Update() {
        CheckInteractableObject();
        CheckInteract();
    }

    private void CheckInteractableObject() {
        if(Physics.Raycast(interactStartTransform.position, interactStartTransform.forward, out hitInfo, interactRange, itemLayerMask)) {
            if(hitInfo.transform.CompareTag("Interactable")) {
                interactText.text = hitInfo.transform.GetComponent<InteractableObject>().GetInteractString();
            }
        } else {
            interactText.text = "";
        }
    }

    private void CheckInteract() {
        if(hitInfo.transform != null) {
            if(playerInput.interact) {
                hitInfo.transform.GetComponent<InteractableObject>().Interact();
                GroundItem groundItem = hitInfo.transform.GetComponent<GroundItem>();
                if(groundItem) {
                    inventoryObject.AddItem(new Item(groundItem.itemData), 1);
                    // playerInventory.AddItem(groundItem.itemData.CreateItem());
                    Destroy(hitInfo.transform.gameObject);
                }
            }
        }
    }
}
