using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class PlayerMovement : MonoBehaviourPun
{
    public Transform targetOfCam;
    public LayerMask groundLayer;

    public float moveSpeed = 10f;
    public float jumpForce = 6f;
    public float verticalRotateSpeed = 4f;
    public float horizontalRotateSpeed = 4f;
    protected PlayerInput playerInput;
    protected Rigidbody playerRigidbody;
    protected GunController gunController;

    protected float verticalMouseMove = 0f;
    protected float horizontalMouseMove = 0f;

    private float maxHorizontalMouseMove = 50f;
    private float minHorizontalMouseMove = -45f;

    [SerializeField]
    protected int numMaxJump = 2;
    [SerializeField]
    protected int numRemainJump;
    [SerializeField]
    protected bool isGrounded = false;

    protected virtual void Start()
    {
        playerInput = GetComponent<PlayerInput>();
        playerRigidbody = GetComponent<Rigidbody>();
        gunController = GetComponent<GunController>();
        numRemainJump = numMaxJump;
    }

    void Update()
    {
        if(!photonView.IsMine)
        {
            return;
        }

        Rotate();
    }

    private void FixedUpdate() {
        if(!photonView.IsMine)
        {
            return;
        }

        Move();
        //Jump();
        if(playerInput.jump && numRemainJump > 0) {
            photonView.RPC("RpcJump", RpcTarget.All);
        }
    }

    protected virtual void Move() {
        Vector3 moveDistance = ((playerInput.verticalMove * transform.forward) + (playerInput.horizontalMove * transform.right)).normalized * moveSpeed * Time.deltaTime;
        playerRigidbody.MovePosition(playerRigidbody.position + moveDistance);
    }

    protected virtual void Rotate() {
        verticalMouseMove += verticalRotateSpeed * playerInput.verticalRotate;
        horizontalMouseMove += horizontalRotateSpeed * playerInput.horizontalRotate;

        horizontalMouseMove = Mathf.Max(horizontalMouseMove, minHorizontalMouseMove);
        horizontalMouseMove = Mathf.Min(horizontalMouseMove, maxHorizontalMouseMove);

        transform.rotation = Quaternion.Euler(0, verticalMouseMove, 0);
        targetOfCam.rotation = Quaternion.Euler(-1 * horizontalMouseMove, verticalMouseMove, 0);
    }

    protected virtual void Jump() {
        if(playerInput.jump && numRemainJump > 0) {
            playerRigidbody.AddForce(jumpForce * transform.up, ForceMode.Impulse);
            isGrounded = false;
            numRemainJump--;
        }
    }
    [PunRPC]
    protected void RpcJump() {
        playerRigidbody.AddForce(jumpForce * transform.up, ForceMode.Impulse);
        isGrounded = false;
        numRemainJump--;
    }

    protected virtual void OnCollisionEnter(Collision other) {
        if(other.gameObject.CompareTag("Ground")) {
            if(Physics.CheckSphere(transform.position, .1f, groundLayer)) {
                isGrounded = true;
                numRemainJump = numMaxJump;
                Debug.Log("ground");
            }
        }
    }
}
