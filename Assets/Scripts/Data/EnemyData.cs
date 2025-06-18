using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewEnemyData", menuName = "RPG/EnemyData")]
public class EnemyData : ScriptableObject
{
    public string enemyName; // Nombre del enemigo
    public int maxHealth; // Salud máxima del enemigo
    public float moveSpeed; // Usado por enemigos melee para patrullar/perseguir
    public float chaseRange; // Rango para detectar al jugador
    public Vector2[] patrolPoints; // Puntos de patrullaje (para melee)
    public bool isRanged; // ¿Es enemigo ranged?
    public WeaponData meleeWeapon; // Arma melee (para enemigos melee o jefe)
    public WeaponData rangedWeapon; // Arma ranged (para enemigos ranged o jefe)
}