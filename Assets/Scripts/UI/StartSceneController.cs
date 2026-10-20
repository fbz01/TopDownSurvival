using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartSceneController : MonoBehaviour
{
    [SerializeField] private TMP_InputField nameInput;
    [SerializeField] private string gameSceneName = "Test_A";

    void Start()
    {
        
    }

    void Update()
    {
        
    }

    public void StartNewGame()
    {
        string playerName = nameInput.text.Trim();

        if (string.IsNullOrWhiteSpace(playerName))
        {
            Debug.LogWarning("Enter a player name!");
            return;
        }

        SessionData.PlayerName = playerName;

        SceneManager.LoadScene(gameSceneName);
    }
}
