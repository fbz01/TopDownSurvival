using UnityEngine;

public class GameManager : MonoBehaviour
{
    public int Score { get; private set; }
    public int Wave { get; private set; }

    private void OnEnable()
    {
        GameEvents.OnEnemyKilled += HandleEnemyKilled;

        GameEvents.OnWaveStarted += HandleWaveStarted;
    }

    private void OnDisable()
    {
        GameEvents.OnEnemyKilled -= HandleEnemyKilled;

        GameEvents.OnWaveStarted -= HandleWaveStarted;
    }

    private void HandleEnemyKilled(Enemy enemy)
    {
        Score += enemy.ScoreValue;
        GameEvents.ScoreChanged(Score);
    }

    private void HandleWaveStarted(int wave)
    {
        Wave += wave;
        GameEvents.WaveStarted(Wave);
    }
}
