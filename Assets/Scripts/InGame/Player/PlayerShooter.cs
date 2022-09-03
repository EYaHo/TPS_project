using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class PlayerShooter : MonoBehaviourPun
{
    [SerializeField]
    public GunController gunController;
    public float attackDamage = 10f;

    protected PlayerInput playerInput;
    [SerializeField]
    protected PlayerInventory playerInventory;

    protected virtual void Awake()
    {
        playerInput = GetComponent<PlayerInput>();
        playerInventory = GetComponent<PlayerInventory>();
    }

    protected virtual void Update()
    {
        if(!photonView.IsMine)
        {
            return;
        }

        if(playerInput.fire) {
            gunController.Fire();
        } else {
            
        }
    }

    protected void OnEnable() {
        gunController.gameObject.SetActive(true);
    }

    protected void OnDisable() {
        gunController.gameObject.SetActive(false);
    }

    [PunRPC]
    public void OnAttack(IDamageable target, Vector3 hitPoint) {
        // 아이템의 OnDamage 함수 호출
        // float damage = playerInventory.OnAttack(attackDamage);

        // 데미지 적용
        // target.OnDamage(damage, hitPoint);
        target.OnDamage(attackDamage, hitPoint);
    }
}
