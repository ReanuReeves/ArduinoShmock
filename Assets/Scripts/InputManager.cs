using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    // Singleton instance
    public static InputManager Instance { get; private set; }

    // Queue to store commands
    private Queue<string> commandQueue = new Queue<string>();

    private void Awake()
    {
        // Ensure only one instance of InputManager exists
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // Persist across scenes
        }
        else
        {
            Destroy(gameObject); // Destroy duplicate instances
        }
    }

    // Add a command to the queue
    public void AddCommand(string command)
    {
        commandQueue.Enqueue(command);
    }

    // Get the next command from the queue
    public string GetNextCommand()
    {
        if (commandQueue.Count > 0)
        {
            return commandQueue.Dequeue();
        }
        return null; // Return null if the queue is empty
    }

    public string PeekNextCommand()
    {
        if (commandQueue.Count > 0)
        {
            return commandQueue.Peek();
        }
        return null; // Return null if the queue is empty
    }

    // Check if there are commands in the queue
    public bool HasCommands()
    {
        return commandQueue.Count > 0;
    }

   
}