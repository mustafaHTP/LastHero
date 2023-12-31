using System;
using UnityEngine;

public class PlayerCollision : MonoBehaviour
{
    BoxCollider2D _bodyCollider;
    PlayerHealth _playerHealth;

    private void Awake()
    {
        _playerHealth = GetComponent<PlayerHealth>();
        _bodyCollider = GetComponent<BoxCollider2D>();
    }

    private void FixedUpdate()
    {
        if (IsCollideWithHazard() && !_playerHealth.IsDead)
        {
            _playerHealth.Die();
        }
    }

    private bool IsCollideWithHazard()
    {
        int hazardLayerMask = LayerMask.GetMask("Hazard");
        if (_bodyCollider.IsTouchingLayers(hazardLayerMask))
        {
            return true;
        }

        return false;
    }
}
