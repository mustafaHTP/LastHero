using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealthBar : MonoBehaviour
{
    [Header("Player Health")]
    [Space(3)]
    [Tooltip("Move player here")]
    [SerializeField] private PlayerHealth _playerHealth;

    private Slider _healthBar;

    private void Awake()
    {
        _healthBar = GetComponent<Slider>();
    }

    private void Update()
    {
        float normalizedHealth = ((float)_playerHealth.CurrentHitPoint) / _playerHealth.MaxHitPoint;
        _healthBar.value = normalizedHealth;
    }
}
