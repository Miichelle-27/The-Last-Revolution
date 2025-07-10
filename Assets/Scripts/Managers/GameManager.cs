using System;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    // Instancia única del GameManager (singleton) para acceder desde otros scripts
    public static GameManager Instance { get; private set; }

    // Arreglo de datos de niveles, configurado en el Inspector de Unity
    [SerializeField] public LevelData[] levels;
    // Lista de índices de niveles desbloqueados (por ejemplo, -1 para lobby, 0 para nivel 1, etc.)
    private List<int> _unlockedLevels = new List<int>();
    // Índice del nivel actual (-1 para lobby, 0 para nivel 1, 1 para nivel 2, etc.)
    private int _currentLevel = -1;

    // Evento que se dispara cuando cambia el nivel, usado por CameraFollow para actualizar límites
    public event Action<int> OnLevelChanged;
    // Evento que se dispara cuando se desbloquea un nivel
    public event Action<int> OnLevelUnlocked;

    // Referencia al objeto "FlagPole" en la escena (por ejemplo, un poste de bandera en el lobby)
    private GameObject _flagPole;

    // Método que se ejecuta al crear el GameObject
    private void Awake()
    {
        // Configura el singleton: asegura que solo haya una instancia de GameManager
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // Evita que se destruya al cambiar de escena
        }
        else
        {
            Destroy(gameObject); // Destruye duplicados
        }
    }

    // Método que se ejecuta al inicio de la escena
    private void Start()
    {
        // Verifica si el arreglo de niveles está configurado en el Inspector
        if (levels == null || levels.Length == 0)
        {
            Debug.LogError("¡El arreglo de datos de niveles está vacío! Configúralo en el Inspector.");
            return;
        }

        // Verifica si EnemyManager está inicializado (otro script que gestiona enemigos)
        if (EnemyManager.Instance == null)
        {
            Debug.LogError("¡EnemyManager no está inicializado!");
            return;
        }

        // Busca el objeto "FlagPole" en la escena
        _flagPole = GameObject.Find("FlagPole");
        if (_flagPole == null)
        {
            Debug.LogWarning("No se encontró el objeto 'FlagPole' en la escena.");
        }

        // Desbloquea el lobby (-1) y el primer nivel (0) al inicio
        _unlockedLevels.Add(-1); // Lobby
        UnlockLevel(0); // Nivel 1
        SetCurrentLevel(-1); // Comienza en el lobby
    }

    // Desbloquea un nivel y lo agrega a la lista de niveles desbloqueados
    public void UnlockLevel(int levelIndex)
    {
        // Solo desbloquea si el nivel no está ya desbloqueado
        if (!_unlockedLevels.Contains(levelIndex))
        {
            _unlockedLevels.Add(levelIndex);
            Debug.Log($"¡Nivel {levelIndex + 1} desbloqueado!");
            OnLevelUnlocked?.Invoke(levelIndex); // Notifica a otros scripts (por ejemplo, UI)
            ActivateLevel(levelIndex); // Activa enemigos para el nivel

            // Si se desbloquea el nivel 4 (quinto nivel), verifica la victoria
            if (levelIndex == 4)
                CheckVictory();
        }
    }

    // Verifica si un nivel está desbloqueado
    public bool IsLevelUnlocked(int levelIndex)
    {
        return _unlockedLevels.Contains(levelIndex);
    }

    // Establece el nivel actual y notifica el cambio
    public void SetCurrentLevel(int levelIndex)
    {
        _currentLevel = levelIndex; // Actualiza el nivel actual
        if (levelIndex == -1)
        {
            Debug.Log("Volviendo al lobby");
            if (_flagPole != null) _flagPole.SetActive(true); // Activa el poste en el lobby
            EnemyManager.Instance?.ClearEnemies(); // Limpia enemigos en el lobby
        }
        else
        {
            Debug.Log($"Nivel actual: {levelIndex + 1}");
            if (_flagPole != null) _flagPole.SetActive(false); // Desactiva el poste en niveles
        }
        OnLevelChanged?.Invoke(_currentLevel); // Notifica a CameraFollow y otros scripts
    }

    // Devuelve los datos del nivel actual, o null si es el lobby o el índice es inválido
    public LevelData GetCurrentLevelData()
    {
        // Si es el lobby (-1) o el índice es inválido, devuelve null
        if (_currentLevel == -1 || _currentLevel < 0 || _currentLevel >= levels.Length)
        {
            return null;
        }
        return levels[_currentLevel]; // Devuelve los datos del nivel actual
    }

    // Devuelve el índice del nivel actual (por ejemplo, -1 para lobby, 0 para nivel 1)
    public int GetCurrentLevelIndex()
    {
        return _currentLevel;
    }

    // Activa los enemigos para un nivel específico
    private void ActivateLevel(int levelIndex)
    {
        EnemyManager.Instance?.SpawnEnemiesForLevel(levelIndex);
    }

    // Muestra un mensaje de victoria cuando se completa el nivel 5
    private void CheckVictory()
    {
        Debug.Log("¡Nivel 5 completado! ¡Victoria final!");
        // Aquí puedes agregar lógica para mostrar una pantalla de victoria o créditos
    }

    // Desbloquea el siguiente nivel cuando se recoge una llave
    public void CollectKey(int currentLevel)
    {
        // Verifica que el nivel actual sea válido y que haya un nivel siguiente
        if (currentLevel >= -1 && currentLevel < levels.Length - 1)
        {
            UnlockLevel(currentLevel + 1); // Desbloquea el siguiente nivel
        }
    }

    // Inicia o reinicia un nivel si está desbloqueado
    public void StartLevel(int levelIndex)
    {
        if (!IsLevelUnlocked(levelIndex))
        {
            Debug.LogWarning("Intentando iniciar un nivel bloqueado.");
            return;
        }

        Debug.Log($"Reiniciando nivel {levelIndex}");
        SetCurrentLevel(levelIndex); // Establece el nivel actual
    }

    // Maneja el evento cuando se acaba el tiempo en un nivel
    public void TimeUp()
    {
        Debug.Log("¡Se acabó el tiempo! Reiniciando nivel...");
        StartLevel(_currentLevel); // Reinicia el nivel actual
        UIManager.Instance?.StopLevel4Timer(); // Detiene el temporizador del nivel 4
    }
}