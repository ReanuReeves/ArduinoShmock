using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    public List<String> levels;
    public int currentLevelIndex = 0;

    public static LevelManager Instance { get; private set; }

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.P)) // For testing purposes, press Space to complete the level
        {
            StartCoroutine(OnLevelCompletion());
        }
    }


    public IEnumerator OnLevelCompletion()
    {
        yield return new WaitForSeconds(1f); // Wait for 1 second before loading the next level
        currentLevelIndex++;
        if (currentLevelIndex < levels.Count)
        {
            SceneManager.LoadScene(levels[currentLevelIndex]);
        }
        else
        {
            Debug.Log("All levels completed!");
            // Optionally, you can load a main menu or a completion screen here.
        }
    }
}
