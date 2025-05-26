using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class PlayerMovement : MonoBehaviour
{
    public LayerMask notObstructing;
    private Dictionary<string, Vector3> commandToDirection;

    IEnumerator moveCoroutine; // Reference to the coroutine for movement

    private void Awake()
    {
        // Initialize the dictionary mapping commands to directions
        commandToDirection = new Dictionary<string, Vector3>
        {
            { RemoteCommands.UP, Vector3.up },
            { RemoteCommands.DOWN, Vector3.down },
            { RemoteCommands.LEFT, Vector3.left },
            { RemoteCommands.RIGHT, Vector3.right }
        };
    }

    private void OnEnable()
    {
        ArduinoConnector.OnArduinoInputReceived += HandleArduinoInput;
    }

    private void OnDisable()
    {
        ArduinoConnector.OnArduinoInputReceived -= HandleArduinoInput;
    }

    private void HandleArduinoInput(string command)
    {
        if (commandToDirection.TryGetValue(command, out Vector3 direction))
        {
            if(moveCoroutine != null)
            {
                return; // Prevent starting a new coroutine if one is already running
            }

            RaycastHit hit;
            Debug.Log("Command: " + command + " Direction: " + direction);
            if (!Physics.Raycast(transform.position, direction, out hit, 1f, notObstructing))
            {
                moveCoroutine = Move(direction);
                StartCoroutine(moveCoroutine); // Start the movement coroutine
            }
           
        }
    }

    IEnumerator Move(Vector3 direction)
    {
        for(int i = 0; i < 50; i++)
        {
            // Move the character in the direction pressed
            transform.position += direction * 0.02f;
            yield return new WaitForSeconds(0.01f); // Wait for a short duration before moving again
        }
        moveCoroutine = null; // Reset the coroutine reference when done
        
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Objective"))
        {
            Debug.Log("Objective reached!");
        }
    }
}