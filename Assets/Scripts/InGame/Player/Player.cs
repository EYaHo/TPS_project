using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class Player : MonoBehaviourPun
{
    public GameObject canvas;
    private List<ItemData> items = new List<ItemData>();

    private void Awake() {
        if(!photonView.IsMine) {
            canvas.SetActive(false);
        }
    }

    public float OnAttack(float damage) {
        for(int i=0; i < items.Count; i++) {
            //damage = items[i].OnAttack(damage);
        }

        return damage;
    }

    public void AddItem(ItemData itemData) {
        items.Add(itemData);
    }
}
