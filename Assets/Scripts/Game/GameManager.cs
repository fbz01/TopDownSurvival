using UnityEngine;

public class GameManager : MonoBehaviour
{
    public int Score { get; private set; }

    private void OnEnable() => GameEvents.OnEnemyKilled += HandleEnemyKilled;
    private void OnDisable() => GameEvents.OnEnemyKilled -= HandleEnemyKilled;

    private void HandleEnemyKilled (Enemy enemy)
    {
        Score += enemy.ScoreValue;
        GameEvents.ScoreChanged(Score);
    }
}
