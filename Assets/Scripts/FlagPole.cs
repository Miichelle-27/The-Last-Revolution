using UnityEngine;

public class FlagPole : MonoBehaviour
{
    [Header("Flag Y Positions")]
    public float flagStartY = 5f;   // Y position when the flag is at the top
    public float flagEndY = -2f;    // Y position when the flag is fully lowered

    [Header("Total levels to complete")]
    public int totalStages = 5;     // Number of stages/levels the player must complete

    private int currentStage = 0;   // Tracks how many levels have been completed
    private Vector3 startPos;       // Starting local position of the flag
    private Vector3 endPos;         // Final local position after lowering

    private void Start()
    {
        // Store the flag's original position
        startPos = transform.localPosition;

        // Set the target end position using the same X/Z and custom Y
        endPos = new Vector3(startPos.x, flagEndY, startPos.z);

        // Subscribe to the GameManager event: triggered when a level is unlocked
        if (GameManager.Instance != null)
        {
            GameManager.Instance.OnLevelUnlocked += OnLevelUnlocked;
        }
    }

    private void OnDestroy()
    {
        // Unsubscribe to avoid memory leaks when this object is destroyed
        if (GameManager.Instance != null)
        {
            GameManager.Instance.OnLevelUnlocked -= OnLevelUnlocked;
        }
    }

    // Called when a new level is unlocked by the GameManager
    private void OnLevelUnlocked(int levelIndex)
    {
        // Only respond to valid levels (avoid over-lowering or Lobby)
        if (levelIndex >= 0 && levelIndex < totalStages)
        {
            LowerFlag();
        }
    }

    // Moves the flag down proportionally to the number of levels completed
    public void LowerFlag()
    {
        currentStage++;

        // Calculate how far the flag should drop (0 to 1)
        float t = (float)currentStage / totalStages;

        // Interpolate between start and end positions based on progress
        transform.localPosition = Vector3.Lerp(startPos, endPos, t);
    }
}