using System.Collections.Generic;
using UnityEngine;

public class SceneController : MonoBehaviour
{
    [Header("Rooms (Lobby + Levels)")]
    [Tooltip("Index 0 = Lobby, 1 = Level 1, ..., 5 = Level 5")]
    public List<GameObject> rooms;

    [Header("Camera Bounds for Each Room")]
    [Tooltip("Match order to Rooms list")]
    public List<Collider2D> cameraBounds;

    [Header("FlagPole (for lowering the flag)")]
    public FlagPole flagPole;

    private void Start()
    {
        // Ensure we start in the Lobby
        ShowRoom(-1);
    }

    private void OnEnable()
    {
        // Subscribe to GameManager events
        GameManager.Instance.OnLevelChanged  += ShowRoom;
        GameManager.Instance.OnLevelUnlocked += HandleLevelUnlocked;
    }

    private void OnDisable()
    {
        if (GameManager.Instance == null) return;
        GameManager.Instance.OnLevelChanged  -= ShowRoom;
        GameManager.Instance.OnLevelUnlocked -= HandleLevelUnlocked;
    }

    private void ShowRoom(int levelIndex)
    {
        // levelIndex == -1 => Lobby, 0 => Level 1, 1 => Level 2, ..., 4 => Level 5
        int targetRoom = (levelIndex < 0) ? 0 : levelIndex + 1;

        // Activate only the target room
        for (int i = 0; i < rooms.Count; i++)
            rooms[i].SetActive(i == targetRoom);

        // Update camera bounds to match
        if (CameraFollow.Instance != null && targetRoom < cameraBounds.Count)
            CameraFollow.Instance.SetBounds(cameraBounds[targetRoom]);
    }

    private void HandleLevelUnlocked(int levelIndex)
    {
        // When Level 5 (index 4) unlocks, lower the flag
        if (levelIndex == 4 && flagPole != null)
            flagPole.LowerFlag();
    }
}