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

        if(on_ground) {
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

    protected override void Rotate() {
        verticalMouseMove += verticalRotateSpeed * playerInput.verticalRotate;
        horizontalMouseMove += horizontalRotateSpeed * playerInput.horizontalRotate;
        
        if(horizontalMouseMove >= 70f) {
            horizontalMouseMove = 70f;
        } else if(horizontalMouseMove <= -90f) {
            horizontalMouseMove = -90f;
        }

        transform.rotation = Quaternion.Euler(0, verticalMouseMove, 0);
        targetOfCam.rotation = Quaternion.Euler(-1 * horizontalMouseMove, verticalMouseMove, 0);
    }

    protected override void Jump() {
        if(playerInput.jump && num_remain_jump > 0) {
            animController.photonView.RPC("ChangeAnimationState", RpcTarget.All, PlayerAnimationController.AnimState.Jump.ToString(), -1, 0f, true);
            playerRigidbody.AddForce(jumpForce * transform.up, ForceMode.Impulse);
            num_remain_jump--;
            on_ground = false;
        }
    }

    protected override void OnCollisionEnter(Collision other) {
        if(other.gameObject.CompareTag("Ground")) {
            on_ground = true;
            Debug.Log("ground");
            RaycastHit hitData;
            if(Physics.Raycast(transform.position, new Vector3(0f, -1f, 0f), out hitData, 2f)) {
                if(hitData.transform.gameObject.CompareTag("Ground")) {
                    //on_ground = true;
                    num_remain_jump = num_max_jump;
                }
            }
        }
    }
}
