using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro; // Assuming you are using TextMeshPro for text display

public class DoorInteractible : MonoBehaviour
{
    GameObject player;
    public string solution = "123"; 
    public GameObject textWindow;
    public GameObject promptWindow;

    bool isInteracting = false;
    // Start is called before the first frame update
    void OnEnable()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        ArduinoConnector.OnArduinoInputReceived += HandleArduinoInput;

    }

    void OnDisable()
    {
        ArduinoConnector.OnArduinoInputReceived -= HandleArduinoInput;
    }

    private void HandleArduinoInput(string command)
    {
        if(command == RemoteCommands.OK && Vector3.Distance(player.transform.position, transform.position) < 1.5f)
        {
            // Handle interaction logic here
            Debug.Log("Interaction command received: " + command);
            Interact();
        }
        if(command == RemoteCommands.BACK && isInteracting)
        {
            // Handle exit interaction logic here
            Debug.Log("Exit command received: " + command);
            ExitPromptWindow();
        }
        else if(isInteracting)
        {
            // switch statement to handle numbers from 0 to 9
            switch (command)
            {
                case RemoteCommands.ZERO:
                    textWindow.GetComponent<TextMeshProUGUI>().text += "0";
                    break;
                case RemoteCommands.ONE:
                    textWindow.GetComponent<TextMeshProUGUI>().text += "1";
                    break;
                case RemoteCommands.TWO:
                    textWindow.GetComponent<TextMeshProUGUI>().text += "2";
                    break;
                case RemoteCommands.THREE:
                    textWindow.GetComponent<TextMeshProUGUI>().text += "3";
                    break;
                case RemoteCommands.FOUR:
                    textWindow.GetComponent<TextMeshProUGUI>().text += "4";
                    break;
                case RemoteCommands.FIVE:
                    textWindow.GetComponent<TextMeshProUGUI>().text += "5";
                    break;
                case RemoteCommands.SIX:
                    textWindow.GetComponent<TextMeshProUGUI>().text += "6";
                    break;
                case RemoteCommands.SEVEN:
                    textWindow.GetComponent<TextMeshProUGUI>().text += "7";
                    break;
                case RemoteCommands.EIGHT:
                    textWindow.GetComponent<TextMeshProUGUI>().text += "8";
                    break;
                case RemoteCommands.NINE:
                    textWindow.GetComponent<TextMeshProUGUI>().text += "9";
                    break;
            }
        }
    }

    void Update()
    {
        if(textWindow.GetComponent<TextMeshProUGUI>().text == solution)
        {
            promptWindow.SetActive(false); // Hide the interaction text window
            textWindow.SetActive(false); // Hide the text window
            isInteracting = false; // Allow re-entry into interaction logic
            gameObject.SetActive(false); // Deactivate the door interactible object
            player.GetComponent<PlayerMovement>().enabled = true; // Re-enable player movement
        }
        else if(textWindow.GetComponent<TextMeshProUGUI>().text.Length > 3)
        {
            textWindow.GetComponent<TextMeshProUGUI>().text = ""; // Clear the text if it exceeds 3 characters
        }
    }

    void Interact()
    {
        if (!isInteracting)
        {
            isInteracting = true; // Prevent re-entry into interaction logic
            promptWindow.SetActive(true); // Show the interaction text window
            textWindow.SetActive(true); // Show the text window for input
            player.GetComponent<PlayerMovement>().enabled = false; // Disable player movement during interaction
        }
    }

    void ExitPromptWindow()
    {
        promptWindow.SetActive(false); // Hide the interaction text window
        isInteracting = false; // Allow re-entry into interaction logic
        textWindow.SetActive(false); // Hide the text window
        textWindow.GetComponent<TextMeshProUGUI>().text = ""; // Clear the text after interaction
        player.GetComponent<PlayerMovement>().enabled = true; // Re-enable player movement
    }



    
}
