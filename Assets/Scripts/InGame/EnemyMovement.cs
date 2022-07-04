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
    private Vector3 destination;

    [SerializeField] float sightAngle;
    [SerializeField] float sightRange;
    [SerializeField] LayerMask layerMask;

    public GameObject target;
    public Transform eyes;

    private void Awake()
    {
        if(!PhotonNetwork.IsMasterClient) return;
        PV = PhotonView.Get(this);
    }

    private void Start()
    {
        if(!PhotonNetwork.IsMasterClient) return;
        agent = GetComponent<NavMeshAgent>();
        agent.updateRotation = false;
    }
    
    public void Initialize(float sightRange, float sightAngle, float moveSpeed)
    {
        this.sightRange = sightRange;
        this.sightAngle = sightAngle;
        agent.speed = moveSpeed;
    }

    void Update()
    {   
        if(!PhotonNetwork.IsMasterClient) return;
        if(target!=null)
        {
            SetDestination(target.transform.position);
        }
    }

    private void FixedUpdate() {
        if(!PhotonNetwork.IsMasterClient) return;
        if(target==null) Sight();
        Stare();
    }

    private void Sight()
    {
        Collider[] targets = Physics.OverlapSphere(transform.position, sightRange, layerMask);
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
}