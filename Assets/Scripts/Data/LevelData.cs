using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewLevelData", menuName = "RPG/LevelData")]
    public class LevelData : ScriptableObject
{
    [SerializeField] public int levelIndex; // Número del nivel (0 a 5, 0 = Lobby)
    [SerializeField] public Vector2 startPosition; // Posición inicial del jugador
    [SerializeField] public Vector2 keySpawnPosition; // Posición donde aparece la llave
    [SerializeField] public EnemyData[] enemyData; // Array de datos de enemigos
    [SerializeField] public Vector2[] enemySpawnPositions; // Posiciones de spawn de enemigos
    [SerializeField] public Vector2[] healthItemPositions; // Posiciones de ítems de salud
    [SerializeField] public GameObject gameObject; // Referencia al GameObject del nivel en la escena
}