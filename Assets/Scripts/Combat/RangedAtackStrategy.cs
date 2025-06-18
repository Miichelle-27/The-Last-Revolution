using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangedAttackStrategy : IAttackStrategy
{
    private WeaponData _weapon; // Datos del arma
    private Transform _owner; // Objeto que realiza el ataque
    private PlayerCombat _playerCombat; // Referencia a PlayerCombat (solo para jugador)

    public RangedAttackStrategy(WeaponData weapon, Transform owner)
    {
        this._weapon = weapon;
        this._owner = owner;
        if (owner.CompareTag("Player"))
        {
            _playerCombat = owner.GetComponent<PlayerCombat>(); // Obtiene PlayerCombat si es el jugador
        }
    }

    // Ejecuta el ataque ranged
    public void Attack()
    {
        // Calcula la dirección del disparo
        Vector2 direction;
        if (_owner.CompareTag("Player"))
        {
            direction = _playerCombat.GetRangedDirection(); // Usa la dirección de movimiento
            if (direction == Vector2.zero)
            {
                direction = Vector2.right; // Dirección por defecto si no hay movimiento
            }
        }
        else
        {
            // Para enemigos, apunta al jugador
            direction = ((Vector2)GameObject.FindGameObjectWithTag("Player").transform.position - (Vector2)_owner.position).normalized;
        }

        if (_weapon.projectilePrefab == null)
        {
            Debug.LogError($"Proyectil no asignado en el arma {_weapon.weaponName}!");
            return;
        }

        // Spawnea el proyectil
        GameObject projectile = Object.Instantiate(_weapon.projectilePrefab, _owner.position, Quaternion.identity);
        Projectile projectileScript = projectile.GetComponent<Projectile>();
        if (projectileScript != null)
        {
            projectileScript.Initialize(direction, _weapon.damage, _owner.tag);
        }
        else
        {
            Debug.LogError($"El prefab {_weapon.projectilePrefab.name} no tiene el componente Projectile!");
        }

        // Crea el efecto de partículas
        if (_weapon.muzzleFlash != null)
        {
            ParticleSystem muzzle = Object.Instantiate(_weapon.muzzleFlash, _owner.position, Quaternion.identity);
            Object.Destroy(muzzle.gameObject, muzzle.main.duration);
        }
        else
        {
            Debug.LogWarning($"Muzzle flash no asignado en el arma {_weapon.weaponName}!");
        }

        Debug.Log($"{_owner.name}: Ataque ranged!");
    }
}
