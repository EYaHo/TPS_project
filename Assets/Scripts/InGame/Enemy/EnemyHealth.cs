using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Cinemachine;

public class EnemyHealth : LivingEntity
{
    [SerializeField]
    private CinemachineVirtualCamera virtualCam;
    private EnemyMovement enemyMovement;

    private void Awake() {
        enemyMovement = GetComponent<EnemyMovement>();
        virtualCam = GameObject.FindWithTag("MainCamera").GetComponent<CinemachineVirtualCamera>();
    }

    private void LateUpdate() {
        healthSlider.transform.LookAt(healthSlider.transform.position + virtualCam.transform.rotation * Vector3.back, virtualCam.transform.rotation * Vector3.up);
    }

    [PunRPC]
    public override void OnDamage(float damage, Vector3 hitPoint, Vector3 hitNormal) {
        base.OnDamage(damage, hitPoint, hitNormal);
        Debug.Log("enemy damaged " + damage);
    }

    public override void Die() {
        base.Die();

        enemyMovement.enabled = false;
        Debug.Log("Test");
    }
}
