using UnityEngine;

// Basklassen i Strategy pattern: beskriver VAD en attack kan göra,
// men inte HUR. Det bestämmer varje konkret strategi.
public abstract class AttackStrategy : ScriptableObject
{
    // Utför attacken. Måste skrivas av varje strategi.
    public abstract void Execute(AttackContext context);

    // Var attacken "träffar", används för effekter och gizmos.
    public virtual Vector2 GetHitCenter(Vector2 origin, Vector2 direction) => origin;

    // Ritar strategins träffyta i Scene-vyn. Frivillig att skriva över.
    public virtual void DrawGizmos(Vector2 origin, Vector2 direction) { }
}