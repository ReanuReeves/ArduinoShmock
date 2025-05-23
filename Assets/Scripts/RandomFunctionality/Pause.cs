using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pause : MonoBehaviour
{
    bool paused = false;
    float timeScale = 1f;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    private void OnEnable()
    {
        ArduinoConnector.OnArduinoInputReceived += HandleArduinoInput;
    }

    private void OnDisable()
    {
        ArduinoConnector.OnArduinoInputReceived -= HandleArduinoInput;
    }

    void HandleArduinoInput(string command)
    {
        if (command == RemoteCommands.STOP)
        {
            if (!paused)
            {
                timeScale = Time.timeScale;
                Time.timeScale = 0;
                paused = true;
            }
            else
            {
                Time.timeScale = timeScale;
                paused = false;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        // if the pause button is pressed pause the game, unpause when it is hit again
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (!paused)
            {
                timeScale = Time.timeScale;
                Time.timeScale = 0;
            }
            else
            {
                Time.timeScale = timeScale;
            }
        }
    }
}
