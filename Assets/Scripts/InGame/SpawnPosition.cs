using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnPosition : MonoBehaviour
{
    public int spawnPointIdx;
    public EnemyData enemyData;
    public bool isEnemySpawned = false;
    public bool isEnemyReSpawning = false;
    public float respawnDelay = 3.0f;

    public bool IsSpawnAble() {
        if(isEnemyReSpawning || isEnemySpawned) {
            return false;
        } else {
            return true;
        }
    }
}
