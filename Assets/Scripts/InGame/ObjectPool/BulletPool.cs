using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class BulletPool : AbstractPool
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
}
