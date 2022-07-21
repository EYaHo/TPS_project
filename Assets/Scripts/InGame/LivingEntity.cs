using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class LivingEntity : MonoBehaviourPun, IDamageable
{
    public float maxHealth = 100f;
    [SerializeField]
    public float health { get; protected set; }
    [SerializeField]
    public bool dead { get; protected set; }
    public event Action onDeath;

    // 호스트->모든 클라이언트 방향으로 체력과 사망 상태를 동기화
    [PunRPC]
    public void ApplyUpdatedHealth(float newHealth, bool newDead) {
        health = newHealth;
        dead = newDead;
    }

    protected virtual void OnEnable() {
        dead = false;
        health = maxHealth;
    }

    [PunRPC]
    public virtual void OnDamage(float damage, Vector3 hitPoint, Vector3 hitNormal) {
        if(PhotonNetwork.IsMasterClient) {
            health -= damage;
            photonView.RPC("ApplyUpdatedHealth", RpcTarget.Others, health, dead);
            photonView.RPC("OnDamage", RpcTarget.Others, damage, hitPoint, hitNormal);
        }

        if(health <= 0 && !dead) {
            Die();
        }
    }

    [PunRPC]
    public virtual void RestoreHealth(float newHealth) {
        if(dead) {
            return;
        }

        if(PhotonNetwork.IsMasterClient) {
            health += newHealth;
            health = Mathf.Min(health, maxHealth);
            photonView.RPC("ApplyUpdatedHealth", RpcTarget.Others, health, dead);
        }
    }

    public virtual void Die() {
        if(onDeath != null) {
            onDeath();
        }

        dead = true;
    }
}
