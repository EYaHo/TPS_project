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
    [SerializeField]
    private AnimationController animController;

    private void Awake() {
        enemyMovement = GetComponent<EnemyMovement>();
        animController = GetComponent<AnimationController>();
    }

    private void LateUpdate() {
        healthSlider.transform.LookAt(healthSlider.transform.position + Camera.main.transform.rotation * Vector3.back, Camera.main.transform.rotation * Vector3.up);
    }

    [PunRPC]
    public override void OnDamage(float damage, Vector3 hitPoint) {
        base.OnDamage(damage, hitPoint);
    }

    public override void Die() {
        base.Die();
        animController.PlayDieAnimation();
        enemyMovement.enabled = false;

        if(GetComponent<Enemy>().hasSpawnPosition) {
            //respawn;
            Debug.Log("execute respawn");
        } else {
            Debug.Log("It doesn't have SpawnPosition.");
        }
    }
}
