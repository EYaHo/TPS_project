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

    protected PlayerInput playerInput;
    [SerializeField]
    protected InventoryObject inventoryObject;
    protected bool fireInputDown = false;

    protected virtual void Awake()
    {
        playerInput = GetComponent<PlayerInput>();
        inventoryObject = GetComponent<Player>().inventoryObject;
    }

    protected virtual void Update()
    {
        if(!photonView.IsMine)
        {
            return;
        }

        if(fireInputDown) {
            gunController.Fire();
        }
    }

    protected void OnEnable() {
        gunController.gameObject.SetActive(true);
    }

    protected void OnDisable() {
        gunController.gameObject.SetActive(false);
    }

    public void OnFire(InputAction.CallbackContext context) {
        switch(context.phase) {
            case InputActionPhase.Performed:
                fireInputDown = true;
                break;
            default:
                fireInputDown = false;
                break;
        }
        Debug.Log(fireInputDown);
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
