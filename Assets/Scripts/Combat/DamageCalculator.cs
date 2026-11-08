using UnityEngine;

// Pure calculation without MonoBehaviour, so it can be unit tested directly.
public static class DamageCalculator
{
    public static int Calculate(int baseDamage, int bonusDamage, int strength,
        bool isCrit = false, float critMultiplier = 1f)
    {
        int damage = baseDamage + bonusDamage + strength;
        return isCrit ? Mathf.RoundToInt(damage * critMultiplier) : damage;
    }
    // Seconds between attacks.
    public static float Cooldown(float attacksPerSecond, float speedMultiplier)
        => 1f / (attacksPerSecond * speedMultiplier);
}