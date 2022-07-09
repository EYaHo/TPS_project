using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Photon.Pun;

public class EnemyMovement : MonoBehaviour
{
    PhotonView PV;

    NavMeshAgent agent;

    private bool isMove;
    private bool isAttacking;
    private bool alreadyAttack;
    private Vector3 destination;

    [SerializeField] float sightAngle;
    [SerializeField] float sightRange;
    [SerializeField] float attackRange;
    [SerializeField] float attackForeDelay;
    [SerializeField] float attackBackDelay;
    [SerializeField] LayerMask layerMask;

    public GameObject target;
    public Transform eyes;
    private float delayTimer;

    private void Awake()
    {
        if(!PhotonNetwork.IsMasterClient) return;
        PV = PhotonView.Get(this);
    }

    private void Start()
    {/*
        if(!PhotonNetwork.IsMasterClient) return;
        agent = GetComponent<NavMeshAgent>();
        agent.updateRotation = false;*/
    }
    
    public void Initialize(float sightRange, float sightAngle, float moveSpeed)
    {
        agent = GetComponent<NavMeshAgent>();
        agent.updateRotation = false;
        this.sightRange = sightRange;
        this.sightAngle = sightAngle;
        this.attackRange = 2.0f;
        this.attackForeDelay = 1.0f;
        this.attackBackDelay = 1.5f;
        agent.speed = moveSpeed;
        //agent.stoppingDistance = attackRange;

        delayTimer = 0.0f;
    }
/*
    void Update()
    {   
        
    }
*/
    private void FixedUpdate() {
        if(!PhotonNetwork.IsMasterClient) return;
        if(target==null) Sight();
        Stare();

        //if(!PhotonNetwork.IsMasterClient) return;
        if(isAttacking)
        {
            delayTimer += Time.deltaTime;
            if(!alreadyAttack && delayTimer>=attackForeDelay)
            {
                delayTimer = 0.0f;
                Attack();
                alreadyAttack = true;
            }
            if(alreadyAttack && delayTimer>=attackBackDelay)
            {
                delayTimer = 0.0f;
                isAttacking = false;
                alreadyAttack = false;
                Debug.Log("Chase again");
            }
        }
        else
        {
            if(target!=null)
            {
                //SetDestination(target.transform.position);
                Chase();
            }
            else
            {
                Roam();
            }
        }
    }

    private void Sight()
    {
        Collider[] targets = Physics.OverlapSphere(transform.position, sightRange, layerMask); //layerMask = Player
        if(targets.Length > 0)
        {
            for(int i=0; i<targets.Length; i++)
            {
                Transform targetTf = targets[i].transform;
                if(targetTf.tag == "Player")
                {
                    
                    Vector3 direction = (targetTf.position - transform.position).normalized;
                    //Debug.Log(direction);
                    float angle = Vector3.Angle(direction, transform.forward);

                    if(angle < sightAngle * 0.5f)
                    {
                        //Debug.Log(this.name);
                        target = targetTf.gameObject;
                    }
                }
            }
        }
    }

    private void SetDestination(Vector3 dest)
    {
        agent.SetDestination(dest);
        destination = dest;
        isMove = true;
    }

    private void Stare()
    {
        if( agent.velocity.magnitude == 0f)
        {
            isMove = false;
            return;
        }

        if(isMove)
        {
            //var dir = new Vector3(agent.steeringTarget.x, transform.position.y, agent.steeringTarget.z) - transform.position;
            //transform.forward = dir;
            Vector3 dir = (target.transform.position - transform.position).normalized;
            Quaternion q = Quaternion.LookRotation(dir);
            transform.rotation = q;
        }
    }

    private void Chase()
    {
        Vector3 targetPosition = target.transform.position;
        Vector3 distance = targetPosition - transform.position;

        if(distance.magnitude-0.1 <= attackRange)
        {
            Debug.Log("Start Attack");
            isAttacking = true;
            return;
        }

        Vector3 direction = distance.normalized;
        Vector3 dest = targetPosition - (direction * attackRange);
        SetDestination(dest);
    }
    private void Attack()
    {
        Debug.Log("Attack!");
    }

    private void Roam()
    {
        //일정 범위를 배회하도록.
    }
}