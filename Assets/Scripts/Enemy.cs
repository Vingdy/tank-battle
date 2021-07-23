using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

//TODO:奖励实现
public class Enemy : MonoBehaviour
{
    //
    public float moveSpeed = 3;

    //
    private SpriteRenderer sr;
    private float v = -1, h;

    public Sprite[] tankSprite;//上右下左 

    public GameObject bulletPrefab;
    public GameObject explosionPrefab;

    public Vector3 bulletRulerAngles;

    private float timeVal;//定时器
    private float timeValChangeDirection = 2;

    private void Awake()
    {
        sr = GetComponent<SpriteRenderer>();//��ȡ��Ⱦ
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // ���̶�ʱ��ִ��
    // Update is called once per frame
    void Update()
    {
        // 妄图使用子弹bullet来处理
        // if (bulletPrefab)
        // {
        //     Attack();
        // }
        if (timeVal >= 3)
        {
            Attack();
        }
        else
        {
            timeVal += Time.deltaTime;
        }

 
    }

    //�̶�ʱ��ִ�У��������
    private void FixedUpdate()
    {
        Move();
    }

    //̹�˹�������
    private void Attack()
    {
            Instantiate(bulletPrefab, transform.position, Quaternion.Euler(transform.eulerAngles+bulletRulerAngles));
            timeVal = 0;
    }

    //̹���ƶ�����
    private void Move()
    {
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
        
        transform.Translate(Vector3.up * v * moveSpeed * Time.fixedDeltaTime, Space.World);//��ֱ�᷽��*����*�ƶ��ٶ�,����������ϵ�ƶ�
        if (v < 0)
        {
            sr.sprite = tankSprite[2];
            bulletRulerAngles = new Vector3(0, 0, 180);
        }
        else if (v > 0)
        {
            sr.sprite = tankSprite[0];
            bulletRulerAngles = new Vector3(0, 0, 0);
        }
        if (v != 0)
        {
            return;
        }
        
        transform.Translate(Vector3.right * h * moveSpeed * Time.fixedDeltaTime, Space.World);//ˮƽ�᷽��*����*�ƶ��ٶ�,����������ϵ�ƶ�
        if (h < 0)
        {
            sr.sprite = tankSprite[3];
            bulletRulerAngles = new Vector3(0, 0, 90);
        }
        else if (h > 0)
        {
            sr.sprite = tankSprite[1];
            bulletRulerAngles = new Vector3(0, 0, -90);
        }
    }
    
    private void Die()
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
}
