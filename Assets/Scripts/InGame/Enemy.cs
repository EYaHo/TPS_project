using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class Enemy : MonoBehaviour
{
    public EnemyData enemyData;
    public string enemyName;

    public void PrintEnemyData()
    {
        Debug.Log("좀비이름 :: " + enemyData.EnemyName);
        Debug.Log("체력 :: " + enemyData.Hp);
        Debug.Log("공격력 :: " + enemyData.Damage);
        Debug.Log("시야 :: " + enemyData.SightRange);
        Debug.Log("이동속도 :: " + enemyData.MoveSpeed);
        Debug.Log("---------------------------------------");
    }
}
