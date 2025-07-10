using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;

    [Header("Player UI")]
    public Image healthBarFill;
    public TextMeshProUGUI healthText;

    [Header("Level 4 Timer")]
    public GameObject timerPanel;
    public TextMeshProUGUI timerText;

    private float _level4Timer;
    private bool _timerRunning;

    private void Awake()
    {
        // Singleton setup
        if (Instance == null) Instance = this;
        else
        {
            Destroy(gameObject);
            return;
        }
    }

    private void OnEnable()
    {
        // Subscribe to level‐change to reset UI on entering any room
        if (GameManager.Instance != null)
            GameManager.Instance.OnLevelChanged += HandleLevelChanged;
    }

    private void OnDisable()
    {
        // Unsubscribe to avoid leaks
        if (GameManager.Instance != null)
            GameManager.Instance.OnLevelChanged -= HandleLevelChanged;
    }

    private void Start()
    {
        // Initialize UI
        UpdateHealth(PlayerHealth.Instance.CurrentHealth, PlayerHealth.Instance.maxHealth);
        timerPanel.SetActive(false);
    }

    private void Update()
    {
        // Level 4 timer logic
        if (_timerRunning)
        {
            _level4Timer -= Time.deltaTime;
            timerText.text = Mathf.Ceil(_level4Timer).ToString("0");
            if (_level4Timer <= 0f)
            {
                _timerRunning = false;
                GameManager.Instance.TimeUp();
            }
        }
    }

    // Called whenever GameManager.SetCurrentLevel(...) fires. Resets health bar and hides any running timer.
    private void HandleLevelChanged(int levelIdx)
    {
        // Reset health UI to current actual values
        UpdateHealth(PlayerHealth.Instance.CurrentHealth, PlayerHealth.Instance.maxHealth);

        // Ensure timer is hidden if we just left level 4
        if (timerPanel.activeSelf)
            StopLevel4Timer();
    }
    
    // Updates the health bar fill and text.
    public void UpdateHealth(int current, int max)
    {
        float pct = (float)current / max;
        healthBarFill.fillAmount = pct;
        healthText.text = $"{current} / {max}";
    }
    
    // Activates and starts the Level 4 timer UI.
    public void StartLevel4Timer(float duration)
    {
        _level4Timer = duration;
        timerPanel.SetActive(true);
        _timerRunning = true;
    }

    // Stops and hides the Level 4 timer UI.
    public void StopLevel4Timer()
    {
        _timerRunning = false;
        timerPanel.SetActive(false);
    }
}
