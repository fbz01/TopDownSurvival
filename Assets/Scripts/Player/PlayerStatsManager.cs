using UnityEngine;
using System;

public class PlayerStatsManager : MonoBehaviour
{
    public static PlayerStatsManager Instance { get; private set; }

    [Header("Startvärden - ska matcha PlayerStats")]
    [SerializeField] private int startingStrength = 5;
    [SerializeField] private int startingCritChance = 10;
    [SerializeField] private float startingAttackSpeed = 1f;

    public int Strength { get; private set; }
    public int CritChance { get; private set; }
    public float AttackSpeedMultiplier { get; private set; }
     public event Action OnStatsSaved;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        Strength = startingStrength;
        CritChance = startingCritChance;
        AttackSpeedMultiplier = startingAttackSpeed;
    }

    public void SetStats(int strength, int critChance, float attackSpeed)
    {
        Strength = Mathf.Max(0, strength);
        CritChance = Mathf.Clamp(critChance, 0, 100);
        AttackSpeedMultiplier = Mathf.Clamp(attackSpeed, 0f, 3f);

        OnStatsSaved?.Invoke();
    }
}

