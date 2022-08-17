using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class SlimeMovement : EnemyMovement
{
    private SlimeAnimationController animController;

    private void Awake() {
        enemyHealth = GetComponent<EnemyHealth>();
        navMeshAgent = GetComponent<UnityEngine.AI.NavMeshAgent>();
        animController = GetComponent<SlimeAnimationController>();
    }

    protected override void UpdateAttack() {
        if(!PhotonNetwork.IsMasterClient) {
            return;
        }

        if(isTargetInAttackRange) {
            if(!enemyHealth.dead && Time.time >= lastAttackTime + attackForeDelay) {
                animController.ChangeAnimationState(SlimeAnimationController.AnimState.Attack01.ToString());    //anim
                //animController.photonView.RPC("ChangeAnimationState", RpcTarget.All, SlimeAnimationController.AnimState.Attack01.ToString());
                lastAttackTime = Time.time;
                Attack(targetEntity);
            }
        }
    }

    protected override IEnumerator UpdatePath() {
        while(!enemyHealth.dead) {
            // 타겟이 있는 경우 NavMeshAgent를 사용
            if(hasTarget && !isTargetInAttackRange) {
                navMeshAgent.isStopped = false;
                Vector3 distance = targetEntity.transform.position - transform.position;
                animController.ChangeAnimationState(SlimeAnimationController.AnimState.WalkFWD.ToString());     //anim
                //animController.photonView.RPC("ChangeAnimationState", RpcTarget.All, SlimeAnimationController.AnimState.WalkFWD.ToString());
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
}
