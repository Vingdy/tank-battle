using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;//使用UI使用
using UnityEngine.SceneManagement;

//TODO：添加双人功能
public class PlayerManager : MonoBehaviour
{
    public int lifeVal = 3;

    public int playerScore = 0;

    public bool isDead = false;
    public bool isDefeat = false;

    public GameObject Born;

    public Text playerScoreText;
    public Text playLifeValText;
    public GameObject isDefeatUI;

    //单例
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
        if (isDead)
        {
            Recover();
        }

        playerScoreText.text = playerScore.ToString();
        playLifeValText.text = lifeVal.ToString();
    }

    private void Recover()
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
            GameObject go = Instantiate(Born, new Vector3(-2, -8), Quaternion.identity);
            go.GetComponent<Born>().createPlayer = true;//设置gameObject中的初始变量
            isDead = false;
        }
    }

    private void ReturnToMainMenu()
    {
        SceneManager.LoadScene(0);
    }
}
