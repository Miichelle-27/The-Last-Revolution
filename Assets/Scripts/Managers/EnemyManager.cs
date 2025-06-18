using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    public static EnemyManager Instance { get; private set; } // Singleton: instancia única
    private List<GameObject> _activeEnemies = new List<GameObject>(); // Lista de enemigos activos
    private List<GameObject> _activeItems = new List<GameObject>(); // Lista de ítems activos
    public GameObject meleeEnemyPrefab; // Prefab base para enemigos melee
    public GameObject rangedEnemyPrefab; // Prefab base para enemigos ranged
    public GameObject bossPrefab; // Prefab base para el jefe
    public GameObject healthItemPrefab; // Prefab para ítems de salud

    private void Awake()
    {
        // Configura el Singleton
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // Spawnea enemigos e ítems para un nivel dado
    public void SpawnEnemiesForLevel(int levelIndex)
    {
        ClearEnemies(); // Limpia enemigos e ítems existentes
        LevelData levelData = GameManager.Instance.GetCurrentLevelData();
        if (levelData.levelIndex != levelIndex) return;

        // Spawnea enemigos
        for (int i = 0; i < levelData.enemyData.Length; i++)
        {
            EnemyData data = levelData.enemyData[i];
            GameObject prefab = data.enemyName.Contains("Boss") ? bossPrefab : data.isRanged ? rangedEnemyPrefab : meleeEnemyPrefab;
            GameObject enemy = Instantiate(prefab, levelData.enemySpawnPositions[i], Quaternion.identity);
            ConfigureEnemy(enemy, data);
            _activeEnemies.Add(enemy);
            EnemyHealth enemyHealth = enemy.GetComponent<EnemyHealth>();
            if (enemyHealth != null)
            {
                enemyHealth.OnDeath += () => CheckEnemiesCleared(levelIndex);
            }
        }

        // Spawnea ítems de salud
        if (levelData.healthItemPositions != null)
        {
            foreach (Vector2 position in levelData.healthItemPositions)
            {
                if (healthItemPrefab != null)
                {
                    GameObject item = Instantiate(healthItemPrefab, position, Quaternion.identity);
                    _activeItems.Add(item);
                }
                else
                {
                    Debug.LogError("healthItemPrefab no está asignado en EnemyManager!");
                }
            }
        }
    }

    // Configura las propiedades del enemigo según EnemyData
    private void ConfigureEnemy(GameObject enemy, EnemyData data)
    {
        EnemyHealth health = enemy.GetComponent<EnemyHealth>();
        if (health != null)
        {
            health.maxHealth = data.maxHealth;
        }
        EnemyMeleeController controller = enemy.GetComponent<EnemyMeleeController>();
        if (controller != null)
        {
            controller.patrolSpeed = data.moveSpeed;
            controller.chaseSpeed = data.moveSpeed * 1.5f;
            controller.chaseRange = data.chaseRange;
            controller.patrolPoints = data.patrolPoints;
            controller.meleeWeapon = data.meleeWeapon;
        }
        EnemyRangedController ranged = enemy.GetComponent<EnemyRangedController>();
        if (ranged != null)
        {
            ranged.rangedWeapon = data.rangedWeapon;
            ranged.detectionRange = data.chaseRange;
        }
        BossCombat boss = enemy.GetComponent<BossCombat>();
        if (boss != null)
        {
            boss.enemyData = data;
        }
    }

    // Verifica si todos los enemigos fueron eliminados
    private void CheckEnemiesCleared(int levelIndex)
    {
        _activeEnemies.RemoveAll(enemy => enemy == null);
        if (_activeEnemies.Count == 0)
        {
            SpawnKey(levelIndex);
        }
    }

    // Spawnea la llave del nivel
    private void SpawnKey(int levelIndex)
    {
        LevelData levelData = GameManager.Instance.GetCurrentLevelData();
        GameObject key = Instantiate(Resources.Load<GameObject>("KeyPrefab"), levelData.keySpawnPosition, Quaternion.identity);
        Debug.Log($"Llave aparecida en el nivel {levelIndex + 1}");
    }

    // Limpia todos los enemigos e ítems activos
    public void ClearEnemies()
    {
        foreach (GameObject enemy in _activeEnemies)
        {
            if (enemy != null)
            {
                Destroy(enemy);
            }
        }
        _activeEnemies.Clear();

        foreach (GameObject item in _activeItems)
        {
            if (item != null)
            {
                Destroy(item);
            }
        }
        _activeItems.Clear();
    }
}
