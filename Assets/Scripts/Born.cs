using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

public class Born : MonoBehaviour
{
    public GameObject[] playerPrefab;

    public GameObject[] enemyPrefabList;
    
    public int Player;
    public bool createPlayer;
    // Start is called before the first frame update
    void Start()
    {
        Invoke("BornTank", 1f);//延迟调用
        Destroy(gameObject, 1f);//不摧毁的话会一直覆盖
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void BornTank()
    {
        //敌人创建：
        //s:0,1 m:2,3 h:4,5,6,7
        if (!createPlayer)
        {
            //是否奖励
            int bonus = Random.Range(0, 5);//0-3：普通，4：奖励
            if (bonus == 4)
            {
                bonus = 1;
            }
            else
            {
                bonus = 0;
            }
            //种类，如果种类为3说明是重坦，需要计算level
            int type = Random.Range(0, 3);//随机数0-2,左闭右开
            if (type == 2) {            
                int level = Random.Range(1, 4);//1：普通，2：黄，3：绿
                if (bonus == 1)
                {
                    Instantiate(enemyPrefabList[type*2+bonus], transform.position, Quaternion.identity); //无旋转
                }
                else
                {
                    if (level == 1)
                    {
                        level = 0;
                    }
                    Instantiate(enemyPrefabList[type*2+level], transform.position, Quaternion.identity); //无旋转  
                }
                return;
            }
            Instantiate(enemyPrefabList[type*2+bonus], transform.position, Quaternion.identity); //无旋转
        }
        else
        {
            Instantiate(playerPrefab[0], transform.position, Quaternion.identity); //无旋转
            if (Player == 2)
            {
                Instantiate(playerPrefab[1], transform.position, Quaternion.identity); //无旋转
            }
        }
    }
}
