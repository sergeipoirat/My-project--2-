using UnityEngine;
using UnityEngine.AI;
using Game.Utils;
using UnityEngine.EventSystems;
using System;

public class EnemyAI : MonoBehaviour
{
    [SerializeField] private State _startingState;
    [SerializeField] private float roamingDistanceMax = 7f;
    [SerializeField] private float roamingDistanceMin = 3f;
    [SerializeField] private float roamingTimerMax = 2f;
    [SerializeField] private bool _isChasingEnemy = false;
    [SerializeField] private bool _isAttackingEnemy = false;
    [SerializeField] private float _chasingDistance = 4f;
    [SerializeField] private float _chasingSpeedMultiplier = 2f;
    [SerializeField] private float _attackingDistance = 2f;
    [SerializeField] private float _attackRate = 2f;

    private float _nextAttackTime = 0f;

    private NavMeshAgent navMeshAgent;
    private BoxCollider2D boxCollider;
    private State _currentState;
    private float _roamingTime;
    private Vector3 _roamPosition;
    private Vector3 startingPosition;
    private float _roamingSpeed;
    private float _chasingSpeed;

    private float _nextCheckDirectionTime = 0f;
    private float _checkDirectionDuration = 0.1f;
    private Vector3 _lastPosition;

    public event EventHandler OnEnemyAttack;

    public EnemyAI(State state)
    {
        _currentState = state;
    }

    public enum State
    {
        Idle,
        Roaming,
        Chasing,
        Attacking,
        Death
    }

    private void Awake()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
        navMeshAgent.updateRotation = false;
        navMeshAgent.updateUpAxis = false;
        _currentState = _startingState;
        _roamingSpeed = navMeshAgent.speed;
        _chasingSpeed = navMeshAgent.speed * _chasingSpeedMultiplier;
    }

    private void Update()
    {
        StateHandler();
        MovementDirectionHandler();
    }

    public void SetDeathState()
    {
        navMeshAgent.ResetPath();
        _currentState = State.Death;
    }

    private void StateHandler()
    {
        switch (_currentState)
        {
            case State.Roaming:
                _roamingTime -= Time.deltaTime;
                if (_roamingTime < 0)
                {
                    Roaming();
                    _roamingTime = roamingTimerMax;
                }
                CheckCurrentState();
                break;
            case State.Chasing:
                ChasingTarget();
                CheckCurrentState();
                break;
            case State.Attacking:
                AttackingTarget();
                CheckCurrentState();
                break;
            case State.Death:
                break;
            default:
            case State.Idle:
                break;
        }
    }

    private void CheckCurrentState()
    {
        float distanceToPlayer = Vector3.Distance(transform.position, NewMonoBehaviourScript.Instance.transform.position);
        State newState = State.Roaming;

        if (_isChasingEnemy) {
            if (distanceToPlayer <= _chasingDistance)
            {
                newState = State.Chasing;
            }
        }

        if (_isAttackingEnemy) {
            if (distanceToPlayer < _attackingDistance)
            {
                newState = State.Attacking;
            }
        }

        if (newState != _currentState)
        {
            if (newState == State.Chasing)
            {
                navMeshAgent.ResetPath();
                navMeshAgent.speed = _chasingSpeed;
            } else if (newState == State.Roaming) {
                _roamingTime = 0f;
                navMeshAgent.speed = _roamingSpeed;
            } else if (newState == State.Attacking)
            {
                navMeshAgent.ResetPath();
            }

            _currentState = newState;
        }
    }

    private void AttackingTarget()
    {
        if (Time.time > _nextAttackTime)
        {
            OnEnemyAttack?.Invoke(this, EventArgs.Empty);
            _nextAttackTime = Time.time + _attackRate;
        }
    }

    private void MovementDirectionHandler()
    {
        if (Time.time > _nextCheckDirectionTime)
        {
            if (IsRunning())
            {
                ChangeFacingDirection(_lastPosition, transform.position);
            }
            else if (_currentState == State.Attacking)
            {
                ChangeFacingDirection(transform.position, NewMonoBehaviourScript.Instance.transform.position);
            }

            _lastPosition = transform.position;
            _nextCheckDirectionTime = Time.time + _checkDirectionDuration;
        }
    }

    private void ChasingTarget()
    {
        navMeshAgent.SetDestination(NewMonoBehaviourScript.Instance.transform.position);
    }

    public float GetRoamingAnimationSpeed()
    {
        return navMeshAgent.speed / _roamingSpeed;
    }

    public bool IsRunning()
    {
        if (navMeshAgent.velocity == Vector3.zero)
        {
            return false;
        }
        else return true;
    }

    private void Roaming()
    {
        startingPosition = transform.position;
        _roamPosition = GetRoamingPosition();
        navMeshAgent.SetDestination(_roamPosition);
    }

    private Vector3 GetRoamingPosition()
    {
        return startingPosition + Utils.GetRandomDir() * UnityEngine.Random.Range(roamingDistanceMin, roamingDistanceMax);
    }

    private void ChangeFacingDirection(Vector3 sourcePosition, Vector3 targetPosition)
    {
        if (sourcePosition.x > targetPosition.x) 
        {
            transform.rotation = Quaternion.Euler(0, -180, 0);
        } else
        {
            transform.rotation = Quaternion.Euler(0, 0, 0);
        }
    }
}
