using UnityEngine;

public class FlagPole : MonoBehaviour
{
    public Transform flag; // Referencia al GameObject de la bandera
    public float mastHeight = 4f; // Altura total del mástil
    public float flagStartY = 2f; // Posición Y inicial de la bandera (cima)
    public float flagEndY = -2f; // Posición Y final de la bandera (base)
    private int _maxLevels = 5; // Número total de niveles (1 a 5)

    private void Start()
    {
        if (flag == null)
        {
            Debug.LogError("Flag no asignado en FlagPole!");
            return;
        }

        // Suscribe al evento de niveles desbloqueados
        if (GameManager.Instance != null)
        {
            GameManager.Instance.OnLevelUnlocked += UpdateFlagPosition;
            // Inicializa la posición según niveles ya desbloqueados
            UpdateFlagPosition(GameManager.Instance.IsLevelUnlocked(0) ? 1 : 0);
        }
        else
        {
            Debug.LogError("GameManager no encontrado!");
        }
    }

    private void OnDestroy()
    {
        // Desuscribe para evitar memory leaks
        if (GameManager.Instance != null)
        {
            GameManager.Instance.OnLevelUnlocked -= UpdateFlagPosition;
        }
    }

    // Actualiza la posición de la bandera según el nivel desbloqueado
    private void UpdateFlagPosition(int levelIndex)
    {
        // Calcula el nivel actual (levelIndex + 1, ya que levelIndex empieza en 0)
        int currentLevel = levelIndex + 1;
        // Calcula la fracción de progreso (0 a 1)
        float progress = Mathf.Clamp01((float)currentLevel / _maxLevels);
        // Interpola la posición Y de la bandera
        float newY = Mathf.Lerp(flagStartY, flagEndY, progress);
        // Actualiza la posición local de la bandera
        flag.localPosition = new Vector3(flag.localPosition.x, newY, flag.localPosition.z);
        Debug.Log($"Bandera actualizada: Nivel {currentLevel}, Y={newY}");
    }
}