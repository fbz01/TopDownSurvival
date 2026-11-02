using System;
using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;

[RequireComponent(typeof(PlayerController), typeof(PlayerStats))]
public class Weapon : MonoBehaviour
{
    [SerializeField] private WeaponData currentWeapon;
    [SerializeField] private Vector2 bodyOffset = new Vector2(0f, 05f);
    [SerializeField] private Vector2 projectileOffset = new Vector2(0f, 0.3f);

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
        int damage = CalculateDamage(out bool isCrit);

        if (currentWeapon.IsRanged)
            StartCoroutine(FireProjectile(damage, isCrit, controller.FacingDirection));
        else
            MeleeHit(damage, isCrit);

        OnAttack?.Invoke(controller.FacingDirection);
    }

    private void MeleeHit(int damage, bool isCrit)
    {
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
    }

    private IEnumerator FireProjectile(int damage, bool isCrit, Vector2 direction)
    {
        WeaponData firedWith = currentWeapon;

        yield return new WaitForSeconds(firedWith.ReleaseDelay);

        Vector2 spawnPos = (Vector2)transform.position + projectileOffset + direction * 0.3f;
        Projectile arrow = Instantiate(firedWith.ProjectilePrefab, spawnPos, Quaternion.identity);
        arrow.Launch(direction, firedWith.ProjectileSpeed, damage, isCrit,
                     firedWith.KnockbackForce, gameObject);
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
