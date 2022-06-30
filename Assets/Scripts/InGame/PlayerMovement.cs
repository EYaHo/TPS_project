using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class PlayerMovement : MonoBehaviourPun
{
    public float moveSpeed = 10f;
    public float jumpForce = 100f;
    private PlayerInput playerInput;
    private Rigidbody playerRigidbody;

    private bool is_jumping = false;

    void Start()
    {
        playerInput = GetComponent<PlayerInput>();
        playerRigidbody = GetComponent<Rigidbody>();
    }

    void Update()
    {
        
    }

    private void FixedUpdate() {
        if(!photonView.IsMine)
        {
            return;
        }

        Rotate();
        Move();
        Jump();
    }

    private void Move() {
        Vector3 moveDistance = ((playerInput.verticalMove * transform.forward) + (playerInput.horizontalMove * transform.right)).normalized * moveSpeed * Time.deltaTime;
        playerRigidbody.MovePosition(playerRigidbody.position + moveDistance);
    }

    private void Rotate() {

    }

    private void Jump() {
        if(is_jumping == false && playerInput.jump) {
            playerRigidbody.AddForce(jumpForce * transform.up);
            is_jumping = true;
        }
    }

    private void OnCollisionEnter(Collision collision) {
        // 땅하고 닿으면 is_jumping = false;
        // 발에 닿은 경우에만 !!!
    }
}
