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
    protected Player player;

    //private PlayerAnimationController animController;

    protected virtual void Awake()
    {
        playerInput = GetComponent<PlayerInput>();
        player = GetComponent<Player>();
        //animController = GetComponent<PlayerAnimationController>();
    }

    protected virtual void Update()
    {
        if(!photonView.IsMine)
        {
            return;
        }

        if(playerInput.fire) {
            //animController.photonView.RPC("ChangeAnimationState", RpcTarget.All, PlayerAnimationController.AnimState.Shoot_Autoshot_AR.ToString(), 1, 0f);
            gunController.Fire();
        } else {
            //animController.photonView.RPC("ChangeAnimationState", RpcTarget.All, PlayerAnimationController.AnimState.Idle_gunMiddle_AR.ToString(), 1, 0f);
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
        float damage = player.OnAttack(attackDamage);

        // 데미지 적용
        target.OnDamage(damage, hitPoint);
    }
}
