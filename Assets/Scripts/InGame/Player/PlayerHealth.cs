using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;

public class PlayerHealth : LivingEntity
{
    private PlayerMovement playerMovement;
    private PlayerInteract playerInteract;

    private void Awake() {
        playerMovement = GetComponent<PlayerMovement>();
        playerInteract = GetComponent<PlayerInteract>();
    }

    protected override void OnEnable() {
        base.OnEnable();

        playerMovement.enabled = true;
        playerInteract.enabled = true;
    }

    public override void Die() {
        base.Die();

        playerMovement.enabled = false;
        playerInteract.enabled = false;
    }
}
