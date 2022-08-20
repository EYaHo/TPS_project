using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;

public class LivingEntity : MonoBehaviourPun, IDamageable
{
    public float maxHealth = 100f;
    public float health { get; protected set; }
    public bool dead { get; protected set; }
    public event Action onDeath;
    public Slider healthSlider;
    public float damagePopupXPositionNoise = 0.5f;

    [SerializeField]
    protected Collider collider;

    protected void Start() {
        collider = GetComponent<Collider>();
    }

    // 호스트->모든 클라이언트 방향으로 체력과 사망 상태를 동기화
    [PunRPC]
    public void ApplyUpdatedHealth(float newHealth, bool newDead) {
        health = newHealth;
        dead = newDead;
        healthSlider.value = health;
    }

    protected virtual void OnEnable() {
        dead = false;
        health = maxHealth;
        healthSlider.gameObject.SetActive(true);
        healthSlider.maxValue = maxHealth;
        healthSlider.value = health;
    }

    [PunRPC]
    public virtual void OnDamage(float damage, Vector3 hitPoint) {
        if(PhotonNetwork.IsMasterClient) {
            health -= damage;
            photonView.RPC("ApplyUpdatedHealth", RpcTarget.Others, health, dead);
            photonView.RPC("OnDamage", RpcTarget.Others, damage, hitPoint);
        }

        healthSlider.value = health;
        photonView.RPC("CreateDamagePopup", RpcTarget.All, hitPoint, (int)damage);

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

        healthSlider.value = health;
    }

    public virtual void Die() {
        if(onDeath != null) {
            onDeath();
        }

        dead = true;
        Debug.Log("Enemy Die function called");

        healthSlider.gameObject.SetActive(false);
        collider.enabled = false;
    }

    // 데미지를 받은 위치에 데미지 팝업을 표시하는 함수
    [PunRPC]
    public void CreateDamagePopup(Vector3 position, int damageAmount) {
        Vector3 noiseVector = new Vector3(UnityEngine.Random.Range(-damagePopupXPositionNoise, damagePopupXPositionNoise), 0f, 0f);
        DamagePopup damagePopup = DamagePopupPool.Instance.GetObject().GetComponent<DamagePopup>();
        damagePopup.Setup(position + noiseVector);
        damagePopup.SetDamageAmount(damageAmount);
    }
}
