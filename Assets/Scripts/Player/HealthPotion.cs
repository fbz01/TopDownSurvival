using System;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Health))]
public class HealthPotion : MonoBehaviour
{
    [SerializeField] private int healAmount = 30;
    [SerializeField] private float cooldown = 10f;

    private Health health;
    private float nextUseTime;

    public event Action OnPotionUsed;

    public float CooldownRemaining => Mathf.Max(0f, nextUseTime - Time.time);
    public float CooldownProgress => 1f - CooldownRemaining / cooldown;
    public bool IsReady => CooldownRemaining <= 0f;

    private void Awake() => health = GetComponent<Health>();

    private void Update()
    {
        Keyboard keyboard = Keyboard.current;
        if (keyboard == null) return;

        if (keyboard.qKey.wasPressedThisFrame)
            TryUse();
    }

    private void TryUse()
    {
        if (!IsReady) return;
        if (health.Current <= 0) return;
        if (health.Current >= health.Max) return; // don't use the potion at full health

        health.Heal(healAmount);
        nextUseTime = Time.time + cooldown;
        OnPotionUsed?.Invoke();
    }
}