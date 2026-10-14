using UnityEngine;

[RequireComponent(typeof(Weapon), typeof(Animator))]
public class SwordSwing : MonoBehaviour
{
    [SerializeField] private GameObject slashEffectPrefab;
    [SerializeField] private float slashLifetime = 0.3f; // hur länge effekten syns

    private Weapon weapon;
    private Animator animator;

    private void Awake()
    {
        weapon = GetComponent<Weapon>();
        animator = GetComponent<Animator>();
    }

    private void OnEnable()  => weapon.OnAttack += HandleAttack;
    private void OnDisable() => weapon.OnAttack -= HandleAttack;

    private void HandleAttack(Vector2 direction)
    {
        animator.SetTrigger("Attack"); // gubben hugger själv i bilderna
        SpawnSlashEffect(direction);
    }

    private void SpawnSlashEffect(Vector2 direction)
    {
        if (slashEffectPrefab == null) return;

        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        GameObject slash = Instantiate(slashEffectPrefab, weapon.GetHitCenter(),
                                       Quaternion.Euler(0f, 0f, angle));
        Destroy(slash, slashLifetime);
    }
}