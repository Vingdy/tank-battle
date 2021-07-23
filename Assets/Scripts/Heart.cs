using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Heart : MonoBehaviour
{
    private SpriteRenderer sr;
    public GameObject explosionPrefab;
    public AudioClip dieAudio;//音频组件

    public Sprite BrokenSprite;
    // Start is called before the first frame update
    void Start()
    {
        sr = GetComponent < SpriteRenderer >();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Die()
    {
        sr.sprite = BrokenSprite;
        Instantiate(explosionPrefab, transform.position, transform.rotation);
        PlayerManager.Instance.isDefeat = true;
        AudioSource.PlayClipAtPoint(dieAudio, transform.position);
    }
}
