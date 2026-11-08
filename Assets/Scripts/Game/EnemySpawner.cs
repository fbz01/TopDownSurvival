using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private Vector2 arenaSize = new Vector2(15f, 8f); // the area inside the walls 
    [SerializeField] private float minDistanceToPlayer = 3f;


    private Transform player;


    private void Start()
    {
        GameObject playerObject = GameObject.FindWithTag("Player");
        if (playerObject != null)
            player = playerObject.transform;
    }



    public Enemy Spawn(Enemy prefab)
    {
        Vector2 position = RandomEdgePoint();
        // Up to 10 tries to find a point far enough from the player.
        for (int i = 0; i < 10 && IsTooCloseToPlayer(position); i++)
            position = RandomEdgePoint();

        return Instantiate(prefab, position, Quaternion.identity);
    }

    private Vector2 RandomEdgePoint()
    {
        Vector2 center = transform.position;
        float hx = arenaSize.x / 2f;
        float hy = arenaSize.y / 2f;

        switch (Random.Range(0, 4))
        {
            case 0: return center + new Vector2(Random.Range(-hx, hx), hy);
            case 1: return center + new Vector2(Random.Range(-hx, hx), -hy); 
            case 2: return center + new Vector2(-hx, Random.Range(-hy, hy)); 
            default: return center + new Vector2(hx, Random.Range(-hy, hy)); 
        }
    }

    private bool IsTooCloseToPlayer(Vector2 position)
    {
        return player != null && Vector2.Distance(position, player.position) < minDistanceToPlayer;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireCube(transform.position, arenaSize);
    }
}