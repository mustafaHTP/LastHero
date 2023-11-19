using UnityEngine;

public class PlayerSwordAttack : MonoBehaviour
{
    [SerializeField] private int damagePoint;
    [SerializeField] private GameObject player;

    Animator _animator;
    BoxCollider2D _swordCollider;

    private void Awake()
    {
        _swordCollider = GetComponent<BoxCollider2D>();
        _animator = player.GetComponent<Animator>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        int enemyLayerMask = LayerMask.GetMask("Enemy");
        if (_swordCollider.IsTouchingLayers(enemyLayerMask))
        {
            if (collision.gameObject.TryGetComponent(out EnemyHealth enemyHealth))
            {
                enemyHealth.TakeDamage(damagePoint);
            }
        }
    }

    private void Attack()
    {
        _animator.SetTrigger("attack");
    }

    private void OnAttack()
    {
        Attack();
    }
}
