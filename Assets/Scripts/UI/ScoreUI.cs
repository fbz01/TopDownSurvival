using TMPro;
using UnityEngine;
using UnityEngine.PlayerLoop;

public class ScoreUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI scoreText;

    private void OnEnable() => GameEvents.OnScoreChanged += UpdateText;
    private void OnDisable() => GameEvents.OnScoreChanged -= UpdateText;

    private void Start() => UpdateText(0);

    private void UpdateText(int score) => scoreText.text = $"Score: {score}";
}
