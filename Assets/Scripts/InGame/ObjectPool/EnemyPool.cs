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

    private List<IObjectPool<GameObject>> enemyPoolList;

    protected override void InitializePool() {
        objectPool = new ObjectPool<GameObject>(
            CreatePooledItem,
            OnTakeFromPool,
            OnReturnedToPool,
            OnDestroyPoolObject,
            false, 10, maxPoolSize
        );
    }

    protected override GameObject CreatePooledItem() {
        return PhotonNetwork.Instantiate(objectPrefab.name, transform.position, transform.rotation);
    }

    protected override void OnTakeFromPool(GameObject obj) {
        obj.GetComponent<PhotonView>().RPC("SetActive", RpcTarget.AllBuffered, true);
        //obj.SetActive(true);
    }

    protected override void OnReturnedToPool(GameObject obj) {
        obj.GetComponent<PhotonView>().RPC("SetActive", RpcTarget.AllBuffered, false);
        //obj.SetActive(false);
    }
}
