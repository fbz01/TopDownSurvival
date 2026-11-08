using UnityEngine;

[CreateAssetMenu(fileName = "NewWeapon", menuName = "Game/Weapon")]

// Data asset for one weapon. The attack behavior is the referenced AttackStrategy.
public class WeaponData : ScriptableObject
{
    [Header("Generellt")]
    [SerializeField] private string displayName = "Sword";
    [SerializeField] private Sprite icon;
    [SerializeField] private int baseDamage = 10;
    [SerializeField] private float attacksPerSecond = 2f;
    [SerializeField] private float knockbackForce = 6f;

    [Header("Attack")]
    [SerializeField] private AttackStrategy attackStrategy;

    [Header("Utseende och ljud")]
    [SerializeField] private AnimatorOverrideController animatorOverride;
    [SerializeField] private GameObject effectPrefab;
    [SerializeField] private AudioClip attackSound;

    public string DisplayName => displayName;
    public Sprite Icon => icon;
    public int BaseDamage => baseDamage;
    public float AttacksPerSecond => attacksPerSecond;
    public float KnockbackForce => knockbackForce;
    public AttackStrategy AttackStrategy => attackStrategy;
    public AnimatorOverrideController AnimatorOverride => animatorOverride;
    public GameObject EffectPrefab => effectPrefab;
    public AudioClip AttackSound => attackSound;
}