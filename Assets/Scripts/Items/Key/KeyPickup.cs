using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyPickup : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            int nextLevel = GameManager.Instance.GetCurrentLevelData().levelIndex + 1;
            if (nextLevel < GameManager.Instance.levels.Length)
            {
                GameManager.Instance.UnlockLevel(nextLevel);
            }
            Destroy(gameObject); // Destruye la llave
            Debug.Log("Llave recogida!");
        }
    }
}
