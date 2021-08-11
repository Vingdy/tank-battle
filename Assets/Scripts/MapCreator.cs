using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;


//Done：修复草和水重叠的DEBUG
//TODO: 奖励实现
//Done：多敌人加入
public class MapCreator : MonoBehaviour
{
    //0.Home 1.Wall 2.Barrier 3.Born 4.River 5.Grass 6.AirWall 7.Bonus
    public GameObject[] item;
    public List<Vector3> itemPositionList = new List<Vector3>();

    // private Utility utils;
    private void Awake()
    {
        InitMap();
    }

    private void InitMap()
    {
        //heart
        CreateItem(item[0], new Vector3(0, -8), Quaternion.identity);
        
        //heart wall
        CreateItem(item[1], new Vector3(-1, -8), Quaternion.identity);
        CreateItem(item[1], new Vector3(1, -8), Quaternion.identity);
        
        for (int i = -1; i <= 1; i++)
        {
            CreateItem(item[1], new Vector3(i, -7), Quaternion.identity);
        }
        
        //air barrier
        for (int i = -11; i <= 11; i++)
        {
            if (i == -9 || i == 9)
            {
                for (int j = -11; j <= 11; j++)
                {
                    CreateItem(item[6], new Vector3(j, i), Quaternion.identity);
                }    
            }
            else
            {
                CreateItem(item[6], new Vector3(-11, i), Quaternion.identity);
                CreateItem(item[6], new Vector3(11, i), Quaternion.identity);
            }
        }

        GameObject go = Instantiate(item[3], new Vector3(-2, -8), Quaternion.identity);//定义初始化位置
        go.GetComponent<Born>().createPlayer = true;//设置gameObject中的初始变量
        
        GameObject go2 = Instantiate(item[3], new Vector3(2, -8), Quaternion.identity);//定义初始化位置
        go2.GetComponent<Born>().createPlayer = true;//设置gameObject中的初始变量
        go2.GetComponent<Born>().Player = 2;//设置gameObject中的初始变量
        
        CreateItem(item[3], new Vector3(-10, 8), Quaternion.identity);
        CreateItem(item[3], new Vector3(0, 8), Quaternion.identity);
        CreateItem(item[3], new Vector3(10, 8), Quaternion.identity);

        InvokeRepeating("CreateEnemy", 4,10);//重复调用

        for (int i = 0; i <= 40; i++)
        {
            CreateItem(item[1], CreateRandomPosition(), Quaternion.identity);
        }
        for (int i = 0; i <= 25; i++)
        {
            CreateItem(item[2], CreateRandomPosition(), Quaternion.identity);
        }
        for (int i = 0; i <= 15; i++)
        {
            CreateItem(item[4], CreateRandomPosition(), Quaternion.identity);
        }
        for (int i = 0; i <= 15; i++)
        {
            CreateItem(item[5], CreateRandomPosition(), Quaternion.identity);
        }

    }

    private void Update()
    {
    }

    private void CreateItem(GameObject createCameObject, Vector3 createPosition, Quaternion createRotation)
    {
        GameObject itemGo = Instantiate(createCameObject, createPosition, createRotation);
        itemGo.transform.SetParent(gameObject.transform);//gameObject.transform是指自己gameObject
        itemPositionList.Add(createPosition);
    }

    private Vector3 CreateRandomPosition()
    {
        while (true)
        {
            Vector3 createPosition = new Vector3(Random.Range(-9, 10), Random.Range(-7, 8));
            if (!HasThePosition(createPosition))
            {
                return createPosition;
            }
            //直接return会退出循环导致同一位置重复渲染
            //存在如果地图全部格子被占满后无法退出循环的问题
            // return new Vector3();
        }
    }

    private bool HasThePosition(Vector3 createPos)
    {
        if (itemPositionList.Contains(createPos))
        {
            return true;
        }
        return false;
    }

    private void CreateEnemy()
    {
        //位置
        int num = Random.Range(0, 3);
        Vector3 EnemyPos = new Vector3();
        if (num == 0)
        {
            EnemyPos = new Vector3(-10, 8);
        }
        else if (num == 1)
        {
            EnemyPos = new Vector3(0, 8);
        }
        else
        {
            EnemyPos = new Vector3(10, 8);
        }
        CreateItem(item[3], EnemyPos, Quaternion.identity);
    }

    private void CreateBonus()
    {
        CreateItem(item[7], CreateRandomPosition(), Quaternion.identity);
    }

    private void HeartProtect(string objName) {
        if (objName == "Wall")
        {
            CreateHeartProtect(item[1]);
        } else if (objName == "Barrier")
        {
            CreateHeartProtect(item[2]);
            this.Invoke(()=>CreateHeartProtect(item[1]), 5f);
        }
    }

    private void CreateHeartProtect(GameObject obj)
    {
        CreateItem(obj, new Vector3(-1, -8), Quaternion.identity);
        this.Invoke(()=>Destroy(obj), 5f);
        CreateItem(obj, new Vector3(1, -8), Quaternion.identity);
        this.Invoke(()=>Destroy(obj), 5f);
        for (int i = -1; i <= 1; i++)
        {
            CreateItem(obj, new Vector3(i, -7), Quaternion.identity);
            this.Invoke(()=>Destroy(obj), 5f);
        }
    }
}

public static class Utility
{
    public static void Invoke(this MonoBehaviour mb, Action f, float delay)
    {
        mb.StartCoroutine(InvokeRoutine(f, delay));
    }
 
    private static IEnumerator InvokeRoutine(System.Action f, float delay)
    {
        yield return new WaitForSeconds(delay);
        f();
    }
}
