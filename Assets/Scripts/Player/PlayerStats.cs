using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    [SerializeField] private int bonusDamage = 0;
    [SerializeField] private float attackSpeedMultiplier = 1f;
    // Declare player stats
    public int strength { get; private set; }
    public int luck { get; private set; }

    public int BonusDamage => bonusDamage;
    public float AttackSpeedMultiplier => attackSpeedMultiplier;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // Initialize player stats
        strength = 5;
        luck = 10;
    }

    // Update is called once per frame
    void Update()
    {

    }

    void ImproveStrength()
    {
        strength += 5;
    }

    void ImproveLuck()
    {
        luck += 1;
    }

    public void IncreaseDamage(int amount)
    {
        if (amount <= 0) return;
        bonusDamage += amount;
    }

    public void IncreaseAttackSpeed(float amount)
{
    if (amount <= 0) return;
    attackSpeedMultiplier = Mathf.Min(attackSpeedMultiplier + amount);
}
}
