using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WinningPosition : MonoBehaviour
{
    LevelManager levelManager;
    // Start is called before the first frame update
    void Start()
    {
        levelManager = LevelManager.Instance;
    }

    // when the player reaches this position, it will trigger the level completion
    void OnTriggerEnter(Collider other)
    {
        Debug.Log("Trigger entered by: " + other.gameObject.name);
        if (other.CompareTag("Player"))
        {
            Debug.Log("Player has reached the winning position!");
            StartCoroutine(levelManager.OnLevelCompletion());
        }
    }
}
