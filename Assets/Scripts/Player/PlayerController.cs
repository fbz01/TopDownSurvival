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
    private GameState currentState = GameState.Playing;
    private Vector2 moveInput;
    private Vector2 lastDirection = Vector2.down;
    public Vector2 FacingDirection => lastDirection;
    private Health health;
    private float knockedUntil;
    private bool IsKnockedBack => Time.time < knockedUntil;
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip deathSound;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        health = GetComponent<Health>();
    }
    private void OnEnable()
    {
        health.OnDied += HandleDeath;
        GameEvents.OnGameStateChanged += HandleGameStateChanged;
    }

    private void OnDisable()
    {
        health.OnDied -= HandleDeath;
        GameEvents.OnGameStateChanged -= HandleGameStateChanged;
    }

    private void HandleGameStateChanged(GameState state) => currentState = state;

    private void HandleDeath()
    {
        rb.linearVelocity = Vector2.zero;
        rb.bodyType = RigidbodyType2D.Kinematic;
        GetComponent<Collider2D>().enabled = false;
        GetComponent<Weapon>().enabled = false;
        animator.SetTrigger("Die");
        if (audioSource != null && deathSound != null)
        {
            audioSource.pitch = 1f;            // återställ tonhöjden från svärdsljuden
            audioSource.PlayOneShot(deathSound);
        }
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

        // P växlar mellan paus och spel
        if (keyboard.pKey.wasPressedThisFrame)
        {
            if (currentState == GameState.Playing) GameEvents.GameStateChanged(GameState.Paused);
            else if (currentState == GameState.Paused) GameEvents.GameStateChanged(GameState.Playing);
            return;
        }

        moveInput = Vector2.zero;
        if (currentState != GameState.Playing) return;

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

    public void ApplyKnockback(Vector2 direction, float force)
    {
        if (!enabled) return;

        rb.linearVelocity = direction * force;
        knockedUntil = Time.time + knockbackDuration;
    }
}