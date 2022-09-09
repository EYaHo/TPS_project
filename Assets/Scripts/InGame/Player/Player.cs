using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class Player : MonoBehaviourPun
{
    public InventoryObject inventoryObject;
    public GameObject canvas;

    private void Awake() {
        if(!photonView.IsMine) {
            canvas.SetActive(false);
        }
    }

    private void OnApplicationQuit() {
        inventoryObject.inventory.itemList.Clear();
    }
}
