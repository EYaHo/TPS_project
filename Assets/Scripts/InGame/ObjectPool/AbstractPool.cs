using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class AbstractPool : MonoBehaviour
{
    public int maxPoolSize = 50;
    public GameObject objectPrefab;
    protected IObjectPool<GameObject> objectPool;

    // Start is called before the first frame update
    public virtual void Awake()
    {
        objectPool = new ObjectPool<GameObject>(
            CreatePooledItem,
            OnTakeFromPool,
            OnReturnedToPool,
            OnDestroyPoolObject,
            false, 10, maxPoolSize
        );
    }

    protected virtual GameObject CreatePooledItem() {
        return Instantiate(objectPrefab, transform.position, transform.rotation, transform);
    }

    protected virtual void OnTakeFromPool(GameObject obj) {
        obj.SetActive(true);
    }

    protected virtual void OnReturnedToPool(GameObject obj) {
        obj.SetActive(false);
    }

    protected virtual void OnDestroyPoolObject(GameObject obj) {
        Destroy(obj.gameObject);
    }

    public int GetPoolSize() {
        return objectPool.CountInactive;
    }

    public GameObject GetObject() {
        return objectPool.Get();
    }
    
    public void ReleaseObject(GameObject obj) {
        objectPool.Release(obj);
    }
}
