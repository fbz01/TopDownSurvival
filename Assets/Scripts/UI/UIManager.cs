using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [Header("Loading Subsystem")]
    [SerializeField] private GameObject loadingScreenPanel;
    [SerializeField] private Slider loadingProgressBar;

    [Header("UI Canvas Overlays")]
    [SerializeField] private GameObject mainMenuPanel;
    [SerializeField] private GameObject pauseMenuPanel;
    [SerializeField] private GameObject upgradeMenuPanel;
    [SerializeField] private GameObject gameOverPanel;

    private void OnEnable()
    {
        // 1. Subscribe to the global game event system (Observer Pattern)
        GameEvents.OnGameStateChanged += HandleGameStateChanged;
    }

    private void OnDisable()
    {
        // 2. IMPORTANT: Unsubscribe to prevent memory leaks if the UI is destroyed
        GameEvents.OnGameStateChanged -= HandleGameStateChanged;
    }

    private void Start()
    {
        ConfigureSystemSettings(GameState.Menu);
    }

    /// <summary>
    /// The event handler that responds whenever the GameEvents publisher fires an update
    /// </summary>
    private void HandleGameStateChanged(GameState newState)
    {
        // Route settings immediately based on the state choice
        switch (newState)
        {
            case GameState.Menu:
                EvaluateSceneTransition("StartScene", newState);
                break;
            
            case GameState.Playing:
                EvaluateSceneTransition("Test_A", newState);
                break;

            case GameState.ChoosingUpgrade:
                ConfigureSystemSettings(newState);
                break;

            case GameState.Paused:
                ConfigureSystemSettings(newState);
                break;

            case GameState.GameOver:
                ConfigureSystemSettings(newState);
                break;
        }
    }
    
    /// <summary>
    /// Checks the actively running scene name context against the targeted objective.
    /// Triggers background loaders only if shifting between boundaries.
    /// </summary>
    private void EvaluateSceneTransition(string targetSceneName, GameState targetState)
    {
        if (SceneManager.GetActiveScene().name != targetSceneName)
        {
            StartCoroutine(LoadSceneAndTransitionStateAsync(targetSceneName, targetState));
        }
        else
        {
            // Already inside the requested scene context boundary: apply panel adjustments locally
            ConfigureSystemSettings(targetState);
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
        ConfigureSystemSettings(targetState);

        // 5. Hide the loading overlay cleanly
        loadingScreenPanel.SetActive(false);
    }

    /// <summary>
    /// Internal helper method to route data settings, activate layouts, adjust cursors, 
    /// and safely alter Unity's internal engine clock speeds.
    /// </summary>
    private void ConfigureSystemSettings(GameState state)
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
                Time.timeScale = 1f;
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

    public void ResumeGame()
    {
        GameEvents.GameStateChanged(GameState.Playing);
    }

    public void QuitGame()
    {
        GameEvents.GameStateChanged(GameState.Menu);
    }
}
