using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class GunController : MonoBehaviour
{
    [SerializeField]
    private GameObject playerGun;
    private Transform muzzle;
    public GameObject bulletPrefab;

    [SerializeField]
    LayerMask layerMask;
    
    private CameraSetup cameraSetup;
    private Camera cam;

    public float attackRange;
    float currentFireRate;

    private void Awake() {
        muzzle = playerGun.transform.GetChild(1);

        cameraSetup = GetComponent<CameraSetup>();
        cam = cameraSetup.followCam.gameObject.GetComponent<Camera>();
        attackRange = 30f;
        currentFireRate = 1f;
    }
    private void Start() {

    }

    public Vector3 GetAimPoint() {
        Vector3 aimPoint;
        RaycastHit hitData;
        Ray ray = cam.ViewportPointToRay(new Vector3 (0.5f, 0.5f, 0));

        if(Physics.Raycast(ray, out hitData, attackRange, layerMask))
        {
            aimPoint = hitData.point;
            //Debug.Log(hitData.transform.name+", "+aimPoint);
        }
        else
        {
            aimPoint = ray.origin + ray.direction * attackRange;
            //Debug.Log("null, "+aimPoint);
        }
        
        return aimPoint;
    }

    public void TraceAim(Vector3 aimPoint) {
        //Vector3 aimPoint = GetAimPoint();
        Vector3 direction = (aimPoint - playerGun.transform.position).normalized;

        Quaternion preRot = playerGun.transform.rotation;
        Quaternion nextRot = Quaternion.LookRotation(direction);

        Quaternion rot = Quaternion.Slerp(preRot, nextRot, 0.1f);
        
        playerGun.transform.rotation = rot;
    }

    public void Shoot(Vector3 aimPoint) {
        //Vector3 aimPoint = GetAimPoint();
        GameObject createdBullet = PhotonNetwork.Instantiate(bulletPrefab.gameObject.name, muzzle.position, muzzle.rotation);
        Bullet bullet = createdBullet.GetComponent<Bullet>();
        bullet.photonView.RPC("Setup", RpcTarget.All, aimPoint, attackRange);
        currentFireRate = 1f;
    }

    public void Fire(Vector3 aimPoint)
    {
        if(currentFireRate <= 0)
        {
            Shoot(aimPoint);
        }
    }

    public void GunFireRateCalc()
    {
        if (currentFireRate > 0)
            currentFireRate -= Time.deltaTime;
    }
}
