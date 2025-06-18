using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeAttackStrategy : IAttackStrategy
{
    private WeaponData _weapon; // Datos del arma
    private Transform _owner; // Objeto que realiza el ataque
    private LayerMask _targetLayer; // Capa de los objetivos (no usado por ahora)

    public MeleeAttackStrategy(WeaponData weapon, Transform owner, LayerMask? targetLayer)
    {
        this._weapon = weapon;
        this._owner = owner;
        this._targetLayer = targetLayer ?? LayerMask.GetMask("Default");
    }

    public void Attack()
    {
        // Detecta objetivos en el rango de ataque
        Collider2D[] hits = Physics2D.OverlapCircleAll(_owner.position, _weapon.attackRange, _targetLayer);
        foreach (Collider2D hit in hits)
        {
            if (_owner.CompareTag("Player") && hit.CompareTag("Enemy"))
            {
                EnemyHealth health = hit.GetComponent<EnemyHealth>();
                if (health != null)
                {
                    health.TakeDamage(_weapon.damage);
                    Debug.Log($"{_owner.name} golpeó a {hit.name} por {_weapon.damage}!");
                }
            }
            else if (_owner.CompareTag("Enemy") && hit.CompareTag("Player"))
            {
                PlayerHealth health = hit.GetComponent<PlayerHealth>();
                if (health != null)
                {
                    health.TakeDamage(_weapon.damage);
                    Debug.Log($"{_owner.name} golpeó al jugador por {_weapon.damage}!");
                }
            }
        }
    }
}
