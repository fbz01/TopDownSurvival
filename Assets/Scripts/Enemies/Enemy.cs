using UnityEngine;

[RequireComponent(typeof(Rigidbody2D), typeof (Health), typeof(Animator))]
public class Enemy : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 2f;
    [SerializeField] private int contactDamage = 10;
    [SerializeField] private float damageCooldown = 1f;
    [SerializeField] private int scoreValue = 10;
    [SerializeField] private float deathDuration = 0.5f;

    public int ScoreValue => scoreValue;
    public bool isDead {get; private set;}

    private Rigidbody2D rb;
    private Health health;
    private Animator animator;
    private SpriteRenderer spriteRenderer;
    private Transform player;
    private float nextDamageTime;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        health = GetComponent<Health>();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void Start()
    {
        GameObject playerObject = GameObject.FindWithTag("Player");
        if (playerObject != null)
            player = playerObject.transform;
    }

    private void FixedUpdate()
    {
        if (isDead || player == null)
        {
            rb.linearVelocity = Vector2.zero;
            return;
        }

        // jaga spelaren
        Vector2 direction = ((Vector2)player.position - rb.position).normalized;
        rb.linearVelocity = direction * moveSpeed;
    }

    private void Update()
    {
        if (isDead) return;

        Vector2 velocity = rb.linearVelocity;
        animator.SetBool("IsMoving", velocity.sqrMagnitude > 0.01f);

        if (Mathf.Abs(velocity.x) > 0.01f)
            spriteRenderer.flipX = velocity.x < 0;
    }
}
