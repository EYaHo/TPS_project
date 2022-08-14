using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class RobotPlayerShooter : PlayerShooter
{
    private PlayerAnimationController animController;

    protected override void Awake()
    {
        playerInput = GetComponent<PlayerInput>();
        animController = GetComponent<PlayerAnimationController>();
    }

    protected override void Update()
    {
        if(!photonView.IsMine)
        {
            return;
        }

        if(playerInput.fire) {
            animController.photonView.RPC("ChangeAnimationState", RpcTarget.All, PlayerAnimationController.AnimState.Shoot_Autoshot_AR.ToString(), 1, 0f);
            gunController.Fire();
            /*
            if(PhotonNetwork.IsMasterClient) gunController.Fire();//gunController.photonView.RPC("Fire", RpcTarget.MasterClient);
            else (gunController as RobotGunController).photonView.RPC("RpcFire", RpcTarget.MasterClient);*/
            
            //gunController.photonView.RPC("RpcFire", RpcTarget.All);

        } else {
            animController.photonView.RPC("ChangeAnimationState", RpcTarget.All, PlayerAnimationController.AnimState.Idle_gunMiddle_AR.ToString(), 1, 0f);
        }
    }
}
