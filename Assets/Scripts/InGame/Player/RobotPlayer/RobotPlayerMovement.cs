using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class RobotPlayerMovement : PlayerMovement
{
    private PlayerAnimationController animController;
    
    protected override void Start()
    {
        playerInput = GetComponent<PlayerInput>();
        playerRigidbody = GetComponent<Rigidbody>();
        gunController = GetComponent<GunController>();
        animController = GetComponent<PlayerAnimationController>();
        num_remain_jump = num_max_jump;
    }

    protected override void Move() {
        Vector3 moveDistance = ((playerInput.verticalMove * transform.forward) + (playerInput.horizontalMove * transform.right)).normalized * moveSpeed * Time.deltaTime;
        playerRigidbody.MovePosition(playerRigidbody.position + moveDistance);
        if(on_ground) {
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
                } else {
                    animController.photonView.RPC("ChangeAnimationState", RpcTarget.All, PlayerAnimationController.AnimState.Idle_gunMiddle_AR.ToString(), -1, 0f);
                }
            }
        }
    }

    protected override void Jump() {
        if(playerInput.jump && num_remain_jump > 0) {
            animController.photonView.RPC("ChangeAnimationState", RpcTarget.All, PlayerAnimationController.AnimState.Jump.ToString(), -1, 0f, true);
            playerRigidbody.AddForce(jumpForce * transform.up, ForceMode.Impulse);
            num_remain_jump--;
            on_ground = false;
        }
    }
}
