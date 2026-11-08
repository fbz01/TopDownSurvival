using System;
using UnityEngine;

public class Health : MonoBehaviour, IDamageable
{

    [SerializeField] private int maxHealth = 100;
    public int Current { get; private set; }
    public int Max => maxHealth;

    public event Action<int, int> OnHealthChanged; // current, max
    public event Action<int, bool> OnDamaged; // amount, isCrit

    public event Action OnDied;
    private void Awake()
    {
        ResetHealth();
    }

    // Public so the unit tests can call it, since Awake doesn't run in Edit Mode tests.
    public void ResetHealth()
    {
        Current = maxHealth;
        OnHealthChanged?.Invoke(Current, maxHealth);
    }

    public void TakeDamage(int amount, bool isCrit = false)
    {
        // Ignore dead objects and negative amounts (bugs found by HealthTests).
        if (Current <= 0 || amount <= 0) return;

        Current = Mathf.Max(Current - amount, 0);
        OnDamaged?.Invoke(amount, isCrit);
        OnHealthChanged?.Invoke(Current, maxHealth);

        if (Current == 0)
            OnDied?.Invoke();
    }

    public void Heal(int amount)
    {
        // Ignore dead objects and negative amounts (bugs found by HealthTests).
        if (Current <= 0 || amount <= 0) return;

        Current = Mathf.Min(Current + amount, maxHealth);
        OnHealthChanged?.Invoke(Current, maxHealth);
    }
}
