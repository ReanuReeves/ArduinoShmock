using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO.Ports;
using System.Threading;
using UnityEditor;
using UnityEngine.SceneManagement;
using Unity.VisualScripting;

public class ArduinoConnector : MonoBehaviour
{
    private SerialPort serialPort = new SerialPort("COM7", 115200);
    private Thread serialThread;
    private bool isRunning = true;
    private Queue<string> serialQueue = new Queue<string>(); // Thread-safe queue for serial data
    private readonly object queueLock = new object();


    public Transform fireOutput;


    public delegate void ArduinoInputReceived(string command);
    public static event ArduinoInputReceived OnArduinoInputReceived;

    void Start()
    {
        serialPort.ReadTimeout = 100;
        serialPort.DtrEnable = true;

        try
        {
            serialPort.Open();
            Debug.Log("Serial port opened successfully.");

            // Start the thread for reading serial data
            serialThread = new Thread(ReadSerialData);
            serialThread.Start();
        }
        catch (System.Exception e)
        {
            Debug.LogError("Failed to open serial port: " + e.Message);
        }

        DontDestroyOnLoad(gameObject); // Keep this object alive across scenes
    }


    

    void Update()
    {
        // Process data from the queue on the main thread
        lock (queueLock)
        {
            while (serialQueue.Count > 0)
            {
                string data = serialQueue.Dequeue();
                OnArduinoInputReceived?.Invoke(data);
            }
        }

        // Simulate input for testing purposes
        if (EditorApplication.isPlaying)
        {
            SimulateInput();
        }
    }

    
    

    IEnumerator FireDetected()
    {
        if(fireOutput == null)
        {
            fireOutput = GameObject.Find("FireHolder").transform.GetChild(0); // Try to find the fire output GameObject
        }
        if (fireOutput != null)
        {
            Debug.Log("Fire detected! Triggering fire output.");
            fireOutput.gameObject.SetActive(true);
            yield return new WaitForSeconds(2f); // Show fire output for 2 seconds
            fireOutput.gameObject.SetActive(false);
        }
        
    }

    void ReadSerialData()
    {
        if(!serialPort.IsOpen)
        {
            Debug.LogError("Serial port is not open. Exiting thread.");
            return;
        }
        while (isRunning)
        {
            try
            {
                if (serialPort != null && serialPort.IsOpen)
                {
                    string data = serialPort.ReadLine().Trim();
                    lock (queueLock)
                    {
                        if(data == "FIRE_DETECTED")
                        {
                            Debug.Log("Fire detected!");

                            StartCoroutine(FireDetected()); // Trigger fire detection coroutine
                            continue; // Skip adding this data to the queue
                        }
                        
                        serialQueue.Enqueue(data); // Add data to the queue
                    }
                }
            }
            catch (System.TimeoutException)
            {
                // Ignore timeout exceptions
            }
            catch (System.Exception e)
            {
                Debug.LogError("Error reading serial data: " + e.Message);
            }
        }
    }

    void SimulateInput()
    {
        if (Input.GetKeyDown(KeyCode.W))
        {
            OnArduinoInputReceived?.Invoke(RemoteCommands.UP);
        }
        else if (Input.GetKeyDown(KeyCode.S))
        {
            OnArduinoInputReceived?.Invoke(RemoteCommands.DOWN);
        }
        else if (Input.GetKeyDown(KeyCode.A))
        {
            OnArduinoInputReceived?.Invoke(RemoteCommands.LEFT);
        }
        else if (Input.GetKeyDown(KeyCode.D))
        {
            OnArduinoInputReceived?.Invoke(RemoteCommands.RIGHT);
        }
        else if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            OnArduinoInputReceived?.Invoke(RemoteCommands.PROGRAM_UP);
        }
        else if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            OnArduinoInputReceived?.Invoke(RemoteCommands.PROGRAM_DOWN);
        }
        else if (Input.GetKeyDown(KeyCode.Space))
        {
            OnArduinoInputReceived?.Invoke(RemoteCommands.OK);
        }
        else if (Input.GetKeyDown(KeyCode.Escape))
        {
            OnArduinoInputReceived?.Invoke(RemoteCommands.BACK);
        }
        // else for numbers
        else if(Input.GetKeyDown(KeyCode.Alpha0))
        {
            OnArduinoInputReceived?.Invoke(RemoteCommands.ZERO);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            OnArduinoInputReceived?.Invoke(RemoteCommands.ONE);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            OnArduinoInputReceived?.Invoke(RemoteCommands.TWO);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            OnArduinoInputReceived?.Invoke(RemoteCommands.THREE);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            OnArduinoInputReceived?.Invoke(RemoteCommands.FOUR);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha5))
        {
            OnArduinoInputReceived?.Invoke(RemoteCommands.FIVE);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha6))
        {
            OnArduinoInputReceived?.Invoke(RemoteCommands.SIX);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha7))
        {
            OnArduinoInputReceived?.Invoke(RemoteCommands.SEVEN);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha8))
        {
            OnArduinoInputReceived?.Invoke(RemoteCommands.EIGHT);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha9))
        {
            OnArduinoInputReceived?.Invoke(RemoteCommands.NINE);
        }
        else if(Input.GetKeyDown(KeyCode.F))
        {
            
            
            StartCoroutine(FireDetected()); // Trigger fire detection coroutine
        }
    }

    private void OnApplicationQuit()
    {
        isRunning = false;

        if (serialThread != null && serialThread.IsAlive)
        {
            serialThread.Join(); // Wait for the thread to finish
        }

        if (serialPort != null && serialPort.IsOpen)
        {
            serialPort.Close();
            Debug.Log("Serial port closed.");
        }
    }
}