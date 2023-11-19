using UnityEngine;

public class EnemySwordAttack : MonoBehaviour
{
    [SerializeField] private int damagePoint;
    [SerializeField] private GameObject enemy;

    Animator _animator;
    BoxCollider2D _swordCollider;

    private void Awake()
    {
        _swordCollider = GetComponent<BoxCollider2D>();
        _animator = enemy.GetComponent<Animator>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        int playerLayerMask = LayerMask.GetMask("Player");
        if (_swordCollider.IsTouchingLayers(playerLayerMask))
        {
            if (collision.gameObject.TryGetComponent(out PlayerHealth playerHealth))
            {
                playerHealth.TakeDamage(damagePoint);
            }
        }
    }
}
