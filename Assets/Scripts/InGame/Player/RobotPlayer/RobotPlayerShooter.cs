using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class RobotPlayerShooter : PlayerShooter
{
    private PlayerAnimationController animController;

    protected override void Awake()
    {
        base.Awake();
        animController = GetComponent<PlayerAnimationController>();
    }

    protected override void Update()
    {
        if(!photonView.IsMine)
        {
            return;
        }

        if(playerInputManager.fire) {
            gunController.Fire();
            animController.PlayAttackAnimation(3);
        } else {
            animController.PlayIdleAnimation(1);
        }
    }
}
