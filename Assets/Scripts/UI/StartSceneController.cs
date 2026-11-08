using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartSceneController : MonoBehaviour
{
    [SerializeField] private TMP_InputField nameInput;

    public void StartNewGame()
    {
        string playerName = nameInput.text.Trim();

        if (string.IsNullOrWhiteSpace(playerName))
        {
            Debug.LogWarning("Enter a player name!");
            return;
        }

        SessionData.PlayerName = playerName;

        GameEvents.GameStateChanged(GameState.Playing); // UIManager loads the game scene 
    }
}
