using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class DamagePopupPool : AbstractPool
{
    private static DamagePopupPool _instance;

    public static DamagePopupPool Instance {
        get {
            if(_instance == null) {
                _instance = FindObjectOfType<DamagePopupPool>();
            }

            return _instance;
        }
    }
}
