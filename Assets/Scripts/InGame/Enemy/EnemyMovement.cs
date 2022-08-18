using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Photon.Pun;

public class EnemyMovement : MonoBehaviour
{
    protected NavMeshAgent navMeshAgent;

    private bool isMove;
    private bool isAttacking;
    private bool alreadyAttack;
    private Vector3 destination;

    [SerializeField] float damage;
    [SerializeField] protected float sightAngle;
    [SerializeField] protected float sightRange;
    [SerializeField] protected float attackRange;
    [SerializeField] protected float attackForeDelay;
    [SerializeField] float attackBackDelay;
    [SerializeField] protected LayerMask layerMask;

    public LivingEntity targetEntity;
    public Transform eyes;
    protected float lastAttackTime;

    protected EnemyHealth enemyHealth;

    [SerializeField]
    protected bool isTargetInAttackRange = false;

    protected bool hasTarget {
        get {
            if(targetEntity != null && !targetEntity.dead) {
                return true;
            }

            return false;
        }
    }

    private void Awake()
    {
        enemyHealth = GetComponent<EnemyHealth>();
        navMeshAgent = GetComponent<NavMeshAgent>();
    }

    private void Start()
    {
        if(!PhotonNetwork.IsMasterClient) return;
        lastAttackTime = Time.time;
        StartCoroutine(UpdatePath());
    }
    
    public void Initialize(float damage, float sightRange, float sightAngle, float moveSpeed)
    {
        navMeshAgent.updateRotation = false;
        this.damage = damage;
        this.sightRange = sightRange;
        this.sightAngle = sightAngle;
        this.attackRange = 2.0f;
        this.attackForeDelay = 1.0f;
        this.attackBackDelay = 1.5f;
        navMeshAgent.speed = moveSpeed;
    }

    private void FixedUpdate() {
        if(!PhotonNetwork.IsMasterClient) return;

        UpdateRotation();
        CheckTargetInAttackRange();
        UpdateAttack();
    }

    private void UpdateRotation() {
        if(targetEntity != null) {
            Vector3 direction = (targetEntity.transform.position - transform.position).normalized;
            Quaternion q = Quaternion.LookRotation(direction);
            transform.rotation = q;
        }
    }

    private void CheckTargetInAttackRange() {
        if(targetEntity != null) {
            if(Vector3.Distance(targetEntity.transform.position, transform.position) <= attackRange) {
                isTargetInAttackRange = true;
            } else {
                isTargetInAttackRange = false;
            }
        } else {
            isTargetInAttackRange = false;
        }
    }

    protected virtual void UpdateAttack() {
        if(!PhotonNetwork.IsMasterClient) {
            return;
        }

        if(isTargetInAttackRange) {
            if(!enemyHealth.dead && Time.time >= lastAttackTime + attackForeDelay) {
                lastAttackTime = Time.time;
                Attack(targetEntity);
            }
        }
    }

    // 경로 탐색
    protected virtual IEnumerator UpdatePath() {
        while(!enemyHealth.dead) {
            // 타겟이 있는 경우 NavMeshAgent를 사용
            if(hasTarget && !isTargetInAttackRange) {
                navMeshAgent.isStopped = false;
                Vector3 distance = targetEntity.transform.position - transform.position;
                navMeshAgent.SetDestination(targetEntity.transform.position - (distance.normalized * attackRange * 0.8f));
            } else {
                navMeshAgent.isStopped = true;

                // 일정 범위의 LivingEntity들을 탐색
                Collider[] colliders = Physics.OverlapSphere(transform.position, sightRange, layerMask);
                foreach (Collider collider in colliders) {
                    LivingEntity livingEntity = collider.GetComponent<LivingEntity>();

                    if(livingEntity != null && !livingEntity.dead) {
                        Transform targetTransform = collider.transform;
                        Vector3 direction = (targetTransform.position - transform.position).normalized;
                        float angle = Vector3.Angle(direction, transform.forward);

                        // 대상 LivingEntity가 시야 범위 내에 존재하면 타겟으로 설정
                        if(angle < sightAngle * 0.5f) {
                            targetEntity = livingEntity;
                            break;
                        }
                    }
                }
            }

            yield return new WaitForSeconds(0.25f);
        }
    }

    protected void Attack(LivingEntity attackTarget) {
        Collider targetCollider = attackTarget.GetComponent<Collider>();
        Vector3 hitPoint = targetCollider.ClosestPoint(transform.position);

        attackTarget.OnDamage(damage, hitPoint);
        Debug.Log("Attack!", attackTarget);
    }

    private void Roam()
    {
        //일정 범위를 배회하도록.
    }
}