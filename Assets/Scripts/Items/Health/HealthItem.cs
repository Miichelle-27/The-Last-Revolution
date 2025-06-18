using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthItem : MonoBehaviour
{
    public int healAmount = 20; // Cantidad de salud restaurada

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerHealth health = other.GetComponent<PlayerHealth>();
            if (health != null)
            {
                health.Heal(healAmount); // Restaura salud
                Destroy(gameObject); // Destruye el ítem
                Debug.Log($"Jugador recogió ítem de salud: +{healAmount} HP");
            }
        }
    }
}
