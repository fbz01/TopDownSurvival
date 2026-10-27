using System;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody2D), typeof(PlayerStats), typeof(Health))]
public class PlayerController : MonoBehaviour, IKnockable
{
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float knockbackDuration = 0.15f;

    private Rigidbody2D rb;
    private Animator animator;
    private SpriteRenderer spriteRenderer;
    private Vector2 moveInput;
    private Vector2 lastDirection = Vector2.down;
    public Vector2 FacingDirection => lastDirection;
    private Health health;
    private float knockedUntil;
    private bool IsKnockedBack => Time.time < knockedUntil;
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
        rb.bodyType = RigidbodyType2D.Kinematic;
        GetComponent<Collider2D>().enabled = false;
        GetComponent<Weapon>().enabled = false;
        animator.SetTrigger("Die");
        enabled = false;
        GameEvents.PlayerDied();
        GameEvents.GameStateChanged(GameState.GameOver);
    }
    private void Update()
    {
        ReadInput();
        UpdateAnimation();
    }

    private void FixedUpdate()
    {
        if (IsKnockedBack)
        {
            rb.linearVelocity *= 0.85f;
            return;
        }

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

        // Pause game when p-key is pressed
        if (keyboard.pKey.isPressed) GameEvents.GameStateChanged(GameState.Paused);
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

    public void ApplyKnockback(Vector2 direction, float force)
    {
        if (!enabled) return;

        rb.linearVelocity = direction * force;
        knockedUntil = Time.time + knockbackDuration;
    }
}