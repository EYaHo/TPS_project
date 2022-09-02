using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class Player : MonoBehaviourPun
{
    public GameObject canvas;

    private void Awake() {
        if(!photonView.IsMine) {
            canvas.SetActive(false);
        }
    }

}
