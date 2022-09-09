using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayerInteract : MonoBehaviour
{
    public TextMeshProUGUI interactText;
    public float interactRange;
    public LayerMask itemLayerMask;

    private InventoryObject inventory;
    private PlayerInput playerInput;
    private RaycastHit hitInfo;

    private void Awake() {
        inventory = GetComponent<Player>().inventoryObject;
        playerInput = GetComponent<PlayerInput>();
    }

    private void Update() {
        CheckInteractableObject();
        CheckInteract();
    }

    private void CheckInteractableObject() {
        if(Physics.Raycast(transform.position, transform.forward, out hitInfo, interactRange, itemLayerMask)) {
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
                    inventory.AddItem(new Item(groundItem.itemData), 1);
                    // playerInventory.AddItem(groundItem.itemData.CreateItem());
                    Destroy(hitInfo.transform.gameObject);
                }
            }
        }
    }
}
