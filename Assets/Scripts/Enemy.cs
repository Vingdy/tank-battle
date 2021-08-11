using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

//TODO:奖励实现
public class Enemy : MonoBehaviour
{
    public float moveSpeed = 3;

    private SpriteRenderer sr;
    private float v = -1, h;
    
    //同一个坦克不同的渲染
    //每四个为一组，0-3：普通，4-7：红，8-11：黄，12-15：绿
    //思考：是否能通过切换gameObj方式来实现
    public Sprite[] tankSprite1;
    public Sprite[] tankSprite2;
    
    public bool stopController;
    private float stopTimeVal = 0;

    public GameObject bulletPrefab;
    public GameObject explosionPrefab;

    public Vector3 bulletRulerAngles;

    public int level = 1;//等级，等同于生命，默认为1
    public bool isBonus;//奖励
    
    private float fireTimeVal = 0.4f;//开火定时器
    private float moveLengthVal = 0;//移动计时器
    private float timeValChangeDirection = 2;

    private void Awake()
    {
        sr = GetComponent<SpriteRenderer>();//当前组件的渲染变量
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (!stopController)
        {
            // stopTimeVal += Time.deltaTime;
            // if (stopTimeVal >= 5)
            // {
            //     stopController = false;
            //     stopTimeVal = 0;
            // }
            // else
            // {
            //     return;
            // }
            if (fireTimeVal >= 3)
            {
                Attack();
            }
            else
            {
                fireTimeVal += Time.deltaTime;
            }
        }
    }

    private void FixedUpdate()
    {
        if (stopController)
        {
            stopTimeVal += Time.fixedDeltaTime;
            if (stopTimeVal >= 5)
            {
                stopController = false;
                stopTimeVal = 0;
            }
            else
            {
                return;
            }
        }
        if (!stopController)
        {
            Move();
        }
    }

    private void Attack()
    {
            Instantiate(bulletPrefab, transform.position, Quaternion.Euler(transform.eulerAngles+bulletRulerAngles));
            fireTimeVal = 0;
    }

    private void Move()
    {
        // 根据level和isBonus来确定渲染数组选择
        int srNum = 0;
        if (isBonus)
        {
            srNum = 4;
        }
        else
        {
            if (level > 1)
            {
                srNum = level * 4;
            }
        }
        if (timeValChangeDirection>=2)
        {
            int num = Random.Range(0,8);
            switch (num)
            {
                case 0:
                {
                    h = 0;
                    v = 1;
                    break;
                }
                case 1:
                case 2:
                {
                    h = -1;
                    v = 0;
                    break;
                }
                case 3:
                case 4:
                {
                    h = 1;
                    v = 0;
                    break;
                }
                default:
                {
                    h = 0;
                    v = -1;
                    break;
                }
            }
            timeValChangeDirection = 0;
        }
        else
        {
            timeValChangeDirection += Time.deltaTime;
        }
        
        transform.Translate(Vector3.up * v * moveSpeed * Time.fixedDeltaTime,  Space.World);
        moveLengthVal += Mathf.Abs(v) * moveSpeed;
        
        if (v < 0)
        {
            if (moveLengthVal > 5 * moveSpeed)
            {
                sr.sprite = tankSprite2[2+srNum];
                if (moveLengthVal > 10 * moveSpeed * Time.fixedDeltaTime)
                {
                    moveLengthVal = 0;
                }
            }
            else
            {
                sr.sprite = tankSprite1[2+srNum];
            }
            bulletRulerAngles = new Vector3(0, 0, 180);
        }
        else if (v > 0)
        {
            if (moveLengthVal > 5 * moveSpeed)
            {
                sr.sprite = tankSprite2[0+srNum];
                if (moveLengthVal > 10 * moveSpeed)
                {
                    moveLengthVal = 0;
                }
            }
            else
            {
                sr.sprite = tankSprite1[0+srNum];
            }
            bulletRulerAngles = new Vector3(0, 0, 0);
        }
        if (v != 0)
        {
            return;
        }
        
        transform.Translate(Vector3.right * h * moveSpeed * Time.fixedDeltaTime,  Space.World);
        moveLengthVal += Mathf.Abs(h) * moveSpeed;
        if (h < 0)
        {
            if (moveLengthVal > 5 * moveSpeed)
            {
                sr.sprite = tankSprite2[3+srNum];
                if (moveLengthVal > 10 * moveSpeed)
                {
                    moveLengthVal = 0;
                }
            }
            else
            {
                sr.sprite = tankSprite1[3+srNum];
            }
            bulletRulerAngles = new Vector3(0, 0, 90);
        }
        else if (h > 0)
        {
            if (moveLengthVal > 5 * moveSpeed)
            {
                sr.sprite = tankSprite2[1+srNum];
                if (moveLengthVal > 10 * moveSpeed)
                {
                    moveLengthVal = 0;
                }
            }
            else
            {
                sr.sprite = tankSprite1[1+srNum];
            }
            bulletRulerAngles = new Vector3(0, 0, -90);
        }
    }
    
    private void Die()
    {
        if (isBonus)
        {
            isBonus = false;
            GameObject.Find("MapCreator").SendMessage("CreateBonus");
            return;
        }
        level--;
        if (level > 0)
        {
            return;
        }
        PlayerManager.Instance.playerScore++;
        Instantiate(explosionPrefab, transform.position, transform.rotation);
        Destroy(gameObject);
    }

    private void DieNow()
    {
        PlayerManager.Instance.playerScore++;
        Instantiate(explosionPrefab, transform.position, transform.rotation);
        Destroy(gameObject);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Enemy")
        {
            timeValChangeDirection = 4;
        }
        // throw new NotImplementedException();
    }

    private void StopNow()
    {
        stopController = true;
    }
}
