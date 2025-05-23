using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO.Ports;
using System.Threading;
using UnityEditor;

public class ArduinoConnector : MonoBehaviour
{
    private SerialPort serialPort = new SerialPort("COM7", 115200);
    private Thread serialThread;
    private bool isRunning = true;
    private Queue<string> serialQueue = new Queue<string>(); // Thread-safe queue for serial data
    private readonly object queueLock = new object();

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