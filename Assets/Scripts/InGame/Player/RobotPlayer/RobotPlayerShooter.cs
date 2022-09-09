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
            //animController.ChangeAnimationState(PlayerAnimationController.AnimState.Shoot_Autoshot_AR.ToString(), 1, 0f);
            animController.PlayAttackAnimation(3);
            gunController.Fire();
        } else {
            //animController.ChangeAnimationState(PlayerAnimationController.AnimState.Idle_gunMiddle_AR.ToString(), 1, 0f);
            animController.PlayIdleAnimation(1);
        }
    }
}
