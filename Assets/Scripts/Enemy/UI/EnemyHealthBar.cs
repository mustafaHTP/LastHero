using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyHealthBar : MonoBehaviour
{
    [SerializeField] private EnemyHealth enemyHealth;

    private Slider _healthBar;

    private void Awake()
    {
        _healthBar = GetComponent<Slider>();
    }

    private void Update()
    {
        float normalizedHealth = ((float)enemyHealth.CurrentHitPoint) / enemyHealth.MaxHitPoint;
        _healthBar.value = normalizedHealth;
    }
}
