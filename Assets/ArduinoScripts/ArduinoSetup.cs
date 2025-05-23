using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO.Ports;

public class ArduinoSetup : MonoBehaviour
{


    SerialPort sp = new SerialPort("COM7", 115200); 
    bool isStreaming = false;

    bool ledOn = false; 
    void OpenConnection()
    {
        isStreaming = true;
        sp.ReadTimeout = 100;
        sp.Open();
        sp.DtrEnable = true; // Enable DTR (Data Terminal Ready) signal
    }
    // Start is called before the first frame update
    void Start()
    {
        OpenConnection();
    }

    // Update is called once per frame
    void Update()
    {
        if(isStreaming)
        {
            string input = ReadSerialPort(50); // Read from the serial port with a 50ms timeout
            if (!string.IsNullOrEmpty(input))
            {
                ProcessInput(input);
            }
            
        }
    }


    string ReadSerialPort(int timeout = 50)
    {
        string message;
        sp.ReadTimeout = timeout; // Set the read timeout to 50ms
        try
        {
            message = sp.ReadLine(); // Read a line from the serial port
            return message;
        }
        catch (System.Exception e)
        {
            return null; // Return null if an error occurs
        }
    }


   void ProcessInput(string input)
    {
        // Check if the input matches any value in RemoteCommands
        foreach (var field in typeof(RemoteCommands).GetFields())
        {
            if (field.GetValue(null).ToString() == input)
            {
                string commandName = field.Name; // Get the name of the command (e.g., "UP", "DOWN")
                // InputManager.Instance.AddCommand(commandName); // Add the command to the InputManager queue
                Debug.Log($"Command added: {commandName}");
                return;
            }
        }

        Debug.LogWarning($"Unknown input received: {input}");
    }


    void OnDisable()
    {
        sp.Close();
        isStreaming = false;
    }
}
