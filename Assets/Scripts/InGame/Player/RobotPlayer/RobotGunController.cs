using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class RobotGunController : GunController
{
    public float bulletLineLifeTime = 0.03f;

    public Transform spine;
    public Transform armPositionLeft;
    public Transform armPositionRight;
    public Transform gunTriggerPoint;

    private LineRenderer bulletLineRenderer;
    [SerializeField]
    private PlayerAnimationController animController;
    
    private Vector3 spineRotationOffset = new Vector3(0, -45, -90);
    private Vector3 armPositionLeftRotationOffset = new Vector3(0, 90, -45);
    private Vector3 armPositionRightRotationOffset = new Vector3(0, 90, -45);

    private void Awake() {
        bulletLineRenderer = GetComponent<LineRenderer>();

        bulletLineRenderer.positionCount = 2;
        bulletLineRenderer.enabled = false;

        animController = gameObject.transform.parent.GetComponent<PlayerAnimationController>();
        gunTriggerPoint.position = armPositionRight.position;
    }
    private void Start() {

    }

    protected override void Update() {/*
        if(!photonView.IsMine)
        {
            return;
        }*/
        base.Update();
        this.gunTriggerPoint.position = this.armPositionRight.position;
    }

    private void LateUpdate() {/*
        if(!photonView.IsMine)
        {
            return;
        }*/
        this.spine.LookAt(this.aimPoint);
        this.spine.rotation = this.spine.rotation * Quaternion.Euler(spineRotationOffset);

        this.armPositionRight.position = this.gunTriggerPoint.position;
        this.armPositionLeft.LookAt(this.aimPoint);
        this.armPositionRight.LookAt(this.aimPoint);
        this.armPositionLeft.rotation = this.armPositionLeft.rotation * Quaternion.Euler(armPositionLeftRotationOffset);
        this.armPositionRight.rotation = this.armPositionRight.rotation * Quaternion.Euler(armPositionRightRotationOffset);

        //this.transform.LookAt(aimPoint);
        //transform.rotation = transform.rotation * Quaternion.Euler(offset);
    }

    protected override void OnEnable() {
        base.OnEnable();
    }

    // 총알 생성
    // 총알의 방향은 총의 방향과 같도록
    public override void Shoot() {
        RaycastHit hit;
        Vector3 hitPosition = Vector3.zero;
        if(Physics.Raycast(muzzle.position, muzzle.forward, out hit, attackRange)) {
            IDamageable target = hit.collider.GetComponent<IDamageable>();

            if(target != null) {
                target.OnDamage(attackDamage, hit.point, hit.normal);
                CreateDamagePopup(hit.point, Camera.main.transform.rotation, (int)attackDamage);
            }

            hitPosition = hit.point;
        } else {
            hitPosition = muzzle.position + muzzle.forward * attackRange;
        }

        StartCoroutine(ShotEffect(hitPosition));
    }

    private IEnumerator ShotEffect(Vector3 hitPosition) {
        bulletLineRenderer.SetPosition(0, muzzle.position);
        bulletLineRenderer.SetPosition(1, hitPosition);
        bulletLineRenderer.enabled = true;

        yield return new WaitForSeconds(bulletLineLifeTime);

        bulletLineRenderer.enabled = false;
    }

    public override void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info) {
        if(stream.IsWriting) {
            stream.SendNext(aimPoint);

            stream.SendNext(spine.rotation);

            // stream.SendNext(armPositionRight.position);
            // stream.SendNext(gunTriggerPoint.position);

            // stream.SendNext(armPositionLeft.rotation);
            // stream.SendNext(armPositionRight.rotation);
        } else {
            aimPoint = (Vector3)stream.ReceiveNext();

            spine.rotation = (Quaternion)stream.ReceiveNext();

            // armPositionRight.position = (Vector3)stream.ReceiveNext();
            // gunTriggerPoint.position = (Vector3)stream.ReceiveNext();

            // armPositionLeft.rotation = (Quaternion)stream.ReceiveNext();
            // armPositionRight.rotation = (Quaternion)stream.ReceiveNext();
        }
    }
}
