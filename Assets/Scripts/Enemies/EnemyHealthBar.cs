using UnityEngine;
using UnityEngine.UI;

public class EnemyHealthBar : MonoBehaviour
{
    [Tooltip("Fill Image of the health bar")]
    public Image fillImage;

    private EnemyHealth _targetHealth;

    void Start()
    {
        // Busca el componente EnemyHealth en el padre o ancestros
        _targetHealth = GetComponentInParent<EnemyHealth>();
        if (_targetHealth == null)
            Debug.LogError("EnemyHealthBar: no se encontró EnemyHealth en el padre.");
    }

    void Update()
    {
        if (_targetHealth == null) return;

        // Calcula porcentaje de vida restante
        float pct = (float)_targetHealth.CurrentHealth / _targetHealth.MaxHealth;

        // Aplica al Image (Type = Filled, Fill Method = Horizontal)
        fillImage.fillAmount = pct;

        // Opcional: si muere, destruye la barra
        if (_targetHealth.CurrentHealth <= 0)
            Destroy(gameObject);
    }
}