using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class GunController : MonoBehaviourPun
{
    [SerializeField]
    private Transform muzzle;
    public GameObject bulletPrefab;

    [SerializeField]
    LayerMask layerMask;
    
    private Camera cam;

    public float attackRange = 30f;
    public float fireTimeInterval = 1f;
    private float lastFireTime;

    private Vector3 aimVector;

    private void Awake() {
        muzzle = transform.GetChild(1);

        cam = GameObject.FindWithTag("MainCamera").GetComponent<Camera>();
    }

    private void Update() {
        if(!photonView.IsMine)
        {
            return;
        }

        Debug.DrawRay(transform.position, aimVector * 20f, Color.red);
        CalcAimVector();
        TraceAim();
    }

    private void OnEnable() {
        lastFireTime = 0;
    }

    public void CalcAimVector() {
        RaycastHit hitData;
        Ray ray = cam.ViewportPointToRay(new Vector3 (0.5f, 0.5f, 0));
        Vector3 aimPoint;

        if(Physics.Raycast(ray, out hitData, attackRange, layerMask))
        {
            aimPoint = hitData.point;
        }
        else
        {
            aimPoint = ray.origin + ray.direction * attackRange;
        }
        
        aimVector = (aimPoint - transform.position).normalized;
    }

    // aim을 따라 플레이어의 총을 회전시킨다.
    public void TraceAim() {
        Quaternion preRot = transform.rotation;
        Quaternion nextRot = Quaternion.LookRotation(aimVector);
        Quaternion rot = Quaternion.Slerp(preRot, nextRot, 0.1f);
        
        transform.rotation = rot;
    }

    // 총알 생성
    // 총알의 방향은 총의 방향과 같도록
    public void Shoot() {
        GameObject createdBullet = PhotonNetwork.Instantiate(bulletPrefab.gameObject.name, muzzle.position, transform.rotation);
        Bullet bullet = createdBullet.GetComponent<Bullet>();
        bullet.photonView.RPC("Setup", RpcTarget.All, attackRange);
    }

    public void Fire()
    {
        if(Time.time >= lastFireTime + fireTimeInterval)
        {
            lastFireTime = Time.time;
            Shoot();
        }
    }
}
