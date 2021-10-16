using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;//使用UI使用
using UnityEngine.SceneManagement;

//总控制集合，除碰撞外所有变量交互和控制必须通过该Manager
//TODO：添加双人功能
public class PlayerManager : MonoBehaviour
{
    public int lifeVal = 3;

    public int playerScore = 0;

    public bool isP1Dead = false;
    public bool isP2Dead = false;
    
    public bool isDefeat = false;

    public GameObject Born;

    public Text playerScoreText;
    public Text playLifeValText;
    public GameObject isDefeatUI;

    public GameObject MapCreator;

    //单例，主要用于其他cs对该cs中public变量控制
    private static PlayerManager instance;

    public static PlayerManager Instance
    {
        get
        {
            return instance;
        }
        set
        {
            instance = value;
        }
    }

    private void Awake()
    {
        Instance = this;
    }


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (isDefeat)
        {
            isDefeatUI.SetActive(true);
            Invoke("ReturnToMainMenu", 3);
            return;
        }
        if (isP1Dead)
        {
            Recover(1);
        }
        if (isP2Dead)
        {
            Recover(2);
        }

        playerScoreText.text = playerScore.ToString();
        playLifeValText.text = lifeVal.ToString();
    }

    private void Recover(int playerNum)
    {
        if (lifeVal <= 0)
        {
            //失败
            isDefeat = true;
            Invoke("ReturnToMainMenu", 3);
        }
        else
        {
            lifeVal--;
            if (playerNum == 1)
            {
                GameObject go = Instantiate(Born, new Vector3(-2, -8), Quaternion.identity);
                go.GetComponent<Born>().createPlayer = true;//设置gameObject中的初始变量  
                go.GetComponent<Born>().Player = 1;//设置gameObject中的初始变量 
                isP1Dead = false;
            }
            else if (playerNum == 2)
            {
                GameObject go2 = Instantiate(Born, new Vector3(2, -8), Quaternion.identity);//定义初始化位置
                go2.GetComponent<Born>().createPlayer = true;//设置gameObject中的初始变量
                go2.GetComponent<Born>().Player = 2;//设置gameObject中的初始变量 
                isP2Dead = false;
            }

        }
    }

    private void ReturnToMainMenu()
    {
        SceneManager.LoadScene(0);
    }

    private void HeartProtect()
    {
        MapCreator.SendMessage("HeartProtect", "Barrier");
    }

    private void FindAllEnemyAndKill()
    {
        //找到所有Tag为Enemy的obj
        GameObject[] enemys = GameObject.FindGameObjectsWithTag("Enemy");
        for (int i = 0; i < enemys.Length; i++)
        {
            enemys[i].SendMessage("DieNow");
        }
    }

    private void FindAllEnemyAndStop()
    {
        GameObject[] enemys = GameObject.FindGameObjectsWithTag("Enemy");
        for (int i = 0; i < enemys.Length; i++)
        {
            enemys[i].SendMessage("StopNow");
        }
    }
}
