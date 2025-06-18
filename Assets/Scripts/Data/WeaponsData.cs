using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewWeaponData", menuName = "RPG/WeaponData")]
    public class WeaponData : ScriptableObject
{
    public string weaponName; // Nombre del arma
    public int damage; // Daño del arma
    public float attackRange; // Rango (corto para melee, largo para ranged)
    public float attackCooldown; // Tiempo entre ataques
    public bool isRanged; // ¿Es un arma a distancia?
    public GameObject projectilePrefab; // Solo para armas ranged
    public ParticleSystem muzzleFlash; // Sistema de partículas para efecto visual de disparo
}
