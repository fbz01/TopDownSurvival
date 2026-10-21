using UnityEngine;

[CreateAssetMenu(fileName = "NewWeapon", menuName = "Game/Weapon")]

public class WeaponData : ScriptableObject
{
    [SerializeField] private string displayName = "Sword";
    [SerializeField] private Sprite icon;
    [SerializeField] private int baseDamage = 10;
    [SerializeField] private float attacksPerSecond = 2f;
    [SerializeField] private float range = 0.8f;
    [SerializeField] private float radius = 0.6f;
    [SerializeField] private float knockbackForce = 6f;
    [SerializeField] private AnimatorOverrideController animatorOverride;
    [SerializeField] private GameObject effectPrefab;
    [SerializeField] private AudioClip attackSound;
    [SerializeField] private Projectile projectilePrefab;
    [SerializeField] private float projectileSpeed = 12f;
    [SerializeField] private float releaseDelay = 0.17f;
    
    public Projectile ProjectilePrefab => projectilePrefab;
    public float ProjectileSpeed => projectileSpeed;
    public float ReleaseDelay => releaseDelay;
    public bool IsRanged => projectilePrefab != null;

    public string DisplayName => displayName;
    public Sprite Icon => icon;
    public int BaseDamage => baseDamage;
    public float AttacksPerSecond => attacksPerSecond;
    public float Range => range;
    public float Radius => radius;
    public float KnockbackForce => knockbackForce;
    public AnimatorOverrideController AnimatorOverride => animatorOverride;
    public GameObject EffectPrefab => effectPrefab;
    public AudioClip AttackSound => attackSound;
}
