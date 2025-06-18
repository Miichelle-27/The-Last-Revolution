using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMeleeController : MonoBehaviour
{
    public float patrolSpeed = 2f; // Velocidad de patrullaje
    public float chaseSpeed = 4f; // Velocidad de persecución
    public float chaseRange = 5f; // Rango para detectar al jugador
    public float attackRange = 1.5f; // Rango de ataque melee
    public float attackCooldown = 1f; // Enfriamiento entre ataques
    public Vector2[] patrolPoints; // Puntos de patrullaje
    public LayerMask obstacleLayer; // Capa para obstáculos (p. ej., paredes)
    private IEnemyMeleeState _currentState; // Estado actual
    private Transform _player; // Referencia al jugador
    private int _currentPatrolIndex = 0; // Índice del punto de patrullaje
    private float _attackCooldownTimer = 0f; // Temporizador de enfriamiento
    private IAttackStrategy _attackStrategy; // Estrategia de ataque melee
    public WeaponData meleeWeapon; // Arma melee (asignada desde EnemyData)

    private void Start()
    {
        _player = GameObject.FindGameObjectWithTag("Player").transform; // Encuentra al jugador
        _currentState = new MeleePatrolState(this); // Inicia en patrullaje
        _attackStrategy = new MeleeAttackStrategy(meleeWeapon, transform, null); // Inicializa estrategia melee
    }

    private void Update()
    {
        _attackCooldownTimer -= Time.deltaTime; // Reduce enfriamiento
        _currentState.UpdateState(); // Actualiza el estado
    }

    // Cambia el estado del enemigo
    public void ChangeState(IEnemyMeleeState newState)
    {
        _currentState = newState;
    }

    // Mueve al enemigo hacia un punto de patrullaje
    public void Patrol()
    {
        Vector2 target = patrolPoints[_currentPatrolIndex];
        transform.position = Vector2.MoveTowards(transform.position, target, patrolSpeed * Time.deltaTime);
        if (Vector2.Distance(transform.position, target) < 0.1f)
        {
            _currentPatrolIndex = (_currentPatrolIndex + 1) % patrolPoints.Length;
        }
    }

    // Persigue al jugador y ataca si está en rango
    public void Chase()
    {
        float distanceToPlayer = Vector2.Distance(transform.position, _player.position);
        if (distanceToPlayer <= attackRange && _attackCooldownTimer <= 0)
        {
            _attackStrategy.Attack(); // Ejecuta ataque melee
            _attackCooldownTimer = attackCooldown; // Reinicia enfriamiento
        }
        else
        {
            // Verifica si hay obstáculos
            Vector2 direction = (_player.position - transform.position).normalized;
            RaycastHit2D hit = Physics2D.Raycast(transform.position, direction, chaseRange, obstacleLayer);
            if (hit.collider == null || hit.collider.CompareTag("Player"))
            {
                transform.position = Vector2.MoveTowards(transform.position, _player.position, chaseSpeed * Time.deltaTime);
            }
        }
    }

    // Verifica si el jugador está en rango
    public bool IsPlayerInRange()
    {
        return Vector2.Distance(transform.position, _player.position) <= chaseRange;
    }
}
