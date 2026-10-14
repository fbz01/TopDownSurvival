using System;
using UnityEngine;

public class Health : MonoBehaviour, IDamageable
{

    [SerializeField] private int maxHealth = 100;
    public int Current {get; private set;}

    [SerializeField] private PlayerStats playerStats;

    public event Action<int, int> OnHealthChanged; // event att lyssna på <nuvarande health, max health>

    // metod för testerna
    private void Awake()
    {
        ResetHealth();

        if (playerStats == null)
        {
            playerStats = GetComponent<PlayerStats>();
        }
    } 
    
    // metod för testerna
    public void ResetHealth()
    {
        Current = maxHealth;
        OnHealthChanged?.Invoke(Current, maxHealth);
    }

    public void TakeDamage(int amount)
    {
        int totalDamage = amount * playerStats.strength * playerStats.luck;
        Current = Mathf.Max(Current - totalDamage, 0);
        OnHealthChanged?.Invoke(Current, maxHealth);
    }

    public void Heal (int amount)
    {
        Current = Mathf.Min(Current+ amount, maxHealth);
        OnHealthChanged?.Invoke(Current, maxHealth);
    }
}
