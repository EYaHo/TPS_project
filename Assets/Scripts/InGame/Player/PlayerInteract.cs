using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayerInteract : MonoBehaviour
{
    [SerializeField]
    public TextMeshProUGUI interactText;

    private void OnTriggerEnter(Collider other) {
        if(other.gameObject.CompareTag("Item")) {
            interactText.text = other.gameObject.GetComponent<Item>().GetInteractString();
        }
    }

    private void OnTriggerExit(Collider other) {
        if(other.gameObject.CompareTag("Item")) {
            interactText.text = "";
        }
    }
}
