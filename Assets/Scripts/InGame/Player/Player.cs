using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class Player : MonoBehaviourPun
{
    public GameObject canvas;
    public Item[] items;

    private void Awake() {
        if(!photonView.IsMine) {
            canvas.SetActive(false);
        }
    }

    public float OnAttack(float damage) {
        foreach (Item item in items) {
            damage = item.OnAttack(damage);
        }

        return damage;
    }
}
