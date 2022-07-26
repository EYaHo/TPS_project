using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public enum EnemyType { A, B, C, D, E }

public class EnemySpawner : MonoBehaviourPun, IPunObservable
{
    public GameObject enemyPrefab;
    public GameObject spawnPointSet;

    [SerializeField]
    public List<EnemyData> enemyDatas;
    [SerializeField]
    private List<Transform> spawnPointList;
    [SerializeField]
    private List<Enemy> enemies = new List<Enemy>();
    private int enemyCount = 0;

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info) {
        if(stream.IsWriting) {
            stream.SendNext(enemies.Count);
        } else {
            enemyCount = (int) stream.ReceiveNext();
        }
    }

    private void Start()
    {
        if (PhotonNetwork.IsMasterClient) {
            InitializeSpawnPosition();
            Invoke("SpawnEnemy", 5);
        }
    }

    private void Update() {
        if(PhotonNetwork.IsMasterClient) {
            if(GameManager.instance != null && GameManager.instance.isGameover) {
                return;
            }
        }
    }

    public void SpawnEnemy()
    {
        int enemyID = Random.Range(0, enemyDatas.Count);
        int spawnPointIdx = Random.Range(0, spawnPointList.Count);
        Transform spawnPoint = spawnPointList[spawnPointIdx];
        GameObject createdEnemy = PhotonNetwork.Instantiate(enemyPrefab.gameObject.name, spawnPoint.position, spawnPoint.rotation);
        Enemy enemy = createdEnemy.GetComponent<Enemy>();

        EnemyData enemyData = enemyDatas[enemyID];
        // 수정하기!!
        // 게임이 시작되자마자 몬스터가 생기면 그 다음 들어오는 플레이어에게는 이 함수가 호출되지 않음
        // 모든 플레이어가 동시에 게임이 시작되도록 수정해야함
        enemy.photonView.RPC("Setup", RpcTarget.All, enemyData.EnemyName, enemyData.Hp, enemyData.Damage, enemyData.SightRange, enemyData.SightAngle, enemyData.MoveSpeed);
        //
        enemy.PrintEnemyData();
        enemies.Add(enemy);
    }

    public void InitializeSpawnPosition()
    {
        for(int i=0; i < spawnPointSet.transform.childCount; i++) {
            spawnPointList.Add(spawnPointSet.transform.GetChild(i).gameObject.transform);
        }
    }
}