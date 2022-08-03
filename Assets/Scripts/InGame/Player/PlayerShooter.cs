using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class PlayerShooter : MonoBehaviourPun
{
    protected PlayerInput playerInput;
    [SerializeField]
    public GunController gunController;

    //private PlayerAnimationController animController;

    protected virtual void Awake()
    {
        playerInput = GetComponent<PlayerInput>();
        //animController = GetComponent<PlayerAnimationController>();
    }

    protected virtual void Update()
    {
        if(!photonView.IsMine)
        {
            return;
        }

        if(playerInput.fire) {
            //animController.photonView.RPC("ChangeAnimationState", RpcTarget.All, PlayerAnimationController.AnimState.Shoot_Autoshot_AR.ToString(), 1, 0f);
            gunController.Fire();
        } else {
            //animController.photonView.RPC("ChangeAnimationState", RpcTarget.All, PlayerAnimationController.AnimState.Idle_gunMiddle_AR.ToString(), 1, 0f);
        }
    }

    private void OnEnable() {
        gunController.gameObject.SetActive(true);
    }

    private void OnDisable() {
        gunController.gameObject.SetActive(false);
    }
}
