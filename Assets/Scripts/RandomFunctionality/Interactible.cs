using System.Collections;
using System.Collections.Generic;
// using System.Numerics;
using UnityEngine;
using TMPro; // Assuming you are using TextMeshPro for text display

public class Interactible : MonoBehaviour
{
    GameObject player;
    public GameObject textWindow;
    public string interactionText = "You have interacted with the object!";
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
    }

    void Interact()
    {
        textWindow.SetActive(true); // Show the interaction text window
        StartCoroutine(PrintText(interactionText));
    }

    IEnumerator PrintText(string text)
    {
        TextMeshProUGUI textComponent = textWindow.GetComponentInChildren<TextMeshProUGUI>();
        textComponent.text = ""; // Clear previous text
        foreach (char c in text)
        {
            textComponent.text += c; // Append each character
            yield return new WaitForSeconds(0.05f); // Adjust the delay as needed
        }

        yield return new WaitForSeconds(2f); // Wait for a moment before hiding the text window
        textWindow.SetActive(false); // Hide the interaction text window
        textComponent.text = ""; // Clear the text after interaction
    }
    
}
