using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyRangedController : MonoBehaviour
{
    public WeaponData rangedWeapon; // Arma ranged
    public float detectionRange = 8f; // Rango de detección
    private Transform _player; // Referencia al jugador
    private float _attackCooldownTimer = 0f; // Temporizador de enfriamiento
    private IAttackStrategy _attackStrategy; // Estrategia de ataque ranged

    private void Start()
    {
        _player = GameObject.FindGameObjectWithTag("Player").transform; // Encuentra al jugador
        _attackStrategy = new RangedAttackStrategy(rangedWeapon, transform); // Inicializa estrategia
    }

    private void Update()
    {
        _attackCooldownTimer -= Time.deltaTime; // Reduce enfriamiento
        if (Vector2.Distance(transform.position, _player.position) <= detectionRange && _attackCooldownTimer <= 0)
        {
            _attackStrategy.Attack(); // Dispara al jugador
            _attackCooldownTimer = rangedWeapon.attackCooldown; // Reinicia enfriamiento
        }
    }
}
