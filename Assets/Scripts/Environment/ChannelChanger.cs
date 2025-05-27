using UnityEngine;
using UnityEngine.SceneManagement; // Required for scene management
using System.Collections.Generic; // Required for List<T>

public class ChannelChanger : MonoBehaviour
{
    // Singleton instance
    public static ChannelChanger Instance { get; private set; }

    List<GameObject> channelList;
    int currentChannelIndex = 0;

    PlayerMovement playerMovement;
    CameraShake cameraShake; // Reference to the CameraShake script
    GameObject channelHolder;

    private void Awake()
    {
        // Ensure only one instance of ChannelChanger exists
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

    void Start()
    {
        playerMovement = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerMovement>();
        cameraShake = Camera.main.GetComponent<CameraShake>(); // Assuming the main camera has the CameraShake script

        RefreshChannelList(); // Initialize the channel list
    }

    

    private void OnEnable()
    {
        ArduinoConnector.OnArduinoInputReceived += HandleArduinoInput;
        SceneManager.sceneLoaded += OnSceneLoaded; // Subscribe to sceneLoaded event
    }

    private void OnDisable()
    {
        ArduinoConnector.OnArduinoInputReceived -= HandleArduinoInput;
        SceneManager.sceneLoaded -= OnSceneLoaded; // Unsubscribe from sceneLoaded event
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        RefreshChannelList(); // Refresh the channel list when a new scene is loaded
        playerMovement = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerMovement>();
        cameraShake = Camera.main.GetComponent<CameraShake>(); // Reassign the CameraShake script reference
        currentChannelIndex = 0; // Reset to the first channel when a new scene is loaded
    }

    void RefreshChannelList()
    {
        if (channelList != null)
        {
            channelList.Clear();
        }
        else
        {
            channelList = new List<GameObject>();
        }

        channelHolder = GameObject.Find("ChannelHolder");
        if (channelHolder != null)
        {
            for (int i = 0; i < channelHolder.transform.childCount; i++)
            {
                channelList.Add(channelHolder.transform.GetChild(i).gameObject);
            }
        }
    }

    void HandleArduinoInput(string command)
    {
        if (command == RemoteCommands.PROGRAM_UP)
        {
            SwitchChannelUp();
        }
        else if (command == RemoteCommands.PROGRAM_DOWN)
        {
            SwtichChannelDown();
        }
    }

    public int GetCurrentChannelIndex()
    {
        return currentChannelIndex;
    }

    void SwitchChannelUp()
    {
        channelList[currentChannelIndex].SetActive(false);
        int previousChannelIndex = currentChannelIndex;

        if (currentChannelIndex + 1 == channelList.Count)
        {
            currentChannelIndex = 0;
        }
        else
        {
            currentChannelIndex++;
        }

        channelList[currentChannelIndex].SetActive(true);

        // Check for collision and revert if necessary
        if (CheckPlayerCollision())
        {
            channelList[currentChannelIndex].SetActive(false);
            currentChannelIndex = previousChannelIndex;
            channelList[currentChannelIndex].SetActive(true);
        }
    }

    void SwtichChannelDown()
    {
        channelList[currentChannelIndex].SetActive(false);
        int previousChannelIndex = currentChannelIndex;

        if (currentChannelIndex - 1 < 0)
        {
            currentChannelIndex = channelList.Count - 1;
        }
        else
        {
            currentChannelIndex--;
        }

        channelList[currentChannelIndex].SetActive(true);

        // Check for collision and revert if necessary
        if (CheckPlayerCollision())
        {
            channelList[currentChannelIndex].SetActive(false);
            currentChannelIndex = previousChannelIndex;
            channelList[currentChannelIndex].SetActive(true);
        }
    }

    bool CheckPlayerCollision()
    {
        Collider playerCollider = playerMovement.GetComponent<Collider>();

        if (playerCollider != null)
        {
            // Iterate through all child objects of the current channel
            foreach (Transform child in channelList[currentChannelIndex].transform)
            {
                Collider childCollider = child.GetComponent<Collider>();
                if (childCollider != null && playerCollider.bounds.Intersects(childCollider.bounds))
                {
                    // Trigger camera shake
                    if (cameraShake != null)
                    {
                        StartCoroutine(cameraShake.Shake(0.3f, 0.1f)); // Adjust duration and magnitude as needed
                    }
                    return true; // Collision detected
                }
            }
        }

        return false; // No collision detected
    }
}