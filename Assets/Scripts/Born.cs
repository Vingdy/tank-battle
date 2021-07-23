using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

public class Born : MonoBehaviour
{
    public GameObject playerPrefab;

    public GameObject[] enemyPrefabList;

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
        if (!createPlayer)
        {
            int num = Random.Range(0, 2);//随机数0-1,左闭右开
            Instantiate(enemyPrefabList[num], transform.position, Quaternion.identity); //无旋转
        }
        else
        {
            Instantiate(playerPrefab, transform.position, Quaternion.identity); //无旋转
        }
    }
}
