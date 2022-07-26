using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class Bullet : MonoBehaviourPunCallbacks
{
    public float speed = 2f;

    private Vector3 startPosition;
    private float attackRange = 100f;
    private float damage = 0f;

    private void Update() {
        transform.position += transform.forward * Time.deltaTime * speed;

        if(Vector3.Distance(startPosition, transform.position) >= attackRange) {
            PhotonNetwork.Destroy(this.gameObject);
        }
    }

    [PunRPC]
    public void Setup(float attackRange, float attackDamage) {
        this.attackRange = attackRange;
        this.damage = attackDamage;
        startPosition = transform.position;
    }

    private void OnTriggerEnter(Collider other) {
        if(other.gameObject.tag == "Enemy" || other.gameObject.tag == "Ground") {
            Debug.Log("Bullet hit: "+other.gameObject.name);
            PhotonNetwork.Destroy(this.gameObject);
        }
    }
}
