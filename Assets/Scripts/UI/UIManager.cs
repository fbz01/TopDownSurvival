using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    // Singleton instance for global access point
    public static UIManager Instance { get; private set; }

    [SerializeField] private GameObject loadingScreenPanel;
    [SerializeField] private Slider loadingProgressBar;

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

    // Clean Facade Interface method visible to menus/states
    public void LoadScene(string sceneName)
    {
        StartCoroutine(LoadSceneAsync(sceneName));
    }
    
    // Iterator Method handling the complex async system smoothly
    private IEnumerator LoadSceneAsync(string sceneName)
    {
        loadingScreenPanel.SetActive(true);
        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneName);

        // Keep updating the progress bar until the scene finishes loading
        while (!operation.isDone)
        {
            // Unity progress ranges from 0 to 0.9 while loading
            float progress = Mathf.Clamp01(operation.progress / 0.9f);
            loadingProgressBar.value = progress;

            yield return null; // Pauses execution and waits until the next frame
        }

        loadingScreenPanel.SetActive(false);
    }
}
