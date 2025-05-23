using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro; // Assuming you are using TextMeshPro for text rendering

public class ChannelText : MonoBehaviour
{
    TextMeshProUGUI channelText; // Reference to the TextMeshProUGUI component
    ChannelChanger channelChanger; // Reference to the ChannelChanger script
    // Start is called before the first frame update
    void Start()
    {
        channelChanger = ChannelChanger.Instance; // Get the instance of ChannelChanger
        channelText = GetComponent<TextMeshProUGUI>(); // Get the TextMeshProUGUI component attached to this GameObject

        if (channelText == null)
        {
            Debug.LogError("TextMeshProUGUI component not found on this GameObject.");
            return;
        }
    }

    // Update is called once per frame
    void Update()
    {
        channelText.text = "Channel: " + (channelChanger.GetCurrentChannelIndex() + 1); // Update the text with the current channel index
    }
}
