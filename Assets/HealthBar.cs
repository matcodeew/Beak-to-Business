using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    [SerializeField] private Image _healthFill;

    public void UpdateHealthBar(float currentValue, float maxValue)
    {
        _healthFill.fillAmount = currentValue / maxValue;
    }
}
