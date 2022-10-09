using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Photon.Pun;

public class PlayerShooter : MonoBehaviourPun
{
    [SerializeField]
    public GunController gunController;
    public float attackDamage = 10f;
    public PlayerInputManager playerInputManager;

    [SerializeField]
    protected InventoryObject inventoryObject;
    

    protected virtual void Awake()
    {
        playerInputManager = GetComponent<PlayerInputManager>();
        inventoryObject = GetComponent<Player>().inventoryObject;
    }

    protected virtual void Update()
    {
        if(!photonView.IsMine)
        {
            return;
        }

        if(playerInputManager.fire) {
            gunController.Fire();
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
        target.OnDamage(attackDamage, hitPoint);
    }
}
