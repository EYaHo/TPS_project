using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;
using Photon.Pun;

public class EnemyPool : AbstractPool
{
    private static EnemyPool _instance;

    public static EnemyPool Instance {
        get {
            if(_instance == null) {
                _instance = FindObjectOfType<EnemyPool>();
            }

            return _instance;
        }
    }

    [SerializeField]
    private List<IObjectPool<GameObject>> poolList;
    [SerializeField]
    private List<EnemyData> enemyList;

    public override void Awake()
    {
        if(!PhotonNetwork.IsMasterClient) return;
        poolList = new List<IObjectPool<GameObject>>();

        for(int i=0; i<enemyList.Count; i++) {
            IObjectPool<GameObject> enemyPool = new ObjectPool<GameObject>(
                () => CreatePooledItem(enemyList[i].EnemyName),
                OnTakeFromPool,
                OnReturnedToPool,
                OnDestroyPoolObject,
                false, 10, maxPoolSize
            );
            GameObject obj = enemyPool.Get();
            enemyPool.Release(obj);
            poolList.Add(enemyPool);
        }
    }
    protected GameObject CreatePooledItem(string enemyName) {
        return PhotonNetwork.Instantiate(enemyName, transform.position, transform.rotation);
    }

    protected override void OnTakeFromPool(GameObject obj) {
        //obj.SetActive(true);
    }

    protected override void OnReturnedToPool(GameObject obj) {
        //obj.SetActive(false);
        obj.GetComponent<PooledObject>().photonView.RPC("SetActive", RpcTarget.AllBuffered, false);
    }

    public GameObject GetObject(int poolListIdx, Vector3 startPosition) {
        GameObject enemy = poolList[poolListIdx].Get();
        var pv = enemy.GetComponent<PooledObject>().photonView;
        pv.RPC("SetPosition", RpcTarget.AllBuffered, startPosition);
        pv.RPC("SetActive", RpcTarget.AllBuffered, true);
        //enemy.transform.position = startPosition;
        //enemy.SetActive(true);
        return enemy;
    }

    public void ReleaseObject(int poolListIdx, GameObject obj) {
        poolList[poolListIdx].Release(obj);
    } 

    public int GetPoolListIdx(string enemyName) {
        int result = -1;
        for(int i=0; i<enemyList.Count; i++) {
            if(enemyList[i].EnemyName == enemyName) {
                result = i;
            }
        }
        return result;
    }
}
