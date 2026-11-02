using TMPro;
using UnityEngine;

public class ControlsUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI controlsText;

    private void OnEnable() => GameEvents.OnGameStateChanged += UpdateVisibility;
    private void OnDisable() => GameEvents.OnGameStateChanged -= UpdateVisibility;

    private void Start()
    {
        controlsText.text =
            "<b>WASD</b>  Move\n" +
            "<b>Space</b>  Attack\n" +
            "<b>E</b>  Switch weapon\n" +
            "<b>Q</b>  Health potion\n" +
            "<b>P</b>  Pause";

        UpdateVisibility(GameState.Playing);
    }

    private void UpdateVisibility(GameState state)
        => controlsText.enabled = state == GameState.Playing;
}