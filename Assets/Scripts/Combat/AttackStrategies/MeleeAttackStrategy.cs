using UnityEngine;

[CreateAssetMenu(fileName = "NewMeleeAttack", menuName = "Game/Attack Strategies/Melee")]
public class MeleeAttackStrategy : AttackStrategy
{
    [SerializeField] private float range = 0.8f;
    [SerializeField] private float radius = 0.6f;
    [SerializeField] private Vector2 bodyOffset = new Vector2(0f, 0.6f);

    public override void Execute(AttackContext context)
    {
        Vector2 center = GetHitCenter(context.Origin, context.Direction);
        Collider2D[] hits = Physics2D.OverlapCircleAll(center, radius);

        foreach (Collider2D hit in hits)
        {
            if (hit.gameObject == context.Owner.gameObject) continue;

            if (hit.TryGetComponent(out IDamageable target))
            {
                target.TakeDamage(context.Damage, context.IsCrit);

                if (hit.TryGetComponent(out IKnockable knockable))
                {
                    Vector2 away = ((Vector2)hit.transform.position - context.Origin).normalized;
                    knockable.ApplyKnockback(away, context.Knockback);
                }
            }
        }
    }

    public override Vector2 GetHitCenter(Vector2 origin, Vector2 direction)
        => origin + bodyOffset + direction * range;

    public override void DrawGizmos(Vector2 origin, Vector2 direction)
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(GetHitCenter(origin, direction), radius);
    }
}