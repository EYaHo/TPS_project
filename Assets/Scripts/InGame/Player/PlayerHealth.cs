using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;

public class PlayerHealth : LivingEntity
{
    public Slider healthSlider;

    private PlayerMovement playerMovement;
    private PlayerInteract playerInteract;

    private void Awake() {
        playerMovement = GetComponent<PlayerMovement>();
        playerInteract = GetComponent<PlayerInteract>();
    }

    protected override void OnEnable() {
        base.OnEnable();

        healthSlider.gameObject.SetActive(true);
        healthSlider.maxValue = maxHealth;
        healthSlider.value = health;

        playerMovement.enabled = true;
        playerInteract.enabled = true;
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
        playerMovement.enabled = false;
        playerInteract.enabled = false;
    }
}
