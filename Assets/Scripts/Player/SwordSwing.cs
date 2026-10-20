using UnityEngine;

[RequireComponent(typeof(Weapon), typeof(Animator))]
public class SwordSwing : MonoBehaviour
{
    [SerializeField] private GameObject slashEffectPrefab;
    [SerializeField] private float slashLifetime = 0.3f;
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private float pitchVariation = 0.1f;

    private Weapon weapon;
    private Animator animator;
    private RuntimeAnimatorController baseController;

    private void Awake()
    {
        weapon = GetComponent<Weapon>();
        animator = GetComponent<Animator>();
        baseController = animator.runtimeAnimatorController;
    }

    // ÄNDRAD: lyssnar nu även på vapenbyte
    private void OnEnable()
    {
        weapon.OnAttack += HandleAttack;
        weapon.OnWeaponChanged += ApplyWeaponVisuals;
    }

    private void OnDisable()
    {
        weapon.OnAttack -= HandleAttack;
        weapon.OnWeaponChanged -= ApplyWeaponVisuals;
    }

    private void Start()
    {
        ApplyWeaponVisuals(weapon.CurrentWeapon);
    }

    private void ApplyWeaponVisuals(WeaponData data)
    {
        if (data != null && data.AnimatorOverride != null)
            animator.runtimeAnimatorController = data.AnimatorOverride;
        else
            animator.runtimeAnimatorController = baseController;
    }

    private void HandleAttack(Vector2 direction)
    {
        animator.SetTrigger("Attack");
        SpawnSlashEffect(direction);
        PlayAttackSound();
    }

    private void SpawnSlashEffect(Vector2 direction)
    {
        GameObject prefab = weapon.CurrentWeapon.EffectPrefab != null
            ? weapon.CurrentWeapon.EffectPrefab
            : slashEffectPrefab;

        if (prefab == null) return;

        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        GameObject slash = Instantiate(prefab, weapon.GetHitCenter(),
                                       Quaternion.Euler(0f, 0f, angle));
        Destroy(slash, slashLifetime);
    }

    private void PlayAttackSound()
    {
        AudioClip clip = weapon.CurrentWeapon.AttackSound;
        if (clip == null || audioSource == null) return;

        audioSource.pitch = 1f + Random.Range(-pitchVariation, pitchVariation);
        audioSource.PlayOneShot(clip);
    }
}