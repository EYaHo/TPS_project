using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class PlayerMovement : MonoBehaviourPun
{
    public Transform targetOfCam;

    public float moveSpeed = 10f;
    public float jumpForce = 6f;
    public float verticalRotateSpeed = 4f;
    public float horizontalRotateSpeed = 4f;
    private PlayerInput playerInput;
    private Rigidbody playerRigidbody;
    private GunController gunController;
    private PlayerAnimationController animController;

    private float verticalMouseMove = 0f;
    private float horizontalMouseMove = 0f;

    [SerializeField]
    private bool on_ground = false;
    [SerializeField]
    private int num_max_jump = 2;
    [SerializeField]
    private int num_remain_jump;

    void Start()
    {
        playerInput = GetComponent<PlayerInput>();
        playerRigidbody = GetComponent<Rigidbody>();
        gunController = GetComponent<GunController>();
        animController = GetComponent<PlayerAnimationController>();
        num_remain_jump = num_max_jump;
    }

    void Update()
    {
        if(!photonView.IsMine)
        {
            return;
        }

        Rotate();
        Jump();
    }

    private void FixedUpdate() {
        if(!photonView.IsMine)
        {
            return;
        }

        Move();
    }

    private void Move() {
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

    private void Rotate() {
        verticalMouseMove += verticalRotateSpeed * playerInput.verticalRotate;
        horizontalMouseMove += horizontalRotateSpeed * playerInput.horizontalRotate;
        
        if(horizontalMouseMove >= 80f) {
            horizontalMouseMove = 80f;
        } else if(horizontalMouseMove <= -80f) {
            horizontalMouseMove = -80f;
        }

        transform.rotation = Quaternion.Euler(0, verticalMouseMove, 0);
        targetOfCam.rotation = Quaternion.Euler(-1 * horizontalMouseMove, verticalMouseMove, 0);
    }

    private void Jump() {
        if(playerInput.jump && num_remain_jump > 0) {
            animController.photonView.RPC("ChangeAnimationState", RpcTarget.All, PlayerAnimationController.AnimState.Jump.ToString(), -1, 0f, true);
            playerRigidbody.AddForce(jumpForce * transform.up, ForceMode.Impulse);
            num_remain_jump--;
            on_ground = false;
        }
    }

    private void OnCollisionEnter(Collision other) {
        if(other.gameObject.CompareTag("Ground")) {
            RaycastHit hitData;
            if(Physics.Raycast(transform.position, new Vector3(0f, -1f, 0f), out hitData, 2f)) {
                if(hitData.transform.gameObject.CompareTag("Ground")) {
                    on_ground = true;
                    num_remain_jump = num_max_jump;
                    Debug.Log("ground");
                }
            }
        }
    }
}
