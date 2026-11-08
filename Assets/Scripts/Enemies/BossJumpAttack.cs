using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Enemy), typeof(Rigidbody2D), typeof(Animator))]
public class BossJumpAttack : MonoBehaviour
{
    [Header("Tider (matchar Boss_Jump-klippet)")]
    [SerializeField] private float cooldown = 5f;
    [SerializeField] private float windup = 0.4f;
    [SerializeField] private float airTime = 0.6f;
    [SerializeField] private float recover = 0.4f;

    [Header("Landning")]
    [SerializeField] private float maxJumpDistance = 6f;
    [SerializeField] private float slamRadius = 2f;
    [SerializeField] private int slamDamage = 25;
    [SerializeField] private float slamKnockback = 12f;
    [SerializeField] private GameObject slamEffectPrefab;
    [SerializeField] private float slamEffectLifetime = 0.5f;

    private Enemy enemy;
    private Rigidbody2D rb;
    private Animator animator;
    private Collider2D col;
    private Transform player;

    private void Awake()
    {
        enemy = GetComponent<Enemy>();
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        col = GetComponent<Collider2D>();
    }

    private void Start()
    {
        GameObject p = GameObject.FindWithTag("Player");
        if (p != null) player = p.transform;
        StartCoroutine(AttackLoop());
    }

    private IEnumerator AttackLoop()
    {
        while (!enemy.IsDead)
        {
            yield return new WaitForSeconds(cooldown);
            if (enemy.IsDead || player == null) yield break;
            yield return JumpAttack(); // wait until the whole jump is done 
        }
    }

    private IEnumerator JumpAttack()
    {
        enemy.IsBusy = true; // pauses Enemy's chase and contact damage during the jump
        rb.linearVelocity = Vector2.zero;
        animator.SetTrigger("Jump");

        yield return new WaitForSeconds(windup);
        if (enemy.IsDead) yield break;

        Vector2 start = rb.position;
        Vector2 target = Vector2.MoveTowards(start, player.position, maxJumpDistance);
        col.enabled = false; // flies over the player instead of pushing them

        for (float t = 0f; t < airTime; t += Time.fixedDeltaTime)
        {
            if (enemy.IsDead) yield break;
            rb.MovePosition(Vector2.Lerp(start, target, t / airTime));
            yield return new WaitForFixedUpdate();
        }

        rb.MovePosition(target);
        col.enabled = true;
        Slam();

        yield return new WaitForSeconds(recover);
        enemy.IsBusy = false;
    }

    // Damages and knocks back the player if within slamRadius.
    private void Slam()
    {
        if (slamEffectPrefab != null)
            Destroy(Instantiate(slamEffectPrefab, transform.position, Quaternion.identity), slamEffectLifetime);

        foreach (Collider2D hit in Physics2D.OverlapCircleAll(transform.position, slamRadius))
        {
            if (!hit.CompareTag("Player")) continue;

            if (hit.TryGetComponent(out IDamageable target))
                target.TakeDamage(slamDamage);

            if (hit.TryGetComponent(out IKnockable knockable))
            {
                Vector2 away = ((Vector2)hit.transform.position - (Vector2)transform.position).normalized;
                knockable.ApplyKnockback(away, slamKnockback);
            }
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = new Color(0.6f, 0.2f, 1f);
        Gizmos.DrawWireSphere(transform.position, slamRadius);
    }
}