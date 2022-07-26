using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class PlayerShooter : MonoBehaviourPun
{
    private PlayerInput playerInput;
    [SerializeField]
    public GunController gunController;

    void Awake()
    {
        playerInput = GetComponent<PlayerInput>();
    }

    void Update()
    {
        if(!photonView.IsMine)
        {
            return;
        }

        if(playerInput.fire) {
            gunController.Fire();
        }
    }

    private void OnEnable() {
        gunController.gameObject.SetActive(true);
    }

    private void OnDisable() {
        gunController.gameObject.SetActive(false);
    }
}
