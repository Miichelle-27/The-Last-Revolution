using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCombat : MonoBehaviour
{
    public WeaponData meleeWeapon; // Arma melee
    public WeaponData rangedWeapon; // Arma ranged
    public bool hasRangedWeapon = false; // Indica si el arma ranged está desbloqueada
    private float _meleeCooldownTimer = 0f; // Temporizador de enfriamiento melee
    private float _rangedCooldownTimer = 0f; // Temporizador de enfriamiento ranged
    private IAttackStrategy _meleeAttackStrategy; // Estrategia melee
    private IAttackStrategy _rangedAttackStrategy; // Estrategia ranged
    private PlayerControls _controls; // Input System controls
    private PlayerMovement _movement; // Referencia a PlayerMovement

    private void Awake()
    {
        _controls = new PlayerControls(); // Inicializa los controles
        _movement = GetComponent<PlayerMovement>(); // Obtiene PlayerMovement
        _meleeAttackStrategy = new MeleeAttackStrategy(meleeWeapon, transform, null);
        _rangedAttackStrategy = new RangedAttackStrategy(rangedWeapon, transform);
    }

    private void OnEnable()
    {
        _controls.Player.Enable();
        _controls.Player.AttackMelee.performed += ctx => PerformMeleeAttack(); // Ataque melee
        _controls.Player.AttackRanged.performed += ctx => PerformRangedAttack(); // Ataque ranged
    }

    private void OnDisable()
    {
       _controls.Player.Disable();
    }

    private void Update()
    {
        _meleeCooldownTimer -= Time.deltaTime; // Reduce enfriamiento melee
        _rangedCooldownTimer -= Time.deltaTime; // Reduce enfriamiento ranged
    }

    private void PerformMeleeAttack()
    {
        if (_meleeCooldownTimer <= 0)
        {
            _meleeAttackStrategy.Attack();
            _meleeCooldownTimer = meleeWeapon.attackCooldown;
            Debug.Log("Jugador: Ataque melee!");
        }
    }

    private void PerformRangedAttack()
    {
        if (hasRangedWeapon && _rangedCooldownTimer <= 0)
        {
            _rangedAttackStrategy.Attack();
            _rangedCooldownTimer = rangedWeapon.attackCooldown;
            Debug.Log("Jugador: Ataque ranged!");
        }
    }

    // Proporciona la dirección para ataques ranged (basada en movimiento)
    public Vector2 GetRangedDirection()
    {
        return _movement.GetMoveDirection();
    }
}
