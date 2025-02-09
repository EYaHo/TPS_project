using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class TestPlayerGunController : GunController
{
    public Transform gunTransform;

    protected override void Update() {
        if(!photonView.IsMine)
        {
            return;
        }

        aimVector = CalcAimVector();
        photonView.RPC("TraceAim", RpcTarget.All, aimVector);
        Debug.DrawRay(muzzle.position, muzzle.forward * (aimPoint - muzzle.position).magnitude, Color.red);
    }

    public override Vector3 CalcAimVector() {
        RaycastHit hitData;
        Ray ray = Camera.main.ViewportPointToRay(new Vector3 (0.5f, 0.5f, 0));

        if(Physics.Raycast(ray, out hitData, attackRange, layerMask))
        {
            aimPoint = hitData.point;
        } else {
            aimPoint = ray.origin + ray.direction * attackRange;
        }

        return (aimPoint - gunTransform.position).normalized;
    }

    // 모든 클라이언트에서 총알을 생성하도록 ShootProcess를 호출
    public override void Shoot() {
        photonView.RPC("ShootProcess", RpcTarget.All);
    }

    // 총알을 ObjectPool에서 가져와서 초기화
    [PunRPC]
    protected void ShootProcess() {
        Bullet bullet = BulletPool.Instance.GetObject().GetComponent<Bullet>();
        bullet.Setup(attackRange, attackDamage, muzzle.position, transform.rotation, transform.parent.gameObject.GetComponent<PlayerShooter>());
    }

    // aim을 따라 플레이어의 총을 회전시킨다.
    [PunRPC]
    public void TraceAim(Vector3 aimVector) {
        Quaternion preRot = transform.rotation;
        Quaternion nextRot = Quaternion.LookRotation(aimVector);
        Quaternion rot = Quaternion.Slerp(preRot, nextRot, 0.1f);
        
        gunTransform.rotation = rot;
        gunTransform.rotation = Quaternion.LookRotation(aimVector);
    }
}
