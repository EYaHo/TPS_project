using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public enum EnemyType { A, B, C, D, E }

public class EnemySpawner : MonoBehaviour
{
    PhotonView PV;
    [SerializeField]
    private List<EnemyData> enemyDatas;
    [SerializeField]
    private GameObject enemyPrefab;

    [SerializeField]
    private GameObject spawnPositionSet;
    [SerializeField]
    private List<Transform> spawnPositionList;

    // Start is called before the first frame update
    void Start()
    {
        if(!PhotonNetwork.IsMasterClient) return;
        Debug.Log("Start");
        PV = PhotonView.Get(this);
        InitializeSpawnPosition();
        PV.RPC("SpawnEnemy", RpcTarget.All);
    }

    [PunRPC]
    public void SpawnEnemy()
    {
        for(int i=0; i<enemyDatas.Count; i++)
       {
            Transform spawnPosition = spawnPositionList[i];
            var newEnemy = PhotonNetwork.Instantiate("Enemy", spawnPosition.position, spawnPosition.rotation).GetComponent<Enemy>();
            //newEnemy.enemyData = enemyDatas[i];
            //newEnemy.gameObject.GetComponent<EnemyMovement>().enemyData = enemyDatas[i];
            SetEnemyData(newEnemy, enemyDatas[i]);
            newEnemy.PrintEnemyData();
        }
    }
    public void SetEnemyData(Enemy enemy, EnemyData _enemyData)
    {
        enemy.enemyData = _enemyData;
        enemy.gameObject.GetComponent<EnemyMovement>().enemyData = _enemyData;
        enemy.name = enemy.enemyData.EnemyName;
    }

    public void InitializeSpawnPosition()
    {
        foreach (Transform area in spawnPositionSet.transform)
        {
            spawnPositionList.Add(area);
        }
    }
}
