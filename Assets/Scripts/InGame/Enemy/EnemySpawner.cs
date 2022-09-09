using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

//public enum EnemyType { A, B, C, D, E }

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

    private static EnemySpawner _instance;

    public static EnemySpawner Instance {
        get {
            if(_instance == null) {
                _instance = FindObjectOfType<EnemySpawner>();
            }

            return _instance;
        }
    }

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
            //Invoke("SpawnEnemy", 5);
            Invoke("SpawnEnemyAllPosition", 5);
        }
    }

    private void Update() {
        if(PhotonNetwork.IsMasterClient) {
            if(GameManager.Instance != null && GameManager.Instance.isGameover) {
                return;
            }
        }
    }

    public void SpawnEnemy()
    {
        int enemyID = Random.Range(0, enemyDatas.Count);
        int spawnPointIdx = Random.Range(0, spawnPointList.Count);
        Transform spawnPoint = spawnPointList[spawnPointIdx];

        EnemyData enemyData = enemyDatas[enemyID];
        GameObject createdEnemy = PhotonNetwork.Instantiate(enemyData.Prefab.gameObject.name/*enemyPrefab.gameObject.name*/, spawnPoint.position, spawnPoint.rotation);
        createdEnemy.name = enemyData.Prefab.gameObject.name;
        Enemy enemy = createdEnemy.GetComponent<Enemy>();

        // 수정하기!!
        // 게임이 시작되자마자 몬스터가 생기면 그 다음 들어오는 플레이어에게는 이 함수가 호출되지 않음
        // 모든 플레이어가 동시에 게임이 시작되도록 수정해야함
        enemy.photonView.RPC("Setup", RpcTarget.All, enemyData.EnemyName, enemyData.Hp, enemyData.Damage, enemyData.SightRange, enemyData.SightAngle, enemyData.MoveSpeed);
        //
        enemy.PrintEnemyData();
        enemies.Add(enemy);
    }

    public void SpawnEnemyByPosition(SpawnPosition spawnPos) {
        // Transform spawnPoint = spawnPointList[spawnPointIdx];
        // SpawnPosition spawnPos = spawnPoint.GetComponent<SpawnPosition>();

        EnemyData enemyData = spawnPos.enemyData;
        GameObject createdEnemy = PhotonNetwork.Instantiate(enemyData.Prefab.gameObject.name/*enemyPrefab.gameObject.name*/, spawnPos.transform.position, spawnPos.transform.rotation);
        createdEnemy.name = enemyData.Prefab.gameObject.name;
        Enemy enemy = createdEnemy.GetComponent<Enemy>();

        enemy.photonView.RPC("Setup", RpcTarget.All, enemyData.EnemyName, enemyData.Hp, enemyData.Damage, enemyData.SightRange, enemyData.SightAngle, enemyData.MoveSpeed, spawnPos.spawnPointIdx);
        //
        enemy.PrintEnemyData();
        enemies.Add(enemy);

        spawnPos.isEnemySpawned = true;
    }

    public void SpawnEnemyAllPosition() {
        for(int i=0; i < spawnPointSet.transform.childCount; i++) {
            Transform spawnPoint = spawnPointList[i];
            SpawnPosition spawnPos = spawnPoint.GetComponent<SpawnPosition>();

            if(spawnPos.IsSpawnAble()) {
                SpawnEnemyByPosition(spawnPos);
            }
        }
    }

    public void ReSpawn(int spawnPointIdx) {
        if (!PhotonNetwork.IsMasterClient) return;

        Transform spawnPoint = spawnPointList[spawnPointIdx];
        SpawnPosition spawnPos = spawnPoint.GetComponent<SpawnPosition>();

        spawnPos.isEnemySpawned = false;
        
        if(spawnPos.IsSpawnAble()) {
            spawnPos.isEnemyReSpawning = true;
            StartCoroutine(ReSpawnCoroutine(spawnPos));
        }
    }
    IEnumerator ReSpawnCoroutine(SpawnPosition spawnPos) {
        yield return new WaitForSeconds(spawnPos.respawnDelay);
        
        SpawnEnemyByPosition(spawnPos);
        spawnPos.isEnemyReSpawning = false;
    }

    public void InitializeSpawnPosition()
    {
        for(int i=0; i < spawnPointSet.transform.childCount; i++) {

            Transform spawnPoint = spawnPointSet.transform.GetChild(i).gameObject.transform;
            SpawnPosition spawnPos = spawnPoint.GetComponent<SpawnPosition>();
            
            spawnPointList.Add(spawnPoint);
            spawnPos.spawnPointIdx = i;
            
        }
    }
}