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

    [SerializeField]
    public Transform damagePopupPrefab;

    public Transform canvasTransform;
    public float attackRange = 30f;
    public float attackDamage = 10f;
    public float fireTimeInterval = 1f;
    public float damagePopupXPositionNoise = 0.5f;
    protected float lastFireTime;
    protected Vector3 aimVector;
    public Vector3 aimPoint { get; protected set; }

    protected virtual void Update() {
        if(!photonView.IsMine)
        {
            return;
        }

        aimVector = CalcAimVector();
        //Debug.DrawRay(muzzle.position, aimVector * (aimPoint - muzzle.position).magnitude, Color.red);
        Debug.DrawRay(muzzle.position, muzzle.forward * (aimPoint - muzzle.position).magnitude, Color.red);
        TraceAim(aimVector);
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

    // aim을 따라 플레이어의 총을 회전시킨다.
    public void TraceAim(Vector3 aimVector) {
        Quaternion preRot = transform.rotation;
        Quaternion nextRot = Quaternion.LookRotation(aimVector);
        Quaternion rot = Quaternion.Slerp(preRot, nextRot, 0.1f);
        
        //transform.rotation = rot;
        //transform.rotation = Quaternion.LookRotation(aimVector);

        //muzzle.rotation = Quaternion.LookRotation(aimVector);
    }

    public virtual void Shoot() {
        
    }

    [PunRPC]
    public void CreateDamagePopup(Vector3 position, Quaternion rotation, int damageAmount) {
        Vector3 noiseVector = new Vector3(Random.Range(-damagePopupXPositionNoise, damagePopupXPositionNoise), 0f, 0f);
        Transform damagePopupTransform = Instantiate(damagePopupPrefab, position + noiseVector, rotation);
        // Transform damagePopupTransform = Instantiate(damagePopupPrefab);
        // Debug.Log("instantiate position: " + damagePopupTransform.position);
        // Debug.Log("input position: " + position);
        // damagePopupTransform.localPosition = Camera.main.WorldToScreenPoint(position);
        // damagePopupTransform.SetParent(canvasTransform);
        // Debug.Log("local position: " + damagePopupTransform.localPosition);

        DamagePopup damagePopup = damagePopupTransform.GetComponent<DamagePopup>();
        damagePopup.Setup(damageAmount);
        
    }

    public void Fire()
    {
        if(Time.time >= lastFireTime + fireTimeInterval)
        {
            lastFireTime = Time.time;
            Shoot();
        }
    }
    
    [PunRPC]
    public void RpcFire()
    {
        //Fire();
        Debug.Log("RpcFire");
    }

    public virtual void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info) {
        // if(stream.IsWriting) {
        //     stream.SendNext(aimPoint);
        // } else {
        //     aimPoint = (Vector3)stream.ReceiveNext();
        // }
    }

}
