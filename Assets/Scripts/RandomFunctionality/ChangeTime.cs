using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeTime : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    

    // Update is called once per frame
    void Update()
    {
        if(Time.timeScale == 0)
        {
            return;
        }

        if(Input.GetKeyDown(KeyCode.Alpha1)) // replace with slow down and FF
        {
            if(Time.timeScale == 1)
            {
                Time.timeScale = 0.5f;
            }
            else if(Time.timeScale > 1)
            {
                Time.timeScale = 1;
            }
        }

        if(Input.GetKeyDown(KeyCode.Alpha2)) // replace with slow down and FF
        {
            if(Time.timeScale == 1)
            {
                Time.timeScale = 2f;
            }
            else if(Time.timeScale < 1 && Time.timeScale > 0)
            {
                Time.timeScale = 1;
            }
        }
    }
}
