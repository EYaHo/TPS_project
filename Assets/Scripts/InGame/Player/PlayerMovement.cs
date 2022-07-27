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
                    on_ground = false;
                    num_remain_jump = num_max_jump;
                }
            }
        }
    }
}
