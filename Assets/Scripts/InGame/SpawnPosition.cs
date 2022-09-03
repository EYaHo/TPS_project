using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class SpawnPosition : MonoBehaviour
{
    public EnemyData enemyData;

    public float spawnDelay = 10.0f;
    public bool isSpawned = false;

    private void Update() {
        if(PhotonNetwork.IsMasterClient) {
            if(!isSpawned) {

            }
        }
    }
}
