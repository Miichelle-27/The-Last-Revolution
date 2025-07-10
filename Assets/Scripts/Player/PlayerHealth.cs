using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    public static PlayerHealth Instance;

    [Header("Health Settings")]
    public int maxHealth = 100;                  // Max HP
    public int CurrentHealth { get; private set; }  // HP tracker

    [Header("Animation & Visuals")]
    private Animator _animator;                  // For damage/death triggers
    private SpriteRenderer _spriteRenderer;      // For flipping on hit
    private PlayerMovement _playerMovement;      // To get facing direction

    // Tracks the current level index (as sent by GameManager.OnLevelChanged)
    private int _currentLevelIndex = -1;         // -1 = Lobby, 0 = Level 1, …

    private void Awake()
    {
        // Singleton setup
        if (Instance == null)
            Instance = this;
        else
        {
            Destroy(gameObject);
            return;
        }

        // Initialize health
        CurrentHealth = maxHealth;

        // Cache components
        _animator       = GetComponent<Animator>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _playerMovement = GetComponent<PlayerMovement>();
    }

    private void OnEnable()
    {
        // Subscribe to level‐change events to know where to respawn
        if (GameManager.Instance != null)
            GameManager.Instance.OnLevelChanged += levelIdx => _currentLevelIndex = levelIdx;
    }

    private void OnDisable()
    {
        // Unsubscribe for safety
        if (GameManager.Instance != null)
            GameManager.Instance.OnLevelChanged -= levelIdx => _currentLevelIndex = levelIdx;
    }

    private void Start()
    {
        // Update initial UI
        if (UIManager.Instance != null)
            UIManager.Instance.UpdateHealth(CurrentHealth, maxHealth);
    }

    // Applies damage, triggers animations, updates UI, and handles death & respawn via GameManager.
    public void TakeDamage(int damage)
    {
        // Reduce health, clamp at zero
        CurrentHealth = Mathf.Max(CurrentHealth - damage, 0);
        Debug.Log($"PlayerHealth: Took {damage}, now at {CurrentHealth} HP");

        // Flip sprite if hit while facing left
        if (_playerMovement != null)
        {
            Vector2 dir = _playerMovement.GetMoveDirection();
            _spriteRenderer.flipX = dir.x < 0;
        }

        // Trigger appropriate animation
        if (_animator != null)
        {
            if (CurrentHealth <= 0)
                _animator.SetTrigger("isDead");
            else
                _animator.SetTrigger("isDamaged");
        }

        // Update UI
        if (UIManager.Instance != null)
            UIManager.Instance.UpdateHealth(CurrentHealth, maxHealth);

        // If dead, handle respawn
        if (CurrentHealth <= 0)
            DieAndRespawn();
    }

    // Heals the player and updates UI.
    public void Heal(int amount)
    {
        CurrentHealth = Mathf.Min(CurrentHealth + amount, maxHealth);
        Debug.Log($"PlayerHealth: Healed {amount}, now at {CurrentHealth} HP");

        if (UIManager.Instance != null)
            UIManager.Instance.UpdateHealth(CurrentHealth, maxHealth);
    }

    // Called when the player dies: disables controls, then asks GameManager to reload the current room.
    private void DieAndRespawn()
    {
        Debug.Log("PlayerHealth: Handling death & respawn");

        // Disable player control & collision
        if (_playerMovement != null)
            _playerMovement.enabled = false;
        Collider2D col = GetComponent<Collider2D>();
        if (col != null)
            col.enabled = false;

        // Tell GameManager to reload this level (or lobby if idx == -1)
        if (GameManager.Instance != null)
            GameManager.Instance.SetCurrentLevel(_currentLevelIndex);
    }
}