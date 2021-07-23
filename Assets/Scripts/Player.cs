using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

//一个神奇的bug，数组初始化为空，重新复制粘贴脚本后修复
//TODO：动画切换
//TODO:more tank
public class Player : MonoBehaviour
{
    public float moveSpeed = 3;

    private SpriteRenderer sr;

    public Sprite[] tankSprite;//上右下左 

    public GameObject bulletPrefab;
    public GameObject explosionPrefab;

    public Vector3 bulletRulerAngles;

    private bool isDefended = true;

    private float timeVal = 0.4f;//定时器

    public AudioSource moveAudio;
    public AudioClip[] tankAudio;
     private void Awake()
    {
        sr = GetComponent<SpriteRenderer>();//获取渲染组件
    }

    // Start is called before the first frame update
    void Start()
    {
        Debug.Log(tankAudio.Length);
    }

    // Update is called once per frame
    void Update()
    {
        if (timeVal >= 0.4f)
        {
            Attack();
        }
        else
        {
            timeVal += Time.fixedDeltaTime;
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
        if(Input.GetKeyDown(KeyCode.Space))
        {
            Instantiate(bulletPrefab, transform.position, Quaternion.Euler(transform.eulerAngles+bulletRulerAngles));
            timeVal = 0;
        }
        
    }

    private void Move()
    {
        float v = Input.GetAxisRaw("Vertical");

        transform.Translate(Vector3.up * v * moveSpeed * Time.fixedDeltaTime,
            Space.World);
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
        
        float h = Input.GetAxisRaw("Horizontal");
        transform.Translate(Vector3.right * h * moveSpeed * Time.fixedDeltaTime,
            Space.World);
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
        Debug.Log(tankAudio.Length);
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

        PlayerManager.Instance.isDead = true;
        Instantiate(explosionPrefab, transform.position, transform.rotation);
        Destroy(gameObject);
    }
}