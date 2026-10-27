using UnityEngine;

public class GameManager : MonoBehaviour
{
    public int Score { get; private set; }
    public int Wave { get; private set; }
    public GameState State { get; private set; }

    private void OnEnable()
    {
        GameEvents.OnCoinCollected += HandleCoinCollected;

        GameEvents.OnWaveStarted += HandleWaveStarted;

        GameEvents.OnGameStateChanged += HandleGameStateChanged;
    }

    private void OnDisable()
    {
        GameEvents.OnCoinCollected -= HandleCoinCollected;

        GameEvents.OnWaveStarted -= HandleWaveStarted;

        GameEvents.OnGameStateChanged -= HandleGameStateChanged;
    }

    private void HandleCoinCollected(int value)
    {
        Score += value;
        GameEvents.ScoreChanged(Score);
    }

    private void HandleWaveStarted(int wave)
    {
        Wave = wave;
    }

    private void HandleGameStateChanged(GameState state)
    {
        State = state;
    }
}
