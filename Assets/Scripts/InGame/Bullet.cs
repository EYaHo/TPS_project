using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class Bullet : MonoBehaviourPunCallbacks
{
    public float speed = 2f;

    private Vector3 startPosition;
    private float attackRange = 100f;
    private bool isAlive;
    private float damage = 0f;

    private void Update() {
        if(!isAlive) return;
        transform.position += transform.forward * Time.deltaTime * speed;

        if(Vector3.Distance(startPosition, transform.position) >= attackRange) {
            //PhotonNetwork.Destroy(this.gameObject);
            Release();
        }
    }

    [PunRPC]
    public void Setup(float attackRange, float attackDamage, Vector3 startPosition, Quaternion rot) {
        this.attackRange = attackRange;
        this.damage = attackDamage;
        this.transform.position = startPosition;
        this.startPosition = transform.position;
        this.transform.rotation = rot;
        GetComponent<PooledObject>().photonView.RPC("SetActive", RpcTarget.All, true);
        isAlive = true;
    }

    private void OnTriggerEnter(Collider other) {
        if(other.gameObject.tag == "Enemy" || other.gameObject.tag == "Ground") {
            Debug.Log("Bullet hit: "+other.gameObject.name);
            //PhotonNetwork.Destroy(this.gameObject);
            Release();
        }
    }
    private void Release() {
        
        BulletPool.instance.ReleaseBullet(gameObject);
        isAlive = false;
    }
}
