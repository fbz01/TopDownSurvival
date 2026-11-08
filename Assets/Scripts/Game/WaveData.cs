using UnityEngine;

// Data asset for one wave: which enemies, how many, and the spawn interval.
[System.Serializable]
public class WaveEntry
{
    public Enemy enemyPrefab;
    public int count = 5;
}

[CreateAssetMenu(fileName = "Wave_", menuName = "Game/Wave")]
public class WaveData : ScriptableObject
{
    [SerializeField] private WaveEntry[] enemies;
    [SerializeField] private float spawnInterval = 1.2f;

    public WaveEntry[] Enemies => enemies;
    public float SpawnInterval => spawnInterval;
}