using System;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(PlayerController), typeof(PlayerStats))]
public class Weapon : MonoBehaviour
{
    [SerializeField] private WeaponData currentWeapon;
    [SerializeField] private Vector2 bodyOffset = new Vector2(0f, 05f);

    private PlayerController controller;
    private PlayerStats stats;
    private float nextAttackTime;

    public WeaponData CurrentWeapon => currentWeapon;
    public int Damage => currentWeapon.BaseDamage + stats.BonusDamage + stats.Strength;
    public float AttackCooldown =>
        1f / (currentWeapon.AttacksPerSecond * stats.AttackSpeedMultiplier);

    public event Action<Vector2> OnAttack;
    public event Action<WeaponData> OnWeaponChanged;

    private void Awake()
    {
        controller = GetComponent<PlayerController>();
        stats = GetComponent<PlayerStats>();
    }

    private void Update()
    {
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
        int damage = currentWeapon.BaseDamage + stats.BonusDamage + stats.Strength;

        isCrit = stats.RollCrit();
        if (isCrit)
            damage = Mathf.RoundToInt(damage * stats.CritMultiplier);

        return damage;
    }

    private void Attack()
    {
        nextAttackTime = Time.time + AttackCooldown;

        int damage = CalculateDamage(out bool isCrit);

        Collider2D[] hits = Physics2D.OverlapCircleAll(GetHitCenter(), currentWeapon.Radius);
        foreach (Collider2D hit in hits)
        {
            if (hit.gameObject == gameObject) continue;

            if (hit.TryGetComponent(out IDamageable target))
            {
                target.TakeDamage(damage, isCrit);

                if (hit.TryGetComponent(out IKnockable knockbackable))
                {
                    Vector2 away = ((Vector2)hit.transform.position - (Vector2)transform.position).normalized;
                    knockbackable.ApplyKnockback(away, currentWeapon.KnockbackForce);
                }
            }
        }
        OnAttack?.Invoke(controller.FacingDirection);
    }

    public Vector2 GetHitCenter()
    {
        return (Vector2)transform.position + bodyOffset
             + controller.FacingDirection * currentWeapon.Range;
    }

    private void OnDrawGizmosSelected()
    {
        if (currentWeapon == null) return;
        if (controller == null) controller = GetComponent<PlayerController>();
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(GetHitCenter(), currentWeapon.Radius);
    }
}
