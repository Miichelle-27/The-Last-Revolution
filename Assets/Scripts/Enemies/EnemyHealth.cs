using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class EnemyHealth : MonoBehaviour
{
    public int maxHealth = 100; // Salud máxima
    private int _currentHealth; // Salud actual
    public event Action OnDeath; // Evento al morir

    private void Start()
    {
        _currentHealth = maxHealth; // Inicializa la salud
    }

    // Aplica daño al enemigo
    public void TakeDamage(int damage)
    {
        _currentHealth -= damage;
        if (_currentHealth <= 0)
        {
            OnDeath?.Invoke(); // Notifica la muerte
            Destroy(gameObject); // Destruye el enemigo
        }
    }
}
