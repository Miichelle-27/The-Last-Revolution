using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    private Vector2 _direction; // Dirección del proyectil
    private float _speed = 10f; // Velocidad del proyectil
    private int _damage; // Daño del proyectil
    private string _ownerTag; // Tag del objeto que disparó
    private float _lifetime = 5f; // Tiempo de vida del proyectil

    // Inicializa el proyectil
    public void Initialize(Vector2 direction, int damage, string ownerTag)
    {
        this._direction = direction.normalized;
        this._damage = damage;
        this._ownerTag = ownerTag;
        Destroy(gameObject, _lifetime); // Destruye tras lifetime segundos
    }

    private void Update()
    {
        // Mueve el proyectil
        transform.Translate(_direction * _speed * Time.deltaTime, Space.World);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        // Aplica daño según el objetivo
        if (_ownerTag == "Player" && other.CompareTag("Enemy"))
        {
            EnemyHealth health = other.GetComponent<EnemyHealth>();
            if (health != null)
            {
                health.TakeDamage(_damage);
                Destroy(gameObject); // Destruye el proyectil
            }
        }
        else if (_ownerTag == "Enemy" && other.CompareTag("Player"))
        {
            PlayerHealth health = other.GetComponent<PlayerHealth>();
            if (health != null)
            {
                health.TakeDamage(_damage);
                Destroy(gameObject); // Destruye el proyectil
            }
        }
    }
}
