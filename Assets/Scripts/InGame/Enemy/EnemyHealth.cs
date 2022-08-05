using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Cinemachine;

public class EnemyHealth : LivingEntity
{
    [SerializeField]
    private EnemyMovement enemyMovement;

    private void Awake() {
        enemyMovement = GetComponent<EnemyMovement>();
    }

    private void LateUpdate() {
        healthSlider.transform.LookAt(healthSlider.transform.position + Camera.main.transform.rotation * Vector3.back, Camera.main.transform.rotation * Vector3.up);
    }

    [PunRPC]
    public override void OnDamage(float damage, Vector3 hitPoint, Vector3 hitNormal) {
        base.OnDamage(damage, hitPoint, hitNormal);
    }

    public override void Die() {
        base.Die();

        enemyMovement.enabled = false;
    }
}
