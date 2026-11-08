using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Projectile : MonoBehaviour
{
    [SerializeField] private float lifetime = 2f;  

    private Rigidbody2D rb;
    private GameObject owner;
    private int damage;
    private bool isCrit;
    private float knockback;

    private void Awake() => rb = GetComponent<Rigidbody2D>();

    public void Launch(Vector2 direction, float speed, int damage, bool isCrit,
                       float knockback, GameObject owner)
    {
        this.damage = damage;
        this.isCrit = isCrit;
        this.knockback = knockback;
        this.owner = owner;

        transform.right = direction;
        rb.linearVelocity = direction * speed;
        Destroy(gameObject, lifetime);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject == owner) return; // don't hit the shooter

        if (other.TryGetComponent(out IDamageable target))
        {
            target.TakeDamage(damage, isCrit);

            if (other.TryGetComponent(out IKnockable knockable))
                knockable.ApplyKnockback(rb.linearVelocity.normalized, knockback);

            Destroy(gameObject);
        }
        // Destroyed by walls, but passes through other triggers like coins.
        else if (!other.isTrigger)
        {
            Destroy(gameObject);
        }
    }
}