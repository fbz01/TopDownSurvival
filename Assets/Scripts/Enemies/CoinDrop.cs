using UnityEngine;

[RequireComponent(typeof(Health), typeof(Enemy))]
public class CoinDrop : MonoBehaviour
{
    [SerializeField] private Coin coinPrefab;
    [SerializeField] private int coinCount = 1;
    [SerializeField] private float scatter = 0.5f; 

    private Health health;
    private Enemy enemy;

    private void Awake()
    {
        health = GetComponent<Health>();
        enemy = GetComponent<Enemy>();
    }

    private void OnEnable()  => health.OnDied += Drop;
    private void OnDisable() => health.OnDied -= Drop;

    private void Drop()
    {
        if (coinPrefab == null) return;

        int valuePerCoin = Mathf.Max(1, enemy.ScoreValue / coinCount);

        for (int i = 0; i < coinCount; i++)
        {
            Vector2 offset = Random.insideUnitCircle * scatter;
            Coin coin = Instantiate(coinPrefab, (Vector2)transform.position + offset, Quaternion.identity);
            coin.Setup(enemy.ScoreValue > 0 ? valuePerCoin : 0);
        }
    }
}