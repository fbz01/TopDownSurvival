using System;

public static class GameEvents
{
    // strid events
    public static event Action<Enemy> OnEnemyKilled;
    public static event Action OnPlayerDied;

    // spelflöde
    public static event Action<GameState> OnGameStateChanged;
    public static event Action<int> OnWaveStarted;
    public static event Action<int> OnWaveCompleted;
    public static event Action<int> OnScoreChanged;
    

    public static void EnemyKilled(Enemy enemy) => OnEnemyKilled?.Invoke(enemy);
    public static void PlayerDied() => OnPlayerDied?.Invoke();
    public static void GameStateChanged(GameState s)   => OnGameStateChanged?.Invoke(s);
    public static void WaveStarted(int wave)           => OnWaveStarted?.Invoke(wave);
    public static void WaveCompleted(int wave)         => OnWaveCompleted?.Invoke(wave);
    public static void ScoreChanged(int score)         => OnScoreChanged?.Invoke(score);
}
