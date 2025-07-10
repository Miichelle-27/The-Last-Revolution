using UnityEngine;

public class LevelTimerTrigger : MonoBehaviour
{
    public float timerDuration = 30f;
    private bool _triggered = false;
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!_triggered && other.CompareTag("Player"))
        {
            _triggered = true;
            UIManager.Instance.StartLevel4Timer(timerDuration);
        } 
    }
}