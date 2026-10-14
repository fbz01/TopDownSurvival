using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody2D), typeof(PlayerStats), typeof(Health))]
public class PlayerController : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 5f;

    private Rigidbody2D rb;
    private Animator animator;
    private SpriteRenderer spriteRenderer;
    private Vector2 moveInput;
    private Vector2 lastDirection = Vector2.down;
    public Vector2 FacingDirection => lastDirection;
    private Health health;
    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        health = GetComponent<Health>();
    }
    private void OnEnable() => health.OnDied += HandleDeath;
    private void OnDisable() => health.OnDied -= HandleDeath;

    private void HandleDeath()
    {
        rb.linearVelocity = Vector2.zero;
        rb.bodyType = RigidbodyType2D.Kinematic;  // påverkas inte längre av knuffar
        GetComponent<Collider2D>().enabled = false;
        enabled = false;
        GameEvents.PlayerDied();
    }
    private void Update()
    {
        ReadInput();
        UpdateAnimation();
    }

    private void FixedUpdate()
    {
        rb.linearVelocity = moveInput * moveSpeed;
    }

    private void ReadInput()
    {
        Keyboard keyboard = Keyboard.current;
        if (keyboard == null) return;

        moveInput = Vector2.zero;
        if (keyboard.wKey.isPressed) moveInput.y += 1;
        if (keyboard.sKey.isPressed) moveInput.y -= 1;
        if (keyboard.dKey.isPressed) moveInput.x += 1;
        if (keyboard.aKey.isPressed) moveInput.x -= 1;

        moveInput = moveInput.normalized;
    }

    private void UpdateAnimation()
    {
        bool isMoving = moveInput != Vector2.zero;

        if (isMoving)
            lastDirection = moveInput;

        animator.SetBool("IsMoving", isMoving);
        animator.SetFloat("MoveX", lastDirection.x);
        animator.SetFloat("MoveY", lastDirection.y);

        spriteRenderer.flipX = lastDirection.x < 0;
    }
}