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
        TraceAim(aimVector);
        Debug.DrawRay(muzzle.position, muzzle.forward * (aimPoint - muzzle.position).magnitude, Color.red);
    }

    // 서버에서 Shoot을 처리하도록 넘김
    public override void Shoot() {
        photonView.RPC("ShootProcessOnServer", RpcTarget.MasterClient);
    }

    // 서버에서 Shoot을 처리
    // 총알을 ObjectPool에서 가져와서 초기화
    [PunRPC]
    protected override void ShootProcessOnServer() {
        Bullet bullet = BulletPool.Instance.GetBullet().GetComponent<Bullet>();
        bullet.Setup(attackRange, attackDamage, muzzle.position, transform.rotation, transform.parent.gameObject.GetComponent<Player>());
    }

    // aim을 따라 플레이어의 총을 회전시킨다.
    public void TraceAim(Vector3 aimVector) {
        Quaternion preRot = transform.rotation;
        Quaternion nextRot = Quaternion.LookRotation(aimVector);
        Quaternion rot = Quaternion.Slerp(preRot, nextRot, 0.1f);
        
        gunTransform.rotation = rot;
        gunTransform.rotation = Quaternion.LookRotation(aimVector);
    }
}
