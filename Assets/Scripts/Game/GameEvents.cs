using System;

// Global event bus (Observer pattern). Systems communicate through these
// events instead of holding references to each other.
public static class GameEvents
{
    // Combat events
    public static event Action<Enemy> OnEnemyKilled;
    public static event Action OnPlayerDied;

    // Game flow events
    public static event Action<GameState> OnGameStateChanged;
    public static event Action<int> OnWaveStarted;
    public static event Action<int> OnWaveCompleted;
    public static event Action<int> OnScoreChanged;
    public static event Action<int> OnCoinCollected;
    
    // Raise methods
    public static void CoinCollected(int value) => OnCoinCollected?.Invoke(value);
    public static void EnemyKilled(Enemy enemy) => OnEnemyKilled?.Invoke(enemy);
    public static void PlayerDied() => OnPlayerDied?.Invoke();
    public static void GameStateChanged(GameState s) => OnGameStateChanged?.Invoke(s);
    public static void WaveStarted(int wave) => OnWaveStarted?.Invoke(wave);
    public static void WaveCompleted(int wave) => OnWaveCompleted?.Invoke(wave);
    public static void ScoreChanged(int score) => OnScoreChanged?.Invoke(score);
}
