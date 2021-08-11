using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shield : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Destroy(gameObject, 3.0f);
        gameObject.SendMessageUpwards("DefendOutTime");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void NewShield()
    {
        Destroy(gameObject, 5.0f);
        gameObject.SendMessageUpwards("DefendOutTime");
    }
}
