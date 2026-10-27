using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    // Singleton instance for global access point
    public static UIManager Instance { get; private set; }

    [Header("Core State Tracking")]
    [SerializeField] private GameState currentState;
    public GameState CurrentState => currentState;

    [Header("Loading Subsystem")]
    [SerializeField] private GameObject loadingScreenPanel;
    [SerializeField] private Slider loadingProgressBar;

    [Header("UI Canvas Overlays")]
    [SerializeField] private GameObject mainMenuPanel;
    [SerializeField] private GameObject pauseMenuPanel;
    [SerializeField] private GameObject upgradeMenuPanel;
    [SerializeField] private GameObject gameOverPanel;

    private void Awake()
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

    private void Start()
    {
        // Set up the initial visual layout for the Main Menu
        ConfigureSystemSettingsForState(GameState.Menu);
    }

    /// <summary>
    /// The primary Facade method. Call this whenever you want to change state or shift scenes.
    /// </summary>
    public void ChangeState(GameState newState, string sceneToLoad = "")
    {
        currentState = newState;

        // Condition: If a scene name is provided, execute the async scene migration process first.
        if (!string.IsNullOrEmpty(sceneToLoad))
        {
            StartCoroutine(LoadSceneAndTransitionStateAsync(sceneToLoad, newState));
        }
        else
        {
            // Immediate state modification (no scene transition needed)
            ConfigureSystemSettingsForState(newState);
        }
    }

    /// <summary>
    /// Smoothly runs an asynchronous scene load while keeping the loading bar animated, 
    /// applying the requested GameState updates precisely when the screen shifts.
    /// </summary>
    private IEnumerator LoadSceneAndTransitionStateAsync(string sceneName, GameState targetState)
    {
        // 1. Immediately block the user's view with the loading screen
        loadingScreenPanel.SetActive(true);
        loadingProgressBar.value = 0f;

        // Ensure time scale is unpaused so the scene loading thread isn't choked
        Time.timeScale = 1f; 

        // 2. Start loading files on a background worker thread
        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneName);

        // 3. Keep updating the slider via an Iterator routine until fully complete
        while (!operation.isDone)
        {
            float progress = Mathf.Clamp01(operation.progress / 0.9f);
            loadingProgressBar.value = progress;

            yield return null; // Pauses execution and returns processing to Unity for the frame
        }

        // 4. Scene loading finished! Configure the new structural states, timelines, and panels
        ConfigureSystemSettingsForState(targetState);
        // 5. Hide the loading overlay cleanly
        loadingScreenPanel.SetActive(false);
    }

    /// <summary>
    /// Internal helper method to route data settings, activate layouts, adjust cursors, 
    /// and safely alter Unity's internal engine clock speeds.
    /// </summary>
    private void ConfigureSystemSettingsForState(GameState state)
    {
        switch (state)
        {
            case GameState.Menu:
                Time.timeScale = 1f;
                SetCursorStatus(true);
                ToggleUIPanels(mainMenu: true, pause: false, upgrade: false, over: false);
                break;
            
            case GameState.Playing:
                Time.timeScale = 1f;
                SetCursorStatus(false);
                ToggleUIPanels(mainMenu: false, pause: false, upgrade: false, over: false);
                break;
            
            case GameState.ChoosingUpgrade:
                Time.timeScale = 0f; // Freeze core background processes
                SetCursorStatus(true);
                ToggleUIPanels(mainMenu: false, pause: false, upgrade: true, over: false);
                break;
            
            case GameState.Paused:
                Time.timeScale = 0f; // Freeze physics and frame updates
                SetCursorStatus(true);
                ToggleUIPanels(mainMenu: false, pause: true, upgrade: false, over: false);
                break;
            
            case GameState.GameOver:
                Time.timeScale = 0f;
                SetCursorStatus(true);
                ToggleUIPanels(mainMenu: false, pause: false, upgrade: false, over: true);
                break;
        }
    }

    private void SetCursorStatus(bool visible)
    {
        Cursor.lockState = visible ? CursorLockMode.None : CursorLockMode.Locked;
        Cursor.visible = visible;
    }

    private void ToggleUIPanels(bool mainMenu, bool pause, bool upgrade, bool over)
    {
        if (mainMenuPanel != null) mainMenuPanel.SetActive(mainMenu);
        if (pauseMenuPanel != null) pauseMenuPanel.SetActive(pause);
        if (upgradeMenuPanel != null) upgradeMenuPanel.SetActive(upgrade);
        if (gameOverPanel != null) gameOverPanel.SetActive(over);
    }
}
