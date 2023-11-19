using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    [Header("AI")]
    [Space(5)]
    [Tooltip("Sight range must be bigger than attack range")]
    [SerializeField] private float sightRange;
    [Tooltip("Attack range must be smaller than sight range")]
    [SerializeField] private float attackRange;
    [Tooltip("Determines the reference point " +
        "of the AI's center of sight")]
    [SerializeField] private Transform pivotPoint;
    [SerializeField] private Transform target;

    [Header("Movement")]
    [Space(5)]
    [SerializeField] private float moveSpeed;
    [Tooltip("It is used for flip object when collision happens")]
    [SerializeField] private BoxCollider2D sideCollider;


    private Rigidbody2D _rigidbody;
    private Vector2 _defaultLocalScale;
    private Animator _animator;
    private PlayerHealth _targetHealth;

    private bool _isTargetDeath;
    private bool _isProvoked;


    private enum Direction
    {
        Left,
        Right
    }

    private Direction _currentDirection;

    private void OnDrawGizmosSelected()
    {
        if (_currentDirection == Direction.Right)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawLine(pivotPoint.position,
                pivotPoint.position + new Vector3(sightRange, 0f, 0f));
            Gizmos.color = Color.red;
            Gizmos.DrawLine(pivotPoint.position,
                pivotPoint.position + new Vector3(attackRange, 0f, 0f));
        }
        else
        {
            Gizmos.color = Color.green;
            Gizmos.DrawLine(pivotPoint.position,
                pivotPoint.position - new Vector3(sightRange, 0f, 0f));
            Gizmos.color = Color.red;
            Gizmos.DrawLine(pivotPoint.position,
                pivotPoint.position - new Vector3(attackRange, 0f, 0f));
        }
    }

    private void Awake()
    {
        _targetHealth = target.GetComponent<PlayerHealth>();
        _animator = GetComponent<Animator>();
        _defaultLocalScale = transform.localScale;
        _rigidbody = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        //TODO: improved sight 
        //Vector2 directionToTarget = (target.position - pivotPoint.position).normalized;
        //Debug.DrawRay(pivotPoint.position, directionToTarget * 50f, Color.cyan);

        //float angle = Vector2.Angle(pivotPoint.transform.right, directionToTarget);
        //float signedAngle = Vector2.SignedAngle(pivotPoint.transform.right, directionToTarget);
        //Debug.Log("Angle: " + angle);
        //Debug.Log("Signed Angle: " + signedAngle);
        //Vector2 rightVector = new(pivotPoint.transform.right.x, pivotPoint.transform.right.y);

        //Quaternion upperVisitionRotation = Quaternion.AngleAxis(20f, pivotPoint.forward);

        //Vector2 upperVision = upperVisitionRotation * rightVector;

        //Debug.DrawRay(pivotPoint.position, upperVision * 50f, Color.cyan);

        /*
         * If the target is in sight range, 
         * enemy got provoked and focuses on target.
         * Otherwise enemy makes transition into patrol mode
         * **/
        _isProvoked = IsTargetInSightRange()
            || IsTargetInAttackRange();

        _isTargetDeath = _targetHealth.IsDead;

        if (!_isProvoked || _isTargetDeath)
        {
            Patrol();
        }
        else
        {
            if (IsTargetInAttackRange())
            {
                AttackTarget();
            }
            else
            {
                StopAttackTarget();
            }

            if (IsTargetInSightRange())
            {
                FollowTarget();
            }
            else
            {
                StopFollowTarget();
            }
        }
    }

    private void StopAttackTarget()
    {
        _animator.SetBool("isAttacking", false);
    }

    private void AttackTarget()
    {
        _animator.SetBool("isAttacking", true);
    }

    private void FollowTarget()
    {
        //Set animation to Run
        _animator.SetBool("isRunning", true);

        _currentDirection = (transform.position.x > target.position.x) switch
        {
            true => Direction.Left,
            _ => Direction.Right
        };

        if (_currentDirection == Direction.Left)
        {
            _rigidbody.velocity = new Vector2(-1f * moveSpeed, _rigidbody.velocity.y);
        }
        else
        {
            _rigidbody.velocity = new Vector2(moveSpeed, _rigidbody.velocity.y);
        }

        FaceTarget(_currentDirection);
    }

    private void StopFollowTarget()
    {
        _animator.SetBool("isRunning", false);
    }

    private void FaceTarget(Direction direction)
    {
        if (direction == Direction.Left)
        {
            transform.localScale = new Vector2(-1f * _defaultLocalScale.x, transform.localScale.y);
        }
        else
        {
            transform.localScale = new Vector2(_defaultLocalScale.x, transform.localScale.y);
        }
    }

    private bool IsTargetInAttackRange()
    {
        //Raycast only player layer
        Vector3 raycastDirection = _currentDirection switch
        {
            Direction.Right => pivotPoint.right,
            _ => -1f * pivotPoint.right
        };

        int playerLayerMask = LayerMask.GetMask("Player");
        RaycastHit2D hitInfo = Physics2D.Raycast(
            pivotPoint.position,
            raycastDirection,
            attackRange,
            playerLayerMask);

        if (hitInfo.collider != null)
        {
            if (hitInfo.distance < attackRange)
            {
                return true;
            }
        }
        return false;
    }

    private bool IsTargetInSightRange()
    {
        //Raycast only player layer
        Vector3 raycastDirection = _currentDirection switch
        {
            Direction.Right => pivotPoint.right,
            _ => -1f * pivotPoint.right
        };

        int playerLayerMask = LayerMask.GetMask("Player");
        RaycastHit2D hitInfo = Physics2D.Raycast(
            pivotPoint.position,
            raycastDirection,
            sightRange,
            playerLayerMask);

        if (hitInfo.collider != null)
        {
            if (hitInfo.distance >= attackRange && hitInfo.distance < sightRange)
            {
                return true;
            }
        }
        return false;
    }

    private void Patrol()
    {
        _animator.SetBool("isAttacking", false);
        _animator.SetBool("isRunning", true);

        int groundLayerMask = LayerMask.GetMask("Ground");
        if (sideCollider.IsTouchingLayers(groundLayerMask))
        {
            //FlipEnemy();
            if (_currentDirection == Direction.Left)
            {
                _currentDirection = Direction.Right;
            }
            else
            {
                _currentDirection = Direction.Left;
            }
        }

        if (_currentDirection == Direction.Left)
        {
            _rigidbody.velocity = new Vector2(-1f * moveSpeed, _rigidbody.velocity.y);
        }
        else
        {
            _rigidbody.velocity = new Vector2(moveSpeed, _rigidbody.velocity.y);
        }

        FaceTarget(_currentDirection);
    }
}
