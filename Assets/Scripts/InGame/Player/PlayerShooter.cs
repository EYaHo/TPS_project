using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class PlayerShooter : MonoBehaviourPun
{
    private PlayerInput playerInput;
    [SerializeField]
    private GunController gunController;

    void Awake()
    {
        playerInput = GetComponent<PlayerInput>();
        gunController = transform.Find("Gun").gameObject.GetComponent<GunController>();
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
