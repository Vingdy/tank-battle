using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public GameObject MapCreator;
    
    public float moveSpeed = 10;

    public bool isPlayerBullet;

    public int level = 0;
    
    public AudioClip HitAudio;
    // Start is called before the first frame update
    void Start()
    {
        MapCreator = GameObject.Find("MapCreator");
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(transform.up*moveSpeed*Time.deltaTime, Space.World);//right为x轴，up为y轴，forward为z轴
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        switch(collision.tag) {
             case "Tank":
                 if (!isPlayerBullet)
                 {
                     collision.SendMessage("Die");//发送给碰撞体的调用方法
                     Destroy(gameObject);//销毁自身
                     PlayHitAudio();
                 }
                 break;
             case "Heart":
                 collision.SendMessage("Die");
                 Destroy(gameObject);//销毁自身
                 PlayHitAudio();
                 break;
             case "Enemy":
                 if (isPlayerBullet)
                 {
                     collision.SendMessage("Die");
                     Destroy(gameObject);
                     PlayHitAudio();
                 }
                 break;
             case "Wall":
                 if (isPlayerBullet)
                 {
                     PlayHitAudio();
                 }
                 Destroy(collision.gameObject);//销毁墙
                 Destroy(gameObject);//销毁自身
                 MapCreator.SendMessage("DelPosition", collision.gameObject.transform.position);
                 break;
             case "Barrier":
                 if (level > 2)
                 {
                     Destroy(collision.gameObject);
                     MapCreator.SendMessage("DelPosition", collision.gameObject.transform.position);
                 }
                 Destroy(gameObject);
                 break;
             default:
                 break;
        }
        // throw new NotImplementedException();
    }
    
    public void PlayHitAudio()
    {
        AudioSource.PlayClipAtPoint(HitAudio, transform.position);
    }

    private void LevelUp()
    {
        level++;
    }
}