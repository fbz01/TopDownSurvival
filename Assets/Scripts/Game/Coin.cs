using UnityEngine;

public class Coin : MonoBehaviour
{
    [SerializeField] private float magnetRange = 1.5f;
    [SerializeField] private float magnetSpeed = 8f;
    [SerializeField] private float lifetime = 15f;
    [SerializeField] private float blinkTime = 3f;
    [SerializeField] private AudioClip pickupSound;
    [SerializeField, Range(0f, 1f)] private float pickupVolume = 0.7f;

    private int value;
    private Transform player;
    private SpriteRenderer spriteRenderer;
    private float spawnTime;

    public void Setup(int value) => this.value = value;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        spawnTime = Time.time;
    }

    private void Start()
    {
        GameObject p = GameObject.FindWithTag("Player");
        if (p != null) player = p.transform;
    }

    private void Update()
    {
        float age = Time.time - spawnTime;

        if (age >= lifetime)
        {
            Destroy(gameObject);
            return;
        }

        if (age > lifetime - blinkTime)
            spriteRenderer.enabled = Mathf.FloorToInt(age * 10f) % 2 == 0;

        if (player != null && Vector2.Distance(transform.position, player.position) < magnetRange)
            transform.position = Vector2.MoveTowards(transform.position, player.position,
                                                     magnetSpeed * Time.deltaTime);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;

        if (pickupSound != null)
            AudioSource.PlayClipAtPoint(pickupSound, Camera.main.transform.position, pickupVolume);

        GameEvents.CoinCollected(value);
        Destroy(gameObject);
    }
}