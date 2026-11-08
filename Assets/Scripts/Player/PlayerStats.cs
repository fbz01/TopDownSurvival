using System;
using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    [Header("Skada")]
    [SerializeField] private int strength = 5;
    [SerializeField] private int bonusDamage = 0;

    [Header("Crit")]
    [SerializeField, Range(0, 100)] private int critChance = 10;    // percent
    [SerializeField] private float critMultiplier = 2f;

    [Header("Attackhastighet")]
    [SerializeField] private float attackSpeedMultiplier = 1f;
    [SerializeField] private float maxAttackSpeedMultiplier = 3f;

    // Raised when a value changes, so the UI can update.
    public event Action OnStatsChanged;

    public int Strength => strength;
    public int BonusDamage => bonusDamage;
    public int CritChance => critChance;
    public float CritMultiplier => critMultiplier;
    public float AttackSpeedMultiplier => attackSpeedMultiplier;

    private void Start()
    {
        LoadSavedStats();

        if (PlayerStatsManager.Instance != null)
        // Reload when the upgrade menu saves new stats.
            PlayerStatsManager.Instance.OnStatsSaved += LoadSavedStats;
    }

        private void OnDestroy()
    {
        if (PlayerStatsManager.Instance != null)
            PlayerStatsManager.Instance.OnStatsSaved -= LoadSavedStats;
    }

    // Rolls whether a hit is a crit. A weapon can add bonus chance.
    public bool RollCrit(int bonusChance = 0)
    {
        return UnityEngine.Random.Range(0, 100) < critChance + bonusChance;
    }

    // Upgrades
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
        critChance = Mathf.Min(critChance + amount, 100);   // never above 100 %
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

    public void LoadSavedStats()
    {
        var savedStats = PlayerStatsManager.Instance;
        if (savedStats == null) return;

        strength = savedStats.Strength;
        critChance = savedStats.CritChance;
        attackSpeedMultiplier = Mathf.Min(savedStats.AttackSpeedMultiplier, maxAttackSpeedMultiplier);

        OnStatsChanged?.Invoke();
    }
}