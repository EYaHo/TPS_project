using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class PooledObject : MonoBehaviourPun
{
    [PunRPC]
    public void SetActive(bool b) {
        gameObject.SetActive(b);
    }
    [PunRPC]
    public void SetPosition(Vector3 v) {
        transform.position = v;
    }
}
