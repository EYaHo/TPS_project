using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class Bullet : MonoBehaviourPunCallbacks
{
    public float damage;
    public float speed;
    private Vector3 aimPoint;
    private Vector3 expiringPoint;
    // Start is called before the first frame update
    void Start()
    {
        damage = 10f;
        speed = 0.05f;
    }
    private void Update() {
        transform.position = Vector3.MoveTowards(transform.position, expiringPoint, speed);
        if(transform.position == expiringPoint) {
            Debug.Log("Bullet is expired: ");
            PhotonNetwork.Destroy(this.gameObject);
        }
    }

    [PunRPC]
    public void Setup(Vector3 aimPoint, float attackRange) {
        this.aimPoint = aimPoint;
        this.expiringPoint = transform.position + (aimPoint - transform.position).normalized * attackRange;
    }

    private void OnCollisionEnter(Collision other) {
        if(other.gameObject.tag == "Weapon" || other.gameObject.tag == "Player") return;
        Debug.Log("Bullet hit: "+other.gameObject.name);
        PhotonNetwork.Destroy(this.gameObject);
    }
}
