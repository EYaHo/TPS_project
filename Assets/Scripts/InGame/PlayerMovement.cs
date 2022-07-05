using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class PlayerMovement : MonoBehaviourPun
{
    public float moveSpeed = 10f;
    public float jumpForce = 100f;
    public float verticalRotateSpeed = 10f;
    public float horizontalRotateSpeed = 2f;
    private PlayerInput playerInput;
    private Rigidbody playerRigidbody;

    private float verticalMouseMove = 0f;

    [SerializeField]
    private bool is_jumping = false;
    [SerializeField]
    private int num_max_jump = 2;
    [SerializeField]
    private int num_remain_jump;

    void Start()
    {
        playerInput = GetComponent<PlayerInput>();
        playerRigidbody = GetComponent<Rigidbody>();
        num_remain_jump = num_max_jump;
    }

    void Update()
    {
        Jump();
    }

    private void FixedUpdate() {
        if(!photonView.IsMine)
        {
            return;
        }

        Rotate();
        Move();
    }

    private void Move() {
        Vector3 moveDistance = ((playerInput.verticalMove * transform.forward) + (playerInput.horizontalMove * transform.right)).normalized * moveSpeed * Time.deltaTime;
        playerRigidbody.MovePosition(playerRigidbody.position + moveDistance);
    }

    private void Rotate() {
        verticalMouseMove += verticalRotateSpeed * playerInput.verticalRotate;
        transform.rotation = Quaternion.Euler(0, verticalMouseMove, 0);
    }

    private void Jump() {
        if(playerInput.jump && num_remain_jump > 0) {
            playerRigidbody.AddForce(jumpForce * transform.up, ForceMode.Impulse);
            num_remain_jump--;
            is_jumping = true;
        }
    }

    private void OnCollisionEnter(Collision collision) {
        if(collision.gameObject.CompareTag("Ground")) {
            is_jumping = false;
            num_remain_jump = num_max_jump;
        }
        // 땅하고 닿으면 is_jumping = false;
        // 발에 닿은 경우에만 !!!
    }
}
