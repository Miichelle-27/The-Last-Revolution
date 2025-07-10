using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    // Instancia única del CameraFollow (singleton)
    public static CameraFollow Instance;

    [SerializeField] private float smoothSpeed = 0.125f; // Velocidad de suavizado (ajustable en el Inspector)
    public Transform player; // Referencia al Transform del jugador
    public Vector3 offset = new Vector3(0, 0, -11); // Offset de la cámara (z = -10 para 2D)
    private Camera _cam; // Componente de la cámara
    private Collider2D _currentBounds; // Límites actuales del nivel (para evitar el fondo azul)

    private void Awake()
    {
        // Configurar singleton
        if (Instance == null)
        {
            Instance = this;
            // Verificar si el GameObject es raíz antes de usar DontDestroyOnLoad
            if (transform.parent == null)
            {
                DontDestroyOnLoad(gameObject); // Persistir entre escenas
            }
            else
            {
                Debug.LogWarning("CameraFollow no es un root GameObject. Mueve la cámara al nivel superior de la jerarquía para usar DontDestroyOnLoad.");
            }
        }
        else
        {
            Destroy(gameObject); // Destruir duplicados
        }
    }

    private void Start()
    {
        // Obtener el componente Camera
        _cam = GetComponent<Camera>();
        if (_cam == null)
        {
            Debug.LogError("¡No se encontró el componente Camera en este GameObject!");
            return;
        }

        // Buscar al jugador si no está asignado
        if (player == null)
        {
            player = GameObject.FindGameObjectWithTag("Player")?.transform;
            if (player == null)
            {
                Debug.LogError("¡No se encontró al jugador con el tag 'Player'!");
                return;
            }
        }

        // Inicializar límites del nivel
        int levelIndex = -1; // Valor por defecto para el lobby
        if (GameManager.Instance != null)
        {
            levelIndex = GameManager.Instance.GetCurrentLevelIndex();
            GameManager.Instance.OnLevelChanged += UpdateBounds;
        }
        else
        {
            Debug.LogWarning("No se encontró GameManager. Usando límites del lobby.");
        }

        UpdateBounds(levelIndex); // Establecer límites iniciales
        UpdateCameraPosition(); // Posicionar la cámara al inicio
    }

    private void LateUpdate()
    {
        if (player == null || _cam == null) return;

        // Calcular la posición deseada de la cámara (jugador + offset)
        Vector3 desiredPosition = player.position + offset;

        // Limitar la cámara dentro de los bounds para evitar el fondo azul
        if (_currentBounds != null)
        {
            float camHalfHeight = _cam.orthographicSize;
            float camHalfWidth = camHalfHeight * _cam.aspect;
            Vector3 minBounds = _currentBounds.bounds.min + new Vector3(camHalfWidth, camHalfHeight, 0);
            Vector3 maxBounds = _currentBounds.bounds.max - new Vector3(camHalfWidth, camHalfHeight, 0);
            desiredPosition.x = Mathf.Clamp(desiredPosition.x, minBounds.x, maxBounds.x);
            desiredPosition.y = Mathf.Clamp(desiredPosition.y, minBounds.y, maxBounds.y);
            desiredPosition.z = offset.z;
        }

        // Mover la cámara con suavizado
        transform.position = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed * Time.deltaTime);
    }

    // Actualizar los límites según el nivel actual
    private void UpdateBounds(int levelIndex)
    {
        // Usar tag "Bounds_Lobby" para el lobby (-1) o "Bounds_LevelX" para niveles
        string boundsTag = levelIndex == -1 ? "Bounds_Lobby": $"Bounds_Level{levelIndex}";
        GameObject boundsObject = GameObject.FindGameObjectWithTag(boundsTag);
        _currentBounds = boundsObject?.GetComponent<Collider2D>();

        if (_currentBounds == null)
        {
            Debug.LogWarning($"No se encontró Collider2D con el tag '{boundsTag}' para el nivel {levelIndex}!");
        }
        else
        {
            UpdateCameraPosition(); // Ajustar la cámara cuando cambian los límites
        }
    }

    // Reposicionar la cámara instantáneamente (por ejemplo, tras teletransporte)
    public void UpdateCameraPosition()
    {
        if (player == null) return;

        Vector3 newPosition = player.position + offset;

        // Limitar dentro de los bounds
        if (_currentBounds != null)
        {
            float camHalfHeight = _cam.orthographicSize;
            float camHalfWidth = camHalfHeight * _cam.aspect;
            Vector3 minBounds = _currentBounds.bounds.min + new Vector3(camHalfWidth, camHalfHeight, 0);
            Vector3 maxBounds = _currentBounds.bounds.max - new Vector3(camHalfWidth, camHalfHeight, 0);
            newPosition.x = Mathf.Clamp(newPosition.x, minBounds.x, maxBounds.x);
            newPosition.y = Mathf.Clamp(newPosition.y, minBounds.y, maxBounds.y);
            newPosition.z = offset.z;
        }

        transform.position = newPosition;
    }

    // Establecer límites manualmente
    public void SetBounds(Collider2D newBounds)
    {
        _currentBounds = newBounds;
        UpdateCameraPosition();
    }

    private void OnEnable()
    {
        // Suscribirse al evento de cambio de nivel
        if (GameManager.Instance != null)
        {
            GameManager.Instance.OnLevelChanged += UpdateBounds;
        }
    }

    private void OnDisable()
    {
        // Desuscribirse del evento para evitar fugas de memoria
        if (GameManager.Instance != null)
        {
            GameManager.Instance.OnLevelChanged -= UpdateBounds;
        }
    }
}