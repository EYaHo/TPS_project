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
    protected PlayerInput playerInput;
    protected Rigidbody playerRigidbody;
    protected GunController gunController;
    //private PlayerAnimationController animController;

    protected float verticalMouseMove = 0f;
    protected float horizontalMouseMove = 0f;

    [SerializeField]
    protected bool on_ground = false;
    [SerializeField]
    protected int num_max_jump = 2;
    [SerializeField]
    protected int num_remain_jump;

    protected virtual void Start()
    {
        playerInput = GetComponent<PlayerInput>();
        playerRigidbody = GetComponent<Rigidbody>();
        gunController = GetComponent<GunController>();
        num_remain_jump = num_max_jump;
        //animController = GetComponent<PlayerAnimationController>();
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

    protected virtual void Move() {
        Vector3 moveDistance = ((playerInput.verticalMove * transform.forward) + (playerInput.horizontalMove * transform.right)).normalized * moveSpeed * Time.deltaTime;
        playerRigidbody.MovePosition(playerRigidbody.position + moveDistance);
    }

    protected virtual void Rotate() {
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

    protected virtual void Jump() {
        if(playerInput.jump && num_remain_jump > 0) {
            playerRigidbody.AddForce(jumpForce * transform.up, ForceMode.Impulse);
            num_remain_jump--;
            on_ground = false;
        }
    }

    protected virtual void OnCollisionEnter(Collision other) {
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
