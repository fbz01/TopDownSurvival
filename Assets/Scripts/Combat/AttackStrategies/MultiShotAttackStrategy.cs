using System.Collections;
using UnityEngine;

[CreateAssetMenu(fileName = "NewMultiShotAttack", menuName = "Game/Attack Strategies/Multi Shot")]

// Like RangedAttackStrategy, but fires several projectiles.
public class MultiShotAttackStrategy : AttackStrategy
{
    [SerializeField] private Projectile projectilePrefab;
    [SerializeField] private float projectileSpeed = 12f;
    [SerializeField] private float releaseDelay = 0.17f;
    [SerializeField] private Vector2 spawnOffset = new Vector2(0f, 0.3f);
    [SerializeField] private float forwardOffset = 0.3f;

    [Header("Multishot")]
    [SerializeField, Min(1)] private int projectileCount = 3;
    [SerializeField] private float spreadAngle = 15f;

    public override void Execute(AttackContext context)
    {
        if (projectilePrefab == null)
        {
            Debug.LogWarning($"{name} saknar projectilePrefab.", this);
            return;
        }
        context.Owner.StartCoroutine(FireAfterDelay(context));
    }

    private IEnumerator FireAfterDelay(AttackContext context)
    {
        yield return new WaitForSeconds(releaseDelay);

        Vector2 spawnPos = GetHitCenter(context.Origin, context.Direction);

        for (int i = 0; i < projectileCount; i++)
        {
            Vector2 direction = GetSpreadDirection(context.Direction, i);
            Projectile arrow = Instantiate(projectilePrefab, spawnPos, Quaternion.identity);
            arrow.Launch(direction, projectileSpeed, context.Damage, context.IsCrit,
                         context.Knockback, context.Owner.gameObject);
        }
    }

    // Spreads the arrows evenly around the aim direction, e.g. -15°, 0°, +15° for three.
    private Vector2 GetSpreadDirection(Vector2 direction, int index)
    {
        float angle = (index - (projectileCount - 1) / 2f) * spreadAngle;
        return Quaternion.Euler(0f, 0f, angle) * direction;
    }

    public override Vector2 GetHitCenter(Vector2 origin, Vector2 direction)
        => origin + spawnOffset + direction * forwardOffset;

    public override void DrawGizmos(Vector2 origin, Vector2 direction)
    {
        Vector2 spawnPos = GetHitCenter(origin, direction);
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(spawnPos, 0.1f);

        for (int i = 0; i < projectileCount; i++)
            Gizmos.DrawRay(spawnPos, GetSpreadDirection(direction, i));
    }
}