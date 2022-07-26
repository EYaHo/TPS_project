using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class GunController : MonoBehaviourPun
{
    [SerializeField]
    protected Transform muzzle;

    [SerializeField]
    LayerMask layerMask;
    
    [SerializeField]
    private Camera cam;

    public float attackRange = 30f;
    public float attackDamage = 10f;
    public float fireTimeInterval = 1f;
    private float lastFireTime;

    private void Start() {
        cam = GameObject.FindWithTag("MainCamera").GetComponent<Camera>();
    }

    private void Update() {
        if(!photonView.IsMine)
        {
            return;
        }

        Vector3 aimVector = CalcAimVector();
        Debug.DrawRay(transform.position, aimVector * 20f, Color.red);
        TraceAim(aimVector);
    }

    private void OnEnable() {
        lastFireTime = 0;
    }

    public Vector3 CalcAimVector() {
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
        
        return (aimPoint - transform.position).normalized;
    }

    // aim을 따라 플레이어의 총을 회전시킨다.
    public void TraceAim(Vector3 aimVector) {
        Quaternion preRot = transform.rotation;
        Quaternion nextRot = Quaternion.LookRotation(aimVector);
        Quaternion rot = Quaternion.Slerp(preRot, nextRot, 0.1f);
        
        transform.rotation = rot;
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
}
