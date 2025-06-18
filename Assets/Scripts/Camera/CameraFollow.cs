using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform player;
    private Vector3 offset = new Vector3(0, 0, -10); // Offset fijo en el eje Z para la cámara

    void LateUpdate()
    {
        if (player != null)
        {
            transform.position = new Vector3(player.position.x, player.position.y, transform.position.z) + offset;
        }
    }

    // Método público para forzar la actualización inmediata (usado por GameManager)
    public void UpdatePosition()
    {
        if (player != null)
        {
            transform.position = new Vector3(player.position.x, player.position.y, transform.position.z) + offset;
        }
    }
    /*public Transform player; // Referencia al jugador
    public Vector3 offset = new Vector3(0, 0, -10); // Offset de la cámara
    public float smoothSpeed = 0.125f; // Velocidad de suavizado
    private Camera _cam; // Referencia a la cámara

    private void Start()
    {
        _cam = GetComponent<Camera>(); // Obtiene la cámara
        if (player == null)
        {
            player = GameObject.FindGameObjectWithTag("Player")?.transform; // Encuentra al jugador
            if (player == null)
            {
                Debug.LogError("No se encontró al jugador con el tag 'Player'!");
                return;
            }
        }
        // Ajusta la cámara al inicio y al cambio de nivel
        UpdateCameraPosition();
    }

    private void LateUpdate()
    {
        if (player == null) return;

        // Obtiene los límites del nivel actual desde LevelData
        LevelData levelData = GameManager.Instance.GetCurrentLevelData();
        Vector2 levelBoundsMin = levelData.keySpawnPosition - new Vector2(15, 10); // Ajusta según tu nivel
        Vector2 levelBoundsMax = levelData.keySpawnPosition + new Vector2(15, 10);

        // Calcula la posición deseada de la cámara
        Vector3 desiredPosition = player.position + offset;

        // Suaviza el movimiento
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);

        // Limita la posición dentro de los bounds
        float camHalfHeight = _cam.orthographicSize;
        float camHalfWidth = camHalfHeight * _cam.aspect;
        smoothedPosition.x = Mathf.Clamp(smoothedPosition.x, levelBoundsMin.x + camHalfWidth, levelBoundsMax.x - camHalfWidth);
        smoothedPosition.y = Mathf.Clamp(smoothedPosition.y, levelBoundsMin.y + camHalfHeight, levelBoundsMax.y - camHalfHeight);

        transform.position = smoothedPosition; // Actualiza la posición
    }
        
        // Método para actualizar la cámara manualmente (p. ej., tras teletransportación)
        public void UpdateCameraPosition()
        {
            if (player != null)
            {
                transform.position = player.position + offset; // Ajusta inmediatamente
            }
        }*/
}
