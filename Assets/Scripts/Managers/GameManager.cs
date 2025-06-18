using System.Collections.Generic;
using UnityEngine;
using System;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; } // Singleton
    [SerializeField] public LevelData[] levels; // Array de datos de niveles
    private List<int> _unlockedLevels = new List<int>(); // Niveles desbloqueados
    private int _currentLevel = 0; // Índice del nivel actual
    public event Action<int> OnLevelUnlocked; // Evento para desbloqueo
    private GameObject _flagPole; // Referencia al mástil

    private void Awake()
    {
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

    private void Start()
    {
        if (levels == null || levels.Length == 0)
        {
            Debug.LogError("El array de niveles en GameManager está vacío!");
            return;
        }
        if (EnemyManager.Instance == null)
        {
            Debug.LogError("EnemyManager no está inicializado!");
            return;
        }
        _flagPole = GameObject.Find("FlagPole"); // Encuentra el mástil
        _unlockedLevels.Add(-1); // Lobby desbloqueado
        UnlockLevel(0); // Nivel 1 desbloqueado
        SetCurrentLevel(-1); // Inicia en el lobby
    }

    public void UnlockLevel(int levelIndex)
    {
        if (!_unlockedLevels.Contains(levelIndex))
        {
            _unlockedLevels.Add(levelIndex);
            Debug.Log($"Nivel {levelIndex + 1} desbloqueado!");
            OnLevelUnlocked?.Invoke(levelIndex);
            ActivateLevel(levelIndex);
            if (levelIndex == 4)
            {
                CheckVictory();
            }
        }
    }

    public bool IsLevelUnlocked(int levelIndex)
    {
        return _unlockedLevels.Contains(levelIndex);
    }

    public void SetCurrentLevel(int levelIndex)
    {
        if (levelIndex == -1)
        {
            Debug.Log("Volviendo al lobby");
            if (_flagPole != null)
            {
                _flagPole.SetActive(true); // Activa el mástil en el lobby
            }
            if (EnemyManager.Instance != null)
            {
                EnemyManager.Instance.ClearEnemies();
            }
            else
            {
                Debug.LogError("EnemyManager no disponible!");
            }
        }
        else
        {
            _currentLevel = levelIndex;
            Debug.Log($"Nivel actual: {levelIndex + 1}");
            if (_flagPole != null)
            {
                _flagPole.SetActive(false); // Desactiva el mástil fuera del lobby
            }
        }
    }

    public LevelData GetCurrentLevelData()
    {
        return levels[_currentLevel];
    }

    private void ActivateLevel(int levelIndex)
    {
        if (EnemyManager.Instance != null)
        {
            EnemyManager.Instance.SpawnEnemiesForLevel(levelIndex);
        }
        else
        {
            Debug.LogError("EnemyManager no disponible!");
        }
    }

    private void CheckVictory()
    {
        Debug.Log("¡Has completado el nivel 5! ¡Victoria!");
    }
}