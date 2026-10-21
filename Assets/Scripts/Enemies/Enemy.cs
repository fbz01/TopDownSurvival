using UnityEngine;

[RequireComponent(typeof(Rigidbody2D), typeof(Health), typeof(Animator))]
public class Enemy : MonoBehaviour, IKnockable
{
    [SerializeField] private float moveSpeed = 2f;
    [SerializeField] private int contactDamage = 10;
    [SerializeField] private float damageCooldown = 1f;
    [SerializeField] private int scoreValue = 10;
    [SerializeField] private float deathDuration = 0.5f;
    [SerializeField] private float hurtDuration = 0.2f;
    [SerializeField] private float contactKnockback = 8f;
    [SerializeField] private float recoilForce = 3f;
    [SerializeField] private float knockbackMultiplier = 1f;

    public int ScoreValue => scoreValue;
    public bool IsDead { get; private set; }
    public bool IsBusy { get; set; }

    private Rigidbody2D rb;
    private Health health;
    private Animator animator;
    private SpriteRenderer spriteRenderer;
    private Transform player;
    private float nextDamageTime;
    private float stunnedUntil;
    private bool IsStunned => Time.time < stunnedUntil;

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
    private void OnEnable() => health.OnDied += Die;
    private void OnDisable() => health.OnDied -= Die;
    private void FixedUpdate()
    {
        if (IsDead || player == null)
        {
            rb.linearVelocity = Vector2.zero;
            return;
        }

        if (IsBusy) return;

        if (IsStunned)
        {
            rb.linearVelocity *= 0.85f;
            return;
        }

        // jaga spelaren
        Vector2 direction = ((Vector2)player.position - rb.position).normalized;
        rb.linearVelocity = direction * moveSpeed;
    }

    private void Update()
    {
        if (IsDead || IsStunned || IsBusy) return;

        Vector2 velocity = rb.linearVelocity;
        animator.SetBool("IsMoving", velocity.sqrMagnitude > 0.01f);

        if (Mathf.Abs(velocity.x) > 0.01f)
            spriteRenderer.flipX = velocity.x < 0;
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (IsDead || IsStunned || IsBusy || Time.time < nextDamageTime) return;
        if (!collision.gameObject.CompareTag("Player")) return;

        if (collision.gameObject.TryGetComponent(out IDamageable target))
        {
            target.TakeDamage(contactDamage);
            nextDamageTime = Time.time + damageCooldown;

            Vector2 pushDirection = ((Vector2)collision.transform.position - rb.position).normalized;

            if (collision.gameObject.TryGetComponent(out IKnockable knockable))
                knockable.ApplyKnockback(pushDirection, contactKnockback);

            rb.linearVelocity = -pushDirection * recoilForce;
            stunnedUntil = Time.time + 0.3f;
        }
    }

    public void ApplyKnockback(Vector2 direction, float force)
    {
        if (IsDead || IsBusy) return;
        rb.linearVelocity = direction * force * knockbackMultiplier;
        stunnedUntil = Time.time + hurtDuration;
        animator.SetTrigger("Hurt");
    }

    private void Die()
    {
        IsDead = true;
        rb.linearVelocity = Vector2.zero;
        GetComponent<Collider2D>().enabled = false; // inga fler träffar eller knuffar

        animator.SetTrigger("Die");
        GameEvents.EnemyKilled(this);

        Destroy(gameObject, deathDuration); // ta bort när animationen har spelats
    }
}
