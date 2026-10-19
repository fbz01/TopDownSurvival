using TMPro;
using UnityEngine;

public class StateUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI stateText;

    private void OnEnable() => GameEvents.OnGameStateChanged += UpdateText;
    private void OnDisable() => GameEvents.OnGameStateChanged -= UpdateText;

    private void Start() => UpdateText(GameState.Playing);

    private void UpdateText(GameState state)
    {
        switch (state)
        {
            case GameState.Menu:
                stateText.text = "Menu";
                break;
            case GameState.Playing:
                stateText.text = "";
                break;
            case GameState.Paused:
                stateText.text = "Game paused!";
                break;
            case GameState.ChoosingUpgrade:
                stateText.text = "Choose upgrade!";
                break;
            case GameState.GameOver:
                stateText.text = "You died! Game over!";
                break;
            default:
                stateText.text = "Default Game State";
                break;
        }
    }
}
