using UnityEngine;

// Everything a strategy needs for one attack. Passed in on every call.
public readonly struct AttackContext
{
    public MonoBehaviour Owner { get; }
    public Vector2 Direction { get; }
    public int Damage { get; }
    public bool IsCrit { get; }
    public float Knockback { get; }

    public Vector2 Origin => Owner.transform.position;

    public AttackContext(MonoBehaviour owner, Vector2 direction,
                         int damage, bool isCrit, float knockback)
    {
        Owner = owner;
        Direction = direction;
        Damage = damage;
        IsCrit = isCrit;
        Knockback = knockback;
    }
}