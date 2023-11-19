using UnityEngine;
using UnityEngine.InputSystem;

public class EnemyHealth : MonoBehaviour
{
    [Header("Enemy Health Settings")]
    [Space(5)]
    [SerializeField]
    [Min(0)]
    private int maxHitPoint;

    [Header("Disabling On Death")]
    [Space(5)]
    [Tooltip("It is disabled when enemy dies")]
    [SerializeField] private Canvas _healthBar;

    private int currentHitPoint;
    private const int MinHitPoint = 0;

    private bool _isDead;

    public int MaxHitPoint { get => maxHitPoint; }
    public int CurrentHitPoint { get => currentHitPoint; }
    public bool IsDead { get => _isDead; }

    private Animator _animator;

    //Disabled when player dies
    EnemyAI _enemyAI;
    BoxCollider2D _bodyCollider;
    Rigidbody2D _rigidbody;

    private void Awake()
    {
        currentHitPoint = maxHitPoint;

        _enemyAI = GetComponent<EnemyAI>();
        _bodyCollider = GetComponent<BoxCollider2D>();
        _rigidbody = GetComponent<Rigidbody2D>();
        _animator = GetComponent<Animator>();
    }

    public void TakeDamage(int damageAmount)
    {
        /*
         * If enemy is already dead, don't take damage
         * **/
        if (_isDead) return;

        ProcessDamage(damageAmount);

        if (_isDead)
        {
            ProcessDeath();
        }
    }

    private void ProcessDeath()
    {
        _healthBar.enabled = false;
        _enemyAI.enabled = false;
        _bodyCollider.enabled = false;
        _rigidbody.simulated = false;
        _rigidbody.bodyType = RigidbodyType2D.Static;

        _animator.SetTrigger("die");
    }

    private void ProcessDamage(int damageAmount)
    {
        currentHitPoint -= damageAmount;
        if (currentHitPoint <= MinHitPoint)
        {
            currentHitPoint = MinHitPoint;
            _isDead = true;
        }
        _animator.SetTrigger("takeDamage");
    }
}
