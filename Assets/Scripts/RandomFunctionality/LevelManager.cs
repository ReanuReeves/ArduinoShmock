using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    public List<Scene> levels;
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


    public void OnLevelCompletion()
    {
        currentLevelIndex++;
        if (currentLevelIndex < levels.Count)
        {
            SceneManager.LoadScene(levels[currentLevelIndex].name);
        }
        else
        {
            Debug.Log("All levels completed!");
            // Optionally, you can load a main menu or a completion screen here.
        }
    }
}
