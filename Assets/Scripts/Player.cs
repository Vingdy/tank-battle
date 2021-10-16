using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

//一个神奇的bug，一切正常的情况下，运行后数组初始化为空，且仅有这个脚本出现问题
//搜索引擎+长时间debug无果，重新复制粘贴脚本后修复
//Done：player移动切换渲染
//TODO:more tank
public class Player : MonoBehaviour
{
    public int PlayerNum;
    
    private SpriteRenderer sr;

    //同一个坦克不同的渲染贴图，上右下左
    public Sprite[] tankSprite;
    public Sprite[] tankSprite2;

    public GameObject bulletPrefab;
    public GameObject explosionPrefab;

    public Vector3 bulletRulerAngles;

    private bool isDefended = true;

    //没印象了，0：普通，1：加速，2：炮弹威力加强（能打穿障碍），3：炮弹发射间隔减低
    private int level = 0;

    private float fireTimeVal = 0.4f;//开火定时器
    private float fireTimeController = 0.4f;
    private float moveLengthVal = 0;//移动计时器
    public float moveSpeed = 3;

    public AudioSource moveAudio;
    public AudioClip[] tankAudio;
     private void Awake()
    {
        sr = GetComponent<SpriteRenderer>();//获取渲染组件
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (fireTimeVal >= fireTimeController)
        {
            Attack();
        }
        else
        {
            fireTimeVal += Time.fixedDeltaTime;
        }
    }

    private void FixedUpdate()
    {
        if (PlayerManager.Instance.isDefeat)
        {
            return;
        }
        Move();
    }

    private void Attack()
    {
        if(Input.GetKeyDown(KeyCode.Space) && PlayerNum == 1)
        {
            GameObject bullet = Instantiate(bulletPrefab, transform.position, Quaternion.Euler(transform.eulerAngles+bulletRulerAngles));

            bullet.GetComponent<Bullet>().level = level;
            // bullet.GetComponent<Bullet>().SendMessage(); = true;//设置gameObject中的初始变量
            fireTimeVal = 0;
        }
        if(Input.GetKeyDown(KeyCode.Return) && PlayerNum == 2)
        {
            GameObject bullet = Instantiate(bulletPrefab, transform.position, Quaternion.Euler(transform.eulerAngles+bulletRulerAngles));

            bullet.GetComponent<Bullet>().level = level;
            // bullet.GetComponent<Bullet>().SendMessage(); = true;//设置gameObject中的初始变量
            fireTimeVal = 0;
        }
    }

    private void Move()
    {
        float v = Input.GetAxisRaw("Player"+PlayerNum+"Vertical");

        transform.Translate(Vector3.up * v * moveSpeed * Time.fixedDeltaTime,
            Space.World);
        moveLengthVal += Mathf.Abs(v) * moveSpeed * Time.fixedDeltaTime;
        
        if (v < 0)
        {
            if (moveLengthVal > 5 * moveSpeed * Time.fixedDeltaTime)
            {
                sr.sprite = tankSprite2[2+level*4];
                if (moveLengthVal > 10 * moveSpeed * Time.fixedDeltaTime)
                {
                    moveLengthVal = 0;
                }
            }
            else
            {
                sr.sprite = tankSprite[2+level*4];
            }
            bulletRulerAngles = new Vector3(0, 0, 180);
        }
        else if (v > 0)
        {
            if (moveLengthVal > 5 * moveSpeed * Time.fixedDeltaTime)
            {
                sr.sprite = tankSprite2[0+level*4];
                if (moveLengthVal > 10 * moveSpeed * Time.fixedDeltaTime)
                {
                    moveLengthVal = 0;
                }
            }
            else
            {
                sr.sprite = tankSprite[0+level*4];
            }
            bulletRulerAngles = new Vector3(0, 0, 0);
        }


        if (Mathf.Abs(v)>0.5f)
        {
            moveAudio.clip = tankAudio[1];
            //记得避免一直调用,eg:1s invoke 10times
            if (!moveAudio.isPlaying)
            {
                moveAudio.Play();
            }
        }
        
        if (v != 0)
        {
            return;
        }
        
        float h = Input.GetAxisRaw("Player"+PlayerNum+"Horizontal");
        transform.Translate(Vector3.right * h * moveSpeed * Time.fixedDeltaTime,
            Space.World);
        moveLengthVal += Mathf.Abs(h) * moveSpeed * Time.fixedDeltaTime;
        if (h < 0)
        {
            if (moveLengthVal > 1 * moveSpeed * Time.fixedDeltaTime)
            {
                sr.sprite = tankSprite2[3+level*4];
                if (moveLengthVal > 2 * moveSpeed * Time.fixedDeltaTime)
                {
                    moveLengthVal = 0;
                }
            }
            else
            {
                sr.sprite = tankSprite[3+level*4];
            }
            bulletRulerAngles = new Vector3(0, 0, 90);
        }
        else if (h > 0)
        {
            if (moveLengthVal > 1 * moveSpeed * Time.fixedDeltaTime)
            {
                sr.sprite = tankSprite2[1+level*4];
                if (moveLengthVal > 2 * moveSpeed * Time.fixedDeltaTime)
                {
                    moveLengthVal = 0;
                }
            }
            else
            {
                sr.sprite = tankSprite[1+level*4];
            }
            bulletRulerAngles = new Vector3(0, 0, -90);
        }
        if (Mathf.Abs(h)>0.5f)
        {
            moveAudio.clip = tankAudio[1];
            //记得避免一直调用,etc 1s invoke 10times
            if (!moveAudio.isPlaying)
            {
                moveAudio.Play();
            }
        }
        else
        {
            moveAudio.clip = tankAudio[0];
            if (!moveAudio.isPlaying)
            {
                moveAudio.Play();
            }
        }
    }

    private void DefendOutTime()
    {
        isDefended = false;
    }

    private void Die()
    {
        if (isDefended)
        {
            return;
        }

        if (level > 0)
        {
            level--;
            return;
        }
        if (PlayerNum == 1)
        {
            PlayerManager.Instance.isP1Dead = true;
        } else if (PlayerNum == 2)
        {
            PlayerManager.Instance.isP2Dead = true;
        }
        Instantiate(explosionPrefab, transform.position, transform.rotation);
        Destroy(gameObject);
    }

    private void ReShield()
    {
        isDefended = true;
        //先获取GameObject然后激活
        GameObject shield = transform.Find("Shield").gameObject;
        shield.SetActive(true);
        shield.SendMessage("NewShield");//如果未激活则会SendMessage no receiver
    }

    private void LevelUp()
    {
        level++;
        if (level > 0)
        {
            moveSpeed = 5;
        }
        if (level > 2)
        {
            fireTimeController = 0.2f;
        }
    }
}