using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    [SerializeField] private float moveSpeed;
    [Tooltip("It is used for flip object when collision happens")]
    [SerializeField] private BoxCollider2D sideCollider;

    private Rigidbody2D _rigidbody;
    private Vector2 _defaultLocalScale;

    private void Awake()
    {
        _defaultLocalScale = transform.localScale;
        _rigidbody = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        Move();
    }

    private void Move()
    {
        int groundLayerMask = LayerMask.GetMask("Ground");
        if (sideCollider.IsTouchingLayers(groundLayerMask))
        {
            FlipEnemy();
            moveSpeed *= -1f;
        }
        _rigidbody.velocity = new Vector2(moveSpeed, _rigidbody.velocity.y);
    }

    private void FlipEnemy()
    {
        float currentLocalScaleX = transform.localScale.x;
        transform.localScale = new Vector2(-1f * currentLocalScaleX, transform.localScale.y);
    }
}
