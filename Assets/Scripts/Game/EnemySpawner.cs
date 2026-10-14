using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private Enemy enemyPrefab;
    [SerializeField] private Vector2 arenaSize = new Vector2(15f, 8f); // området innanför väggarna
    [SerializeField] private float minDistanceToPlayer = 3f;

    [Header("Takt")]
    [SerializeField] private float startInterval = 3f;              // sekunder mellan slimes i början
    [SerializeField] private float minInterval = 0.5f;              // snabbaste takten
    [SerializeField] private float intervalDecreasePerSpawn = 0.05f;
    [SerializeField] private int maxEnemies = 20;

    private float currentInterval;
    private float nextSpawnTime;
    private int aliveCount;
    private Transform player;

    private void OnEnable()  => GameEvents.OnEnemyKilled += HandleEnemyKilled;
    private void OnDisable() => GameEvents.OnEnemyKilled -= HandleEnemyKilled;

    private void Start()
    {
        currentInterval = startInterval;
        nextSpawnTime = Time.time + 1f; // en sekunds andrum i början

        GameObject playerObject = GameObject.FindWithTag("Player");
        if (playerObject != null)
            player = playerObject.transform;
    }

    private void Update()
    {
        if (Time.time < nextSpawnTime || aliveCount >= maxEnemies) return;

        Spawn();
        nextSpawnTime = Time.time + currentInterval;
        currentInterval = Mathf.Max(minInterval, currentInterval - intervalDecreasePerSpawn);
    }

    private void Spawn()
    {
        Vector2 position = RandomEdgePoint();

        // Försök några gånger att hitta en plats som inte ligger precis vid spelaren
        for (int i = 0; i < 10 && IsTooCloseToPlayer(position); i++)
            position = RandomEdgePoint();

        Instantiate(enemyPrefab, position, Quaternion.identity);
        aliveCount++;
    }

    private Vector2 RandomEdgePoint()
    {
        Vector2 center = transform.position;
        float hx = arenaSize.x / 2f;
        float hy = arenaSize.y / 2f;

        switch (Random.Range(0, 4))
        {
            case 0:  return center + new Vector2(Random.Range(-hx, hx),  hy); // överkant
            case 1:  return center + new Vector2(Random.Range(-hx, hx), -hy); // nederkant
            case 2:  return center + new Vector2(-hx, Random.Range(-hy, hy)); // vänster
            default: return center + new Vector2( hx, Random.Range(-hy, hy)); // höger
        }
    }

    private bool IsTooCloseToPlayer(Vector2 position)
    {
        return player != null && Vector2.Distance(position, player.position) < minDistanceToPlayer;
    }

    private void HandleEnemyKilled(Enemy enemy)
    {
        aliveCount--;
    }

    // Visar spawnområdet som en gul rektangel i Scene-fönstret
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireCube(transform.position, arenaSize);
    }
}