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

    protected GunController gunController;
    protected PlayerInputManager playerInputManager;
    protected CharacterController characterController;

    protected float verticalMouseMove = 0f;
    protected float horizontalMouseMove = 0f;

    private float maxHorizontalMouseMove = 50f;
    private float minHorizontalMouseMove = -75f;
    private const float k_Gravity = 9.81f;

    private Vector3 groundNormal;

    [SerializeField]
    protected int numMaxJump = 2;
    [SerializeField]
    protected int numRemainJump;
    [SerializeField]

    public Vector3 characterVelocity { get; private set; }

    protected virtual void Start()
    {
        playerInputManager = GetComponent<PlayerInputManager>();
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
    }

    private void FixedUpdate() {
        if(!photonView.IsMine)
        {
            return;
        }

        Move();
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
        Vector3 _worldspaceMoveInput = transform.forward * playerInputManager.move.y + transform.right * playerInputManager.move.x;
        if(characterController.isGrounded) {
            Vector3 _targetVelocity = _worldspaceMoveInput * maxSpeedOnGround;
            characterVelocity = Vector3.Lerp(characterVelocity, _targetVelocity, movementSharpnessOnGround * Time.deltaTime);
            if(playerInputManager.jump) {
                characterVelocity = new Vector3(characterVelocity.x, 0f, characterVelocity.z);
                characterVelocity += Vector3.up * jumpForce;
                groundNormal = Vector3.up;
                numRemainJump--;
            }
        } else {
            characterVelocity += _worldspaceMoveInput * AccelerationSpeedInAir * Time.deltaTime;

            float _verticalVelocity = characterVelocity.y;
            Vector3 _horizontalVelocity = Vector3.ProjectOnPlane(characterVelocity, Vector3.up);
            _horizontalVelocity = Vector3.ClampMagnitude(_horizontalVelocity, maxSpeedInAir);
            characterVelocity = _horizontalVelocity + (Vector3.up * _verticalVelocity);

            characterVelocity += Vector3.down * k_Gravity * Time.deltaTime;
        }

        characterController.Move(characterVelocity * Time.deltaTime);
    }
    
    protected virtual void Rotate() {
        verticalMouseMove += verticalRotateSpeed * playerInputManager.look.x;
        horizontalMouseMove += horizontalRotateSpeed * playerInputManager.look.y;

        horizontalMouseMove = Mathf.Max(horizontalMouseMove, minHorizontalMouseMove);
        horizontalMouseMove = Mathf.Min(horizontalMouseMove, maxHorizontalMouseMove);

        transform.rotation = Quaternion.Euler(0, verticalMouseMove, 0);
        targetOfCam.rotation = Quaternion.Euler(-1 * horizontalMouseMove, verticalMouseMove, 0);
    }

    [PunRPC]
    protected void RpcJump() {
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
