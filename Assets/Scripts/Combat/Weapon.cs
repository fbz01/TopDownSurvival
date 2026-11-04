using System;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(PlayerController), typeof(PlayerStats))]
public class Weapon : MonoBehaviour
{
    [SerializeField] private WeaponData currentWeapon;

    private PlayerController controller;
    private PlayerStats stats;
    private float nextAttackTime;

    public WeaponData CurrentWeapon => currentWeapon;
    public int Damage =>
        DamageCalculator.Calculate(currentWeapon.BaseDamage, stats.BonusDamage, stats.Strength);

    public float AttackCooldown =>
        DamageCalculator.Cooldown(currentWeapon.AttacksPerSecond, stats.AttackSpeedMultiplier);
    public event Action<Vector2> OnAttack;
    public event Action<WeaponData> OnWeaponChanged;

    private AttackStrategy Strategy => currentWeapon != null ? currentWeapon.AttackStrategy : null;

    private void Awake()
    {
        controller = GetComponent<PlayerController>();
        stats = GetComponent<PlayerStats>();
    }

    private void Update()
    {
        if (Time.timeScale == 0f) return;
        if (currentWeapon == null) return;

        Keyboard keyboard = Keyboard.current;
        if (keyboard == null) return;

        if (keyboard.spaceKey.isPressed && Time.time >= nextAttackTime)
            Attack();
    }

    public void Equip(WeaponData newWeapon)
    {
        if (newWeapon == null) return;
        currentWeapon = newWeapon;
        OnWeaponChanged?.Invoke(newWeapon);
    }

    private int CalculateDamage(out bool isCrit)
    {
        isCrit = stats.RollCrit();
        return DamageCalculator.Calculate(currentWeapon.BaseDamage, stats.BonusDamage,
        stats.Strength, isCrit, stats.CritMultiplier);
    }

    private void Attack()
    {
        nextAttackTime = Time.time + AttackCooldown;

        if (Strategy == null)
        {
            Debug.LogWarning($"{currentWeapon.name} saknar AttackStrategy.", currentWeapon);
            return;
        }

        int damage = CalculateDamage(out bool isCrit);
        Vector2 direction = controller.FacingDirection;

        AttackContext context = new AttackContext(this, direction, damage, isCrit,
                                                  currentWeapon.KnockbackForce);
        Strategy.Execute(context);

        OnAttack?.Invoke(direction);
    }

    public Vector2 GetHitCenter()
    {
        if (Strategy == null) return transform.position;
        return Strategy.GetHitCenter(transform.position, controller.FacingDirection);
    }

    private void OnDrawGizmosSelected()
    {
        if (Strategy == null) return;
        if (controller == null) controller = GetComponent<PlayerController>();
        Strategy.DrawGizmos(transform.position, controller.FacingDirection);
    }
}