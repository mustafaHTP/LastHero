using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerHealth : MonoBehaviour, IDamagable
{
    [Header("Game Session Data")]
    [Space(5)]
    [SerializeField] private PlayerSessionInfoSO playerSessionInfo;

    [SerializeField]
    [Min(0)]
    private int maxHitPoint;

    private int currentHitPoint;
    private const int MinHitPoint = 0;

    private bool _isDead;

    public int MaxHitPoint { get => maxHitPoint; }
    public int CurrentHitPoint { get => currentHitPoint; }
    public bool IsDead { get => _isDead; }

    private Animator _animator;

    //Disabled when player dies
    PlayerMovement _playerMovement;
    PlayerInput _playerInput;
    BoxCollider2D _bodyCollider;
    Rigidbody2D _rigidbody;

    private void Awake()
    {
        currentHitPoint = playerSessionInfo.Health;

        _playerMovement = GetComponent<PlayerMovement>();
        _playerInput = GetComponent<PlayerInput>();
        _bodyCollider = GetComponent<BoxCollider2D>();
        _rigidbody = GetComponent<Rigidbody2D>();
        _animator = GetComponent<Animator>();
    }

    public void TakeDamage(int damageAmount)
    {
        /*
         * If player is already dead, don't take damage
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
        _playerMovement.enabled = false;
        _playerInput.enabled = false;
        _bodyCollider.enabled = false;
        _rigidbody.simulated = false;
        _rigidbody.bodyType = RigidbodyType2D.Static;

        //GameManager.Instance.ResetGameSession();
        //GameManager.Instance.LoadInitialLevel();
        _animator.SetTrigger("die");

        GameManager.Instance.DisplayGameOverCanvas();
    }

    private void ProcessDamage(int damageAmount)
    {
        currentHitPoint -= damageAmount;
        if (currentHitPoint <= MinHitPoint)
        {
            currentHitPoint = MinHitPoint;
            _isDead = true;
        }

        playerSessionInfo.Health = currentHitPoint;

        _animator.SetTrigger("takeDamage");
    }
}
