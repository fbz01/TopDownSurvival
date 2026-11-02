using System.Collections;
using UnityEngine;

public class WaveManager : MonoBehaviour
{
    [SerializeField] private WaveData[] waves;
    [SerializeField] private EnemySpawner spawner;
    [SerializeField] private float firstWaveDelay = 2f;
    [SerializeField] private float pauseBetweenWaves = 3f;

    private int aliveCount;

    private void OnEnable()
    {
        GameEvents.OnEnemyKilled += HandleEnemyKilled;
        GameEvents.OnPlayerDied += HandlePlayerDied;
    }

    private void OnDisable()
    {
        GameEvents.OnEnemyKilled -= HandleEnemyKilled;
        GameEvents.OnPlayerDied -= HandlePlayerDied;
    }

    private void Start() => StartCoroutine(RunWaves());

    private IEnumerator RunWaves()
    {
        yield return new WaitForSeconds(firstWaveDelay);

        for (int i = 0; i < waves.Length; i++)
        {
            int waveNumber = i + 1;
            GameEvents.WaveStarted(waveNumber);

            yield return SpawnWave(waves[i]);

            yield return new WaitUntil(() => aliveCount <= 0);   // alla döda
            GameEvents.WaveCompleted(waveNumber);

            if (i < waves.Length - 1)
            {
                GameEvents.GameStateChanged(GameState.ChoosingUpgrade);
                yield return new WaitForSeconds(pauseBetweenWaves);
            }
        }

        Debug.Log("Alla waves klara!");   // här kan en vinstskärm visas senare
    }

    private IEnumerator SpawnWave(WaveData wave)
    {
        foreach (WaveEntry entry in wave.Enemies)
        {
            for (int n = 0; n < entry.count; n++)
            {
                spawner.Spawn(entry.enemyPrefab);
                aliveCount++;
                yield return new WaitForSeconds(wave.SpawnInterval);
            }
        }
    }

    private void HandleEnemyKilled(Enemy enemy) => aliveCount--;

    private void HandlePlayerDied() => StopAllCoroutines();   // inga nya fiender efter Game Over
}