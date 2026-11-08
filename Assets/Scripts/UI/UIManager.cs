using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

// Facade between the UI and the rest of the game: reacts to OnGameStateChanged
// and loads scenes, shows panels and sets the time scale.
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
        GameEvents.OnGameStateChanged += HandleGameStateChanged;
    }

    private void OnDisable()
    {
        // Unsubscribe so the static event doesn't keep a reference to a destroyed UIManager.
        GameEvents.OnGameStateChanged -= HandleGameStateChanged;
    }

    private void Start()
    {
        Debug.Log("UIManager Start körs!");

        Debug.Log("MainMenuPanel referens: " +
            (mainMenuPanel != null ? mainMenuPanel.name : "NULL"));

        ConfigureSystemSettings(GameState.Menu);
    }

    // Menu and Playing need a scene change; the other states only change panels.
    private void HandleGameStateChanged(GameState newState)
    {
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

    //Loads the scene only if we're not already in it
    private void EvaluateSceneTransition(string targetSceneName, GameState targetState)
    {
        if (SceneManager.GetActiveScene().name != targetSceneName)
        {
            StartCoroutine(LoadSceneAndTransitionStateAsync(targetSceneName, targetState));
        }
        else
        {
            ConfigureSystemSettings(targetState);
        }
    }

    // Loads the scene in the background while showing a loading bar.
    private IEnumerator LoadSceneAndTransitionStateAsync(string sceneName, GameState targetState)
    {
        loadingScreenPanel.SetActive(true);
        loadingProgressBar.value = 0f;

        // Time.timeScale = 1f;  → "Unpause in case we come from the pause menu."
        Time.timeScale = 1f;

        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneName);

        while (!operation.isDone)
        {
            // Unity reports 0.9 when loading is done, so the progress is scaled to 0–1.
            float progress = Mathf.Clamp01(operation.progress / 0.9f);
            loadingProgressBar.value = progress;

            yield return null;
        }

        ConfigureSystemSettings(targetState);

        loadingScreenPanel.SetActive(false);
    }

    // Sets time scale, cursor and visible panels for each state.
    private void ConfigureSystemSettings(GameState state)
    {
        Debug.Log("GameState ändras till: " + state);

        switch (state)
        {
            case GameState.Menu:
                Time.timeScale = 1f;
                SetCursorStatus(true);
                ToggleUIPanels(mainMenu: true, pause: false, upgrade: false, over: false);
                break;

            case GameState.Playing:
                Time.timeScale = 1f;
                SetCursorStatus(true);
                ToggleUIPanels(mainMenu: false, pause: false, upgrade: false, over: false);
                break;

            case GameState.ChoosingUpgrade:
                Time.timeScale = 0f; // pauses the game while the menu is open
                SetCursorStatus(true);
                ToggleUIPanels(mainMenu: false, pause: false, upgrade: true, over: false);
                break;

            case GameState.Paused:
                Time.timeScale = 0f; // pauses the game while the menu is open
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
