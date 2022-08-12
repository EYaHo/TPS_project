using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class RobotPlayerMovement : PlayerMovement
{
    private PlayerAnimationController animController;
    
    protected override void Start()
    {
        base.Start();
        animController = GetComponent<PlayerAnimationController>();
    }

    protected override void Move() {
        base.Move();

        if(isGrounded) {
            if(playerInput.horizontalMove == 0 && playerInput.verticalMove == 0) {
                animController.photonView.RPC("ChangeAnimationState", RpcTarget.All, PlayerAnimationController.AnimState.Idle_gunMiddle_AR.ToString(), -1, 0f);
            }
            if(playerInput.horizontalMove != 0) {
                if(playerInput.horizontalMove < 0) {
                    animController.photonView.RPC("ChangeAnimationState", RpcTarget.All, PlayerAnimationController.AnimState.WalkLeft_Shoot_AR.ToString(), -1, 0f);
                    Debug.Log("Left");
                } else {
                    animController.photonView.RPC("ChangeAnimationState", RpcTarget.All, PlayerAnimationController.AnimState.WalkRight_Shoot_AR.ToString(), -1, 0f);
                    Debug.Log("Right");
                }
            }
            else {
                if(playerInput.verticalMove < 0) {
                    animController.photonView.RPC("ChangeAnimationState", RpcTarget.All, PlayerAnimationController.AnimState.WalkBack_Shoot_AR.ToString(), -1, 0f);
                    Debug.Log("Back");
                } else if(playerInput.verticalMove > 0) {
                    animController.photonView.RPC("ChangeAnimationState", RpcTarget.All, PlayerAnimationController.AnimState.WalkFront_Shoot_AR.ToString(), -1, 0f);
                    Debug.Log("Front");
                }
            }
        } else {
           // animController.photonView.RPC("ChangeAnimationState", RpcTarget.All, PlayerAnimationController.AnimState.Jump.ToString(), -1, 0.551f, true);
        }
    }
}
