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
    private InteractableObject interactableObject;
    private GroundItem groundItem;
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
            interactableObject = _colliders[0].gameObject.GetComponent<InteractableObject>();
            groundItem = _colliders[0].gameObject.GetComponent<GroundItem>();
            if(interactableObject != null) {
                interactText.text = interactableObject.GetInteractString();
            }
        }
    }
    
    private void CheckInteract() {
    // interact키가 눌렸을 경우 상호작용 가능한 오브젝트와 상호작용
        if(playerInputManager.interact) {
            // interact키를 누를 때만 함수가 동작하도록 하는 조건문
            if(prevInteractPressed == false) {
                prevInteractPressed = true;
                
                if(interactableObject != null) {
                    interactableObject.Interact();

                    // 아이템이라면
                    if(groundItem) {
                        inventoryObject.AddItem(new Item(groundItem.itemData), 1);
                        interactableObject.DestoryInNetwork();
                    }
                }
            }
        } else {
            prevInteractPressed = false;
        }
    }

}
