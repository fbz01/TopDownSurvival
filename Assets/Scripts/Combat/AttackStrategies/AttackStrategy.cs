using UnityEngine;

// Strategy base class: defines what an attack can do; each subclass decides how.
public abstract class AttackStrategy : ScriptableObject
{
    // Performs the attack. Every strategy must implement it.
    public abstract void Execute(AttackContext context);

    public virtual Vector2 GetHitCenter(Vector2 origin, Vector2 direction) => origin;

    public virtual void DrawGizmos(Vector2 origin, Vector2 direction) { }
}