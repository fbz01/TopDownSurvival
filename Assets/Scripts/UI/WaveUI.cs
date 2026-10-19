using TMPro;
using UnityEngine;

public class WaveUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI waveText;

    private void OnEnable() => GameEvents.OnWaveStarted += UpdateText;
    private void OnDisable() => GameEvents.OnWaveStarted -= UpdateText;

    private void Start() => UpdateText(1);

    private void UpdateText(int wave) => waveText.text = $"Wave: {wave}";
}
