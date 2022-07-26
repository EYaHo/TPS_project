using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class TestPlayerGunController : GunController
{
    public GameObject bulletPrefab;

    // 총알 생성
    // 총알의 방향은 총의 방향과 같도록
    public override void Shoot() {
        GameObject createdBullet = PhotonNetwork.Instantiate(bulletPrefab.gameObject.name, muzzle.position, transform.rotation);
        Bullet bullet = createdBullet.GetComponent<Bullet>();
        bullet.photonView.RPC("Setup", RpcTarget.All, attackRange, attackDamage);
    }
}
