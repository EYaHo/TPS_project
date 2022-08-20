using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class GunController : MonoBehaviourPun, IPunObservable
{
    [SerializeField]
    protected Transform muzzle;

    [SerializeField]
    protected LayerMask layerMask;

    public float attackRange = 30f;
    public float attackDamage = 10f;
    public float fireTimeInterval = 1f;
    protected float lastFireTime;
    protected Vector3 aimVector;
    public Vector3 aimPoint { get; protected set; }

    protected virtual void Update() {
        if(!photonView.IsMine)
        {
            return;
        }

        aimVector = CalcAimVector();
        Debug.DrawRay(muzzle.position, muzzle.forward * (aimPoint - muzzle.position).magnitude, Color.red);
    }

    protected virtual void OnEnable() {
        lastFireTime = 0f;
    }

    // 화면의 중앙으로 향하는 에임 벡터를 계산
    public virtual Vector3 CalcAimVector() {
        RaycastHit hitData;
        Ray ray = Camera.main.ViewportPointToRay(new Vector3 (0.5f, 0.5f, 0));

        if(Physics.Raycast(ray, out hitData, attackRange, layerMask))
        {
            aimPoint = hitData.point;
        } else {
            aimPoint = ray.origin + ray.direction * attackRange;
        }

        return (aimPoint - muzzle.position).normalized;
    }

    public virtual void Shoot() {
        
    }

    public void Fire()
    {
        if(Time.time >= lastFireTime + fireTimeInterval)
        {
            lastFireTime = Time.time;
            Shoot();
        }
    }

    public virtual void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info) {
        if(stream.IsWriting) {
            stream.SendNext(aimPoint);
        } else {
            aimPoint = (Vector3)stream.ReceiveNext();
        }
    }

}
