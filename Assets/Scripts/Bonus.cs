using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bonus : MonoBehaviour
{
    private SpriteRenderer sr;
    
    //敌人
    // private GameObject enemy;
    
    //0:+1HP 1:砸瓦鲁多 2:heart无敌 3:Boom 4:lvUp 5:tank无敌
    //TODO:闪烁消失
    //TODO:粉重坦奖励蜜汁消失
    public Sprite[] bonusSprite;
    public int bonusNum;

    public float BonusTimeVal = 15f;
    private void Awake()
    {
        sr = GetComponent<SpriteRenderer>();//获取渲染组件
    }
    
    // Start is called before the first frame update
    void Start()
    {
        //debug
        bonusNum = Random.Range(0, 6);
        // bonusNum = 3;
        switch (bonusNum)
        {
            case 0:
            {
                sr.sprite = bonusSprite[0];
                break;
            }
            case 1:
            {
                sr.sprite = bonusSprite[1];
                break;
            }
            case 2:
            {
                sr.sprite = bonusSprite[2];
                break;
            }
            case 3:
            {
                sr.sprite = bonusSprite[3];
                break;
            }
            case 4:
            {
                sr.sprite = bonusSprite[4];
                break;
            }
            case 5:
            {
                sr.sprite = bonusSprite[5];
                break;
            }
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (BonusTimeVal <= 10f)
        {
            
        }
        else if (BonusTimeVal <= 5f)
        {
            
        }
        else
        {
            BonusTimeVal -= Time.fixedDeltaTime;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Tank")
        {
            switch (bonusNum)
            {
                case 0:
                {
                    PlayerManager.Instance.lifeVal++;
                    break;
                }
                case 1:
                {
                    PlayerManager.Instance.SendMessage("FindAllEnemyAndStop");
                    break;
                }
                case 2:
                {
                    PlayerManager.Instance.SendMessage("HeartProtect");
                    break;
                }
                case 3:
                {
                    PlayerManager.Instance.SendMessage("FindAllEnemyAndKill");
                    break;
                }
                case 4:
                {
                    collision.SendMessage("LevelUp");
                    break;
                }
                case 5:
                {
                    collision.SendMessage("ReShield");
                    break;
                }
            }
            Destroy(gameObject);
        }
        
    }
}
