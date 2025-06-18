using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerHealth : MonoBehaviour
{
    public int maxHealth = 100; // Salud máxima
    private int _currentHealth; // Salud actual
    private Animator _animator; // Componente para animaciones
    private SpriteRenderer _spriteRenderer; // Para espejar sprites
    private PlayerMovement _playerMovement; // Para obtener dirección

    private void Awake()
    {
        // Inicializa salud y componentes
        _currentHealth = maxHealth; // Inicializa la salud
        _animator = GetComponent<Animator>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _playerMovement = GetComponent<PlayerMovement>();
    }

    // Aplica daño al jugador
    public void TakeDamage(int damage)
    {
        // Reduce salud y activa animaciones
        _currentHealth = Mathf.Max(0, _currentHealth - damage);
        Debug.Log($"Jugador: Salud = {_currentHealth}");

        // Ajusta espejado para Hurt/Death en izquierda
        Vector2 direction = _playerMovement.GetMoveDirection();
        _spriteRenderer.flipX = direction.x < 0; // Espeja si mira a la izquierda

        if (_currentHealth <= 0)
        {
            // Activa animación de muerte y desactiva movimiento/colisiones
            _animator.SetTrigger("isDead");
            _playerMovement.enabled = false;
            GetComponent<Collider2D>().enabled = false;
            Debug.Log("Jugador muerto!");
            // Opcional: Reiniciar escena
            // SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
        else
        {
            // Activa animación de daño
            _animator.SetTrigger("isDamaged");
        }
    }

    public void Heal(int amount)
    {
        // Aumenta salud
        _currentHealth = Mathf.Min(maxHealth, _currentHealth + amount);
        Debug.Log($"Jugador: Salud = {_currentHealth}");
    }
}
