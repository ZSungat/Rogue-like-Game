using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

public class Optimizations : MonoBehaviour
{
    public int GameFrameRate = 144;
    public int MainMenuFrameRate = 60;

    private void Start()
    {
        // Disable VSync to allow custom frame rate limiting
        QualitySettings.vSyncCount = 0;

        // Register to the scene loaded event
        SceneManager.sceneLoaded += OnSceneLoaded;

        // Set initial frame rate based on current scene
        UpdateFrameRate();
    }

    private void OnDestroy()
    {
        // Unregister from the scene loaded event when object is destroyed
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        UpdateFrameRate();
        ManageEventSystems();
    }

    private void UpdateFrameRate()
    {
        string currentSceneName = SceneManager.GetActiveScene().name;

        if (GameFrameRate < 30)
        {
            if (GameFrameRate == 0)
            {
                Debug.Log($"Framerate limit is set to 0. Resetting Framerate limit to Unlimited for game scene");
                return;
            }
            else
            {
                GameFrameRate = 30;

                Debug.Log($"Framerate limit of the Game is less than 30. Resetting Framerate limit to {GameFrameRate} for the {currentSceneName} scene");
            }
        }
        else if (MainMenuFrameRate < 30)
        {

            if (MainMenuFrameRate == 0)
            {
                Debug.Log($"Framerate limit is set to 0. Resetting Framerate limit to Unlimited for game scene");
                return;
            }
            else
            {
                MainMenuFrameRate = 30;

                Debug.Log($"Framerate limit of the Main Menu is less than 30. Resetting Framerate limit to {MainMenuFrameRate} for the {currentSceneName} scene");
            }
        }


        switch (currentSceneName)
        {
            case "Pc Game" or "Mobile Game":
                Application.targetFrameRate = GameFrameRate;
                Debug.Log($"Set frame rate limit to {GameFrameRate} for the scene: {currentSceneName}");
                break;
            case "PC Main Menu" or "Mobile Main Menu":
                Application.targetFrameRate = MainMenuFrameRate;
                Debug.Log($"Set frame rate limit to {MainMenuFrameRate} for the scene: {currentSceneName}");
                break;
            default:
                // Default to game frame rate for any other scenes
                Application.targetFrameRate = MainMenuFrameRate;
                Debug.Log($"Set frame rate limit to {MainMenuFrameRate} for the scene: {currentSceneName}");
                break;
        }

    }

    private void ManageEventSystems()
    {
        // Find all EventSystems in the scene
        EventSystem[] eventSystems = FindObjectsOfType<EventSystem>();

        if (eventSystems != null && eventSystems.Length > 1)
        {
            Debug.Log($"Found {eventSystems.Length} EventSystems. Cleaning up duplicates...");

            // Keep the first EventSystem and destroy others
            for (int i = 1; i < eventSystems.Length; i++)
            {
                Debug.Log($"Destroying duplicate EventSystem on {eventSystems[i].gameObject.name}");
                Destroy(eventSystems[i].gameObject);
            }
        }
        else if (eventSystems != null && eventSystems.Length == 0)
        {
            // If no EventSystem exists, create one
            Debug.Log("No EventSystem found. Creating new EventSystem...");
            GameObject eventSystem = new GameObject("EventSystem");
            eventSystem.AddComponent<EventSystem>();
            eventSystem.AddComponent<StandaloneInputModule>();
        }
    }
}
