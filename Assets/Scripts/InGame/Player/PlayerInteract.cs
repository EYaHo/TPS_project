using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayerInteract : MonoBehaviour
{
    public TextMeshProUGUI interactText;
    public float interactRange;
    public LayerMask itemLayerMask;

    private Player player;
    private PlayerInput playerInput;
    private RaycastHit hitInfo;

    private void Awake() {
        player = GetComponent<Player>();
        playerInput = GetComponent<PlayerInput>();
    }

    private void Update() {
        CheckInteractableObject();
        CheckInteract();
    }

    private void CheckInteractableObject() {
        if(Physics.Raycast(transform.position, transform.forward, out hitInfo, interactRange, itemLayerMask)) {
            Debug.Log(hitInfo.transform.tag);
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
                Item item = hitInfo.transform.GetComponent<Item>();
                if(item) {
                    player.AddItem(item.itemData);
                }
            }
        }
    }

    // private void OnTriggerEnter(Collider other) {
    //     if(other.gameObject.CompareTag("Item")) {
    //         interactText.text = other.gameObject.GetComponent<Item>().GetInteractString();
    //     }
    // }

    // private void OnTriggerStay(Collider other) {
    //     if(playerInput.interact) {
    //         Item item = other.GetComponent<Item>();
    //         if(item) {
    //             player.AddItem(item);
    //             item.Interact();
    //         }
    //         // 추후 Item 스크립트를 넘기는 것이 아니라 
    //         // Player 스크립트에서 모든 아이템을 정의해 놓은 다음
    //         // 해당 아이템의 수를 1 늘리기
    //     }
    // }

    // private void OnTriggerExit(Collider other) {
    //     if(other.gameObject.CompareTag("Item")) {
    //         interactText.text = "";
    //     }
    // }
}
