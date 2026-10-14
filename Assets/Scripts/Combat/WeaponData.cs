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

    public string DisplayName => displayName;
    public Sprite Icon => icon;
    public int BaseDamage => baseDamage;
    public float AttacksPerSecond => attacksPerSecond;
    public float Range => range;
    public float Radius => radius; 
}
