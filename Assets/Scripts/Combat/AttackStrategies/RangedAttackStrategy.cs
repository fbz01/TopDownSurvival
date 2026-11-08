using System.Collections;
using UnityEngine;

[CreateAssetMenu(fileName = "NewRangedAttack", menuName = "Game/Attack Strategies/Ranged")]

// Fires one projectile in the facing direction.
public class RangedAttackStrategy : AttackStrategy
{
    [SerializeField] private Projectile projectilePrefab;
    [SerializeField] private float projectileSpeed = 12f;
    [SerializeField] private float releaseDelay = 0.17f;
    [SerializeField] private Vector2 spawnOffset = new Vector2(0f, 0.3f);
    [SerializeField] private float forwardOffset = 0.3f;

    public override void Execute(AttackContext context)
    {
        if (projectilePrefab == null)
        {
            Debug.LogWarning($"{name} saknar projectilePrefab.", this);
            return;
        }

        // ScriptableObjects can't run coroutines, so the owner's is borrowed.
        context.Owner.StartCoroutine(FireAfterDelay(context));
    }

    private IEnumerator FireAfterDelay(AttackContext context)
    {
        yield return new WaitForSeconds(releaseDelay);

        Vector2 spawnPos = GetHitCenter(context.Origin, context.Direction);
        Projectile arrow = Instantiate(projectilePrefab, spawnPos, Quaternion.identity);
        arrow.Launch(context.Direction, projectileSpeed, context.Damage, context.IsCrit,
                     context.Knockback, context.Owner.gameObject);
    }

    public override Vector2 GetHitCenter(Vector2 origin, Vector2 direction)
        => origin + spawnOffset + direction * forwardOffset;

    public override void DrawGizmos(Vector2 origin, Vector2 direction)
    {
        Vector2 spawnPos = GetHitCenter(origin, direction);
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(spawnPos, 0.1f);
        Gizmos.DrawRay(spawnPos, direction);
    }
}