using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class BulletPool : MonoBehaviour
{
    private static BulletPool _instance;

    private int maxPoolSize = 20;

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
                return Instantiate(bulletPrefab, transform.position, transform.rotation);
            },
            bullet => { //Get
                bullet.SetActive(true);
            },
            bullet => { //Release
                bullet.SetActive(false);
            },
            bullet => {
                Destroy(bullet.gameObject);
            },
            false, 10, maxPoolSize
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
