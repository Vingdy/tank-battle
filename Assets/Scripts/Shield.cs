using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shield : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        gameObject.SetActive(true);
        Invoke("Close", 3.0f);
        gameObject.SendMessageUpwards("DefendOutTime");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void Close()
    {
        gameObject.SetActive(false);
    }
    
    private void NewShield()
    {
        Invoke("Close", 5.0f);
        gameObject.SendMessageUpwards("DefendOutTime");
    }
}
