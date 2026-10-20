using System;
using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    [Header("Skada")]
    [SerializeField] private int strength = 5;
    [SerializeField] private int bonusDamage = 0;

    [Header("Crit")]
    [SerializeField, Range(0, 100)] private int critChance = 10;    // procent
    [SerializeField] private float critMultiplier = 2f;

    [Header("Attackhastighet")]
    [SerializeField] private float attackSpeedMultiplier = 1f;
    [SerializeField] private float maxAttackSpeedMultiplier = 3f;

    // Skickas när något värde ändras, så att UI kan uppdateras
    public event Action OnStatsChanged;

    public int Strength => strength;
    public int BonusDamage => bonusDamage;
    public int CritChance => critChance;
    public float CritMultiplier => critMultiplier;
    public float AttackSpeedMultiplier => attackSpeedMultiplier;

    // Gubben avgör om slaget blir en crit. Ett vapen kan ge extra chans.
    public bool RollCrit(int bonusChance = 0)
    {
        return UnityEngine.Random.Range(0, 100) < critChance + bonusChance;
    }

    // Uppgraderingar
    public void IncreaseStrength(int amount)
    {
        if (amount <= 0) return;
        strength += amount;
        OnStatsChanged?.Invoke();
    }

    public void IncreaseDamage(int amount)
    {
        if (amount <= 0) return;
        bonusDamage += amount;
        OnStatsChanged?.Invoke();
    }

    public void IncreaseCritChance(int amount)
    {
        if (amount <= 0) return;
        critChance = Mathf.Min(critChance + amount, 100);   // aldrig över 100 %
        OnStatsChanged?.Invoke();
    }

    public void IncreaseCritMultiplier(float amount)
    {
        if (amount <= 0) return;
        critMultiplier += amount;
        OnStatsChanged?.Invoke();
    }

    public void IncreaseAttackSpeed(float amount)
    {
        if (amount <= 0) return;
        attackSpeedMultiplier = Mathf.Min(attackSpeedMultiplier + amount, maxAttackSpeedMultiplier);
        OnStatsChanged?.Invoke();
    }
}