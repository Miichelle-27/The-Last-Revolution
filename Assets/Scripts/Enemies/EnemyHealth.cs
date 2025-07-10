using UnityEngine;
using System;

public class EnemyHealth : MonoBehaviour
{
    [Header("Enemy Stats")]
    public EnemyData enemyData; // ScriptableObject con los datos del enemigo

    public int CurrentHealth { get; private set; } // Salud actual
    public int MaxHealth { get; private set; }     // Salud máxima (copiada desde EnemyData)

    public event Action OnDeath;

    private void Awake()
    {
        if (enemyData == null)
        {
            Debug.LogError("EnemyHealth: Falta asignar EnemyData en el inspector.");
            return;
        }

        MaxHealth = enemyData.maxHealth;
        CurrentHealth = MaxHealth;
    }

    public void TakeDamage(int damage)
    {
        CurrentHealth = Mathf.Max(CurrentHealth - damage, 0);

        if (CurrentHealth == 0)
        {
            OnDeath?.Invoke();
            Destroy(gameObject);
        }
    }
}