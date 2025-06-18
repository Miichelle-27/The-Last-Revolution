using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossCombat : MonoBehaviour
{
    public EnemyData enemyData; // Datos del jefe
    private Transform _player; // Referencia al jugador
    private float _meleeCooldownTimer = 0f; // Temporizador melee
    private float _rangedCooldownTimer = 0f; // Temporizador ranged
    private IAttackStrategy _meleeAttackStrategy; // Estrategia melee
    private IAttackStrategy _rangedAttackStrategy; // Estrategia ranged

    private void Start()
    {
        _player = GameObject.FindGameObjectWithTag("Player").transform; // Encuentra al jugador
        _meleeAttackStrategy = new MeleeAttackStrategy(enemyData.meleeWeapon, transform, null);
        _rangedAttackStrategy = new RangedAttackStrategy(enemyData.rangedWeapon, transform);
    }

    private void Update()
    {
        _meleeCooldownTimer -= Time.deltaTime;
        _rangedCooldownTimer -= Time.deltaTime;

        float distanceToPlayer = Vector2.Distance(transform.position, _player.position);
        if (distanceToPlayer <= enemyData.meleeWeapon.attackRange && _meleeCooldownTimer <= 0)
        {
            _meleeAttackStrategy.Attack(); // Ataque melee
            _meleeCooldownTimer = enemyData.meleeWeapon.attackCooldown;
        }
        else if (distanceToPlayer <= enemyData.chaseRange && _rangedCooldownTimer <= 0)
        {
            _rangedAttackStrategy.Attack(); // Ataque ranged
            _rangedCooldownTimer = enemyData.rangedWeapon.attackCooldown;
        }
    }
}
