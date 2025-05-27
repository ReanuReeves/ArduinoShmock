using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro; // Assuming you are using TextMeshPro for text display

public class TutorialOneText : MonoBehaviour
{
    public string text;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(PrintText(text));
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    IEnumerator PrintText(string text)
    {
        // This coroutine will print the text character by character
        foreach (char c in text)
        {
            gameObject.GetComponent<TextMeshProUGUI>().text += c; // Assuming you have a TextMeshProUGUI component
            yield return new WaitForSeconds(0.05f); // Adjust the delay as needed
        }
    }
}
