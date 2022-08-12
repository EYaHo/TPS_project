using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;
using Photon.Pun;

public class BulletPool : MonoBehaviourPun
{
    private static BulletPool _instance;

    public static BulletPool Instance {
        get {
            if(_instance == null) {
                _instance = FindObjectOfType<BulletPool>();
            }

            return _instance;
        }
    }

    private IObjectPool<GameObject> bulletPool;
    public GameObject bulletPrefab;

    // Start is called before the first frame update
    void Awake()
    {
        
        bulletPool = new ObjectPool<GameObject>(
            () => {
                return PhotonNetwork.Instantiate(bulletPrefab.gameObject.name,transform.position,transform.rotation);
            },
            bullet => { //Get
                //bullet.GetComponent<PooledObject>().photonView.RPC("SetActive", RpcTarget.All, true); <-Bullet.cs Setup()
            },
            bullet => { //Release
                bullet.GetComponent<PooledObject>().photonView.RPC("SetActive", RpcTarget.All, false);
            },
            bullet => {
                PhotonNetwork.Destroy(bullet.gameObject);
            },
            false, 10, 20
        );
    }

    /*
    void Update()
    {
        Debug.Log("bullet count: "+bulletPool.CountInactive);
    }*/

    public GameObject GetBullet() {
        return bulletPool.Get();
    }
    public void ReleaseBullet(GameObject bullet) {
        bulletPool.Release(bullet);
    }
}
