using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class Bullet : MonoBehaviour
{
    public LayerMask enemyLayerMasks;

    private Rigidbody rigidbody;
    private TrailRenderer trailRenderer;
    [SerializeField]
    private PlayerShooter playerShooter;
    

    public float speed = 20f;

    private Vector3 startPosition;
    private float attackRange = 100f;
    private float damage = 0f;
    private Transform gunController;

    private void Awake() {
        rigidbody = GetComponent<Rigidbody>();
        trailRenderer = GetComponent<TrailRenderer>();
    }

    private void Update() {
        // 총알이 사거리를 벗어나면 제거
        if(Vector3.Distance(startPosition, transform.position) >= attackRange) {
            Release();
        }
    }

    // 총알의 사거리, 데미지, 초기 위치, 회전, 어느 플레이어의 소유인지 등을 설정
    public void Setup(float attackRange, float attackDamage, Vector3 startPosition, Quaternion rot, PlayerShooter playerShooter) {
        this.attackRange = attackRange;
        this.damage = attackDamage;
        this.transform.position = startPosition;
        this.startPosition = transform.position;
        this.transform.rotation = rot;
        this.playerShooter = playerShooter;

        trailRenderer.enabled = true;
        trailRenderer.Clear();
        rigidbody.velocity = Vector3.zero;
        rigidbody.angularVelocity = Vector3.zero;
        rigidbody.AddForce(transform.forward * speed, ForceMode.VelocityChange);
    }

    private void OnTriggerEnter(Collider other) {
        if(other.gameObject.tag == "Enemy" || other.gameObject.tag == "Ground") {
            // 마스터 클라이언트인 경우 데미지 계산하고 처리
            if(PhotonNetwork.IsMasterClient) {
                IDamageable target = other.gameObject.GetComponent<IDamageable>();

                if(target != null) {
                    playerShooter.photonView.RPC("OnAttack", RpcTarget.MasterClient, target, other.transform.position);
                }
            }

            Release();
        }
    }
    
    // ObjectPool에서 가져온 총알을 Release
    private void Release() {
        trailRenderer.enabled = false;
        BulletPool.Instance.ReleaseObject(gameObject);
    }
}
