using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Photon.Pun;

public class PlayerMovement : MonoBehaviourPun
{
    public Transform targetOfCam;
    public LayerMask groundLayer;

    public float moveSpeed = 10f;
    public float maxSpeedOnGround = 10f;
    public float movementSharpnessOnGround = 15f;
    public float maxSpeedInAir = 5f;
    public float AccelerationSpeedInAir = 3f;
    public float jumpForce = 6f;
    public float verticalRotateSpeed = 1f;
    public float horizontalRotateSpeed = 1f;
    // protected PlayerInput playerInput;
    // protected Rigidbody rigid;
    protected GunController gunController;

    protected float verticalMouseMove = 0f;
    protected float horizontalMouseMove = 0f;

    private float maxHorizontalMouseMove = 50f;
    private float minHorizontalMouseMove = -75f;
    private const float k_Gravity = 9.81f;

    protected Vector3 moveInput;
    private Vector3 groundNormal;
    private bool jumpInputDown = false;
    public CharacterController characterController;
    // public PlayerInputs playerInput;

    [SerializeField]
    protected int numMaxJump = 2;
    [SerializeField]
    protected int numRemainJump;
    [SerializeField]

    public Vector3 characterVelocity { get; private set; }

    protected virtual void Start()
    {
        // playerInput = GetComponent<PlayerInput>();
        // rigid = GetComponent<Rigidbody>();
        gunController = GetComponent<GunController>();
        characterController = GetComponent<CharacterController>();
        characterController.enableOverlapRecovery = true;
        numRemainJump = numMaxJump;
    }

    void Update()
    {
        if(!photonView.IsMine)
        {
            return;
        }

        Rotate();
        // if(playerInput.jump && numRemainJump > 0) {
        //     photonView.RPC("RpcJump", RpcTarget.All);
        // }
    }

    private void FixedUpdate() {
        if(!photonView.IsMine)
        {
            return;
        }

        Move();
        // Jump();
        // Rotate();
    }

    private void GroundCheck() {
        // float chosenGroundCheckDistance = isGrounded ? (characterController.skinWidth + groundCheckDistance) : k_GroundCheckDistanceInAir;

        // isGrounded = false;
        // groundNormal = Vector3.up;

        // if(Time.time >= lastTimeJumped + k_JumpGroundingPreventionTime) {
        //     if(Physics.CapsuleCast(GetCapsuleBottomHemisphere(), GetCapsuleTopHemisphere()))

                // Vector3 spherePosition = new Vector3(transform.position.x, transform.position.y - GroundedOffset,
                //     transform.position.z);
                // Grounded = Physics.CheckSphere(spherePosition, GroundedRadius, GroundLayers,
                //     QueryTriggerInteraction.Ignore);
        // }
    }

    protected virtual void Move() {
        Vector3 worldspaceMoveInput = transform.forward * moveInput.y + transform.right * moveInput.x;
        // if(isGrounded) {
        if(characterController.isGrounded) {
            Vector3 targetVelocity = worldspaceMoveInput * maxSpeedOnGround;
            // targetVelocity = GetDirectionalReorientedOnSlope(targetVelocity.normalized, groundNormal) * targetVelocity.magnitude;
            characterVelocity = Vector3.Lerp(characterVelocity, targetVelocity, movementSharpnessOnGround * Time.deltaTime);
            if(jumpInputDown) {
                characterVelocity = new Vector3(characterVelocity.x, 0f, characterVelocity.z);
                characterVelocity += Vector3.up * jumpForce;
                groundNormal = Vector3.up;
                numRemainJump--;
            }
        } else {
            characterVelocity += worldspaceMoveInput * AccelerationSpeedInAir * Time.deltaTime;

            float verticalVelocity = characterVelocity.y;
            Vector3 horizontalVelocity = Vector3.ProjectOnPlane(characterVelocity, Vector3.up);
            horizontalVelocity = Vector3.ClampMagnitude(horizontalVelocity, maxSpeedInAir);
            characterVelocity = horizontalVelocity + (Vector3.up * verticalVelocity);

            characterVelocity += Vector3.down * k_Gravity * Time.deltaTime;
        }

        characterController.Move(characterVelocity * Time.deltaTime);
    }
    
    protected virtual void Rotate() {
        // verticalMouseMove += verticalRotateSpeed * playerInput.verticalRotate;
        // horizontalMouseMove += horizontalRotateSpeed * playerInput.horizontalRotate;

        // horizontalMouseMove = Mathf.Max(horizontalMouseMove, minHorizontalMouseMove);
        // horizontalMouseMove = Mathf.Min(horizontalMouseMove, maxHorizontalMouseMove);

        transform.rotation = Quaternion.Euler(0, verticalMouseMove, 0);
        targetOfCam.rotation = Quaternion.Euler(-1 * horizontalMouseMove, verticalMouseMove, 0);
    }

    public void OnMove(InputAction.CallbackContext context) {
        moveInput = context.ReadValue<Vector2>().normalized;
    }

    public void OnJump(InputAction.CallbackContext context) {

        switch(context.phase) {
            case InputActionPhase.Performed:
                jumpInputDown = true;
                break;
            default:
                jumpInputDown = false;
                break;
        }
        // if(context.performed && numRemainJump > 0) {
        //     photonView.RPC("RpcJump", RpcTarget.All);
        // }
    }

    public void OnRotate(InputAction.CallbackContext context) {
        Vector2 input = context.ReadValue<Vector2>();
        verticalMouseMove += verticalRotateSpeed * input.x;
        horizontalMouseMove += horizontalRotateSpeed * input.y;

        horizontalMouseMove = Mathf.Max(horizontalMouseMove, minHorizontalMouseMove);
        horizontalMouseMove = Mathf.Min(horizontalMouseMove, maxHorizontalMouseMove);
    }

    [PunRPC]
    protected void RpcJump() {
        // rigid.AddForce(jumpForce * transform.up, ForceMode.Impulse);
        numRemainJump--;
    }

    protected virtual void OnCollisionEnter(Collision other) {
        if(other.gameObject.CompareTag("Ground")) {
            if(Physics.CheckSphere(transform.position, .1f, groundLayer)) {
                numRemainJump = numMaxJump;
                Debug.Log("ground");
            }
        }
    }

    // 슬로프에서의 이동 방향 계산
    public Vector3 GetDirectionalReorientedOnSlope(Vector3 direction, Vector3 slopeNormal) {
        Vector3 directionalRight = Vector3.Cross(direction, transform.up);
        return Vector3.Cross(slopeNormal, directionalRight).normalized;
    }
}
