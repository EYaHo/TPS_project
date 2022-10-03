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
        base.Update();

        if(playerInputManager.fire) {
            animController.PlayAttackAnimation(3);
        } else {
            animController.PlayIdleAnimation(1);
        }
    }
}
