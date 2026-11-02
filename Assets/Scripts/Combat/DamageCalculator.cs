// Ren räknelogik utan MonoBehaviour, så att den går att testa direkt.
using UnityEngine;

// Ren räknelogik utan MonoBehaviour, så att den går att testa direkt.
public static class DamageCalculator
{
    public static int Calculate(int baseDamage, int bonusDamage, int strength,
        bool isCrit = false, float critMultiplier = 1f)
    {
        int damage = baseDamage + bonusDamage + strength;
        return isCrit ? Mathf.RoundToInt(damage * critMultiplier) : damage;
    }

    public static float Cooldown(float attacksPerSecond, float speedMultiplier)
        => 1f / (attacksPerSecond * speedMultiplier);
}