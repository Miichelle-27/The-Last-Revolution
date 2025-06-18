using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangedWeaponPickup : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerCombat combat = other.GetComponent<PlayerCombat>();
            if (combat != null)
            {
                combat.hasRangedWeapon = true;
                Destroy(gameObject); // Destruye el pickup
                Debug.Log("Arma ranged desbloqueada!");
            }
        }
    }
}
