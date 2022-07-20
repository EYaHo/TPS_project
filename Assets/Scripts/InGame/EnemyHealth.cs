using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Cinemachine;

public class EnemyHealth : LivingEntity
{
    public Slider healthSlider;

    [SerializeField]
    private CinemachineVirtualCamera virtual_cam;
    private EnemyMovement enemyMovement;

    private void Awake() {
        enemyMovement = GetComponent<EnemyMovement>();
        virtual_cam = GameObject.FindWithTag("MainCamera").GetComponent<CinemachineVirtualCamera>();
    }

    private void Update() {
        healthSlider.transform.LookAt(healthSlider.transform.position + virtual_cam.transform.rotation * Vector3.back, virtual_cam.transform.rotation * Vector3.up);
    }

    protected override void OnEnable() {
        base.OnEnable();

        healthSlider.gameObject.SetActive(true);
        healthSlider.maxValue = maxHealth;
        healthSlider.value = health;
    }

    [PunRPC]
    public override void RestoreHealth(float newHealth) {
        base.RestoreHealth(newHealth);
        healthSlider.value = health;
    }

    [PunRPC]
    public override void OnDamage(float damage, Vector3 hitPoint, Vector3 hitDirection) {
        base.OnDamage(damage, hitPoint, hitDirection);
        healthSlider.value = health;
    }

    public override void Die() {
        base.Die();

        healthSlider.gameObject.SetActive(false);
        enemyMovement.enabled = false;
    }
}
