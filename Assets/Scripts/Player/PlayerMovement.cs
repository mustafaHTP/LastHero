using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [Header("Movement Settings")]
    [Space(2)]
    [SerializeField] private float jumpSpeed;
    [SerializeField] private float moveSpeed;
    [SerializeField] private float climbSpeed;

    [Header("Gravity Scale Settings")]
    [SerializeField] private float gravityScale;

    [SerializeField] private BoxCollider2D feetCollider;
    private BoxCollider2D _bodyCollider;

    private Rigidbody2D _rigidbody;
    private Animator _animator;
    private Vector2 _defaultLocalScale;

    //Inputs
    private float _moveInput;
    private float _climbInput;

    //States
    private bool _isGrounded;

    private void Awake()
    {
        _defaultLocalScale = transform.localScale;
        _rigidbody = GetComponent<Rigidbody2D>();
        _animator = GetComponent<Animator>();
        _bodyCollider = GetComponent<BoxCollider2D>();
    }

    private void FixedUpdate()
    {
        //Check hero is grounded for animator
        if (IsGrounded())
        {
            _animator.SetBool("isGrounded", true);
        }
        else
        {
            _animator.SetBool("isGrounded", false);
        }

        //Set velocity for animator
        _animator.SetFloat("verticalVelocity", _rigidbody.velocity.y);

        Move();
        Climb();
    }

    private void Move()
    {
        FlipPlayer();
        SetMoveAnimation();

        //Move
        float moveVelocity = moveSpeed * _moveInput;
        _rigidbody.velocity = new Vector2(moveVelocity, _rigidbody.velocity.y);
    }

    private void SetMoveAnimation()
    {
        bool hasMoveInput = Mathf.Abs(_moveInput) > Mathf.Epsilon;
        _animator.SetBool("isRunning", hasMoveInput);
    }

    private void FlipPlayer()
    {
        if (_moveInput > 0f)
        {
            transform.localScale = new Vector2(
                _defaultLocalScale.x,
                _defaultLocalScale.y);
        }
        else if (_moveInput < 0f)
        {
            transform.localScale = new Vector2(
                -1f * _defaultLocalScale.x,
                _defaultLocalScale.y);
        }
    }

    private void Jump()
    {
        if (IsGrounded())
        {
            _animator.SetTrigger("jump");
            _rigidbody.velocity = new Vector2(0f, jumpSpeed);
        }
    }

    private void Climb()
    {
        int climbLayerMask = LayerMask.GetMask("Climb");
        if (_bodyCollider.IsTouchingLayers(climbLayerMask))
        {
            _rigidbody.gravityScale = 0f;
            float climbVelocity = climbSpeed * _climbInput;
            _rigidbody.velocity = new Vector2(_rigidbody.velocity.x, climbVelocity);
        }
        else
        {
            _rigidbody.gravityScale = gravityScale;
        }
    }

    private bool IsGrounded()
    {
        int groundLayerMask = LayerMask.GetMask("Ground");
        if (feetCollider.IsTouchingLayers(groundLayerMask))
        {
            _isGrounded = true;
        }
        else
        {
            _isGrounded = false;
        }

        return _isGrounded;
    }

    private void OnMove(InputValue inputValue)
    {
        _moveInput = inputValue.Get<float>();
    }

    private void OnJump(InputValue inputValue)
    {
        Jump();
    }

    private void OnClimb(InputValue inputValue)
    {
        _climbInput = inputValue.Get<float>();
    }
}
