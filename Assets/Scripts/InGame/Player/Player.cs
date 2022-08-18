using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class Player : MonoBehaviourPun
{
    public float attackDamage = 10f;
    public GameObject canvas;

    [SerializeField]
    private Item[] items;

    private void Awake() {
        if(!photonView.IsMine) {
            canvas.SetActive(false);
        }
    }

    [PunRPC]
    public void OnDamage(IDamageable target, Vector3 hitPoint) {
        // 아이템의 OnDamage 함수 호출

        // 데미지 적용
        target.OnDamage(attackDamage, hitPoint);
    }
}
