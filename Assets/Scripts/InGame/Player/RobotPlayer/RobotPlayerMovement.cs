using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
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

        if(characterController.isGrounded) {
            if(moveInput.x == 0 && moveInput.y == 0) {   
                //animController.ChangeAnimationState(PlayerAnimationController.AnimState.Idle_gunMiddle_AR.ToString(), -1, 0f);
                animController.PlayIdleAnimation(-1);
            }
            if(moveInput.x != 0) {
                if(moveInput.x < 0) {
                    //animController.ChangeAnimationState(PlayerAnimationController.AnimState.WalkLeft_Shoot_AR.ToString(), -1, 0f);
                    animController.PlayWalkAnimation("left");
                } else {
                    //animController.ChangeAnimationState(PlayerAnimationController.AnimState.WalkRight_Shoot_AR.ToString(), -1, 0f);
                    animController.PlayWalkAnimation("right");
                }
            }
            else {
                if(moveInput.y < 0) {
                    //animController.ChangeAnimationState(PlayerAnimationController.AnimState.WalkBack_Shoot_AR.ToString(), -1, 0f);
                    animController.PlayWalkAnimation("back");
                } else if(moveInput.y > 0) {
                    //animController.ChangeAnimationState(PlayerAnimationController.AnimState.WalkFront_Shoot_AR.ToString(), -1, 0f);
                    animController.PlayWalkAnimation("front");
                }
            }
        } else {
           // animController.photonView.RPC("ChangeAnimationState", RpcTarget.All, PlayerAnimationController.AnimState.Jump.ToString(), -1, 0.551f, true);
        }
    }
}
