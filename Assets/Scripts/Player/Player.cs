using System;
using System.Collections;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.InputSystem;

[SelectionBase]
public class NewMonoBehaviourScript : MonoBehaviour
{
    public static NewMonoBehaviourScript Instance { get; private set; }
    public event EventHandler OnPlayerDeath;

    [SerializeField] private float _movingSpeed = 10f;
    [SerializeField] private int _maxHealth = 5;
    [SerializeField] private float _damageRecoveryTime = 0.5f;

    private Rigidbody2D _rb;
    private KnockBack _knockBack;

    Vector2 inputVector;
    private float _minMovementSpeed = 0.1f;
    private bool _isRunning = false;

    private int _currentHealth = 0;
    private bool _canTakeDamage;
    private bool _isAlive;

    private void Awake()
    {
        Instance = this;
        _rb = GetComponent<Rigidbody2D>();
        _knockBack = GetComponent<KnockBack>();
    }

    private void Start()
    {
        _currentHealth = _maxHealth;
        _canTakeDamage = true;
        _isAlive = true;
        GameInput.instance.OnPlayerAttack += Player_OnPlayerAttack;
    }

    private void Player_OnPlayerAttack(object sender, System.EventArgs e)
    {
        ActiveWeapon.Instance.GetActiveWeapon().Attack();
    }

    private void Update()
    {
        inputVector = GameInput.instance.GetMovementVector();
    }

    private void FixedUpdate()
    {
        if (_knockBack.GettingKnockedBack)
            return;
        HandleMovement();
    }

    public void TakeDamage(Transform damageSource, int damage)
    {
        if (_canTakeDamage && _isAlive)
        {
            _canTakeDamage = false;
            _currentHealth = Math.Max(0, _currentHealth -= damage);
            _knockBack.GetKnockedBack(damageSource);
            Debug.Log(_currentHealth);
            StartCoroutine(DamageRecoveryRoutine());
        }

        DetectDeath();
    }

    public bool IsAlive() => _isAlive;

    private void DetectDeath()
    {
        if (_currentHealth ==  0 && _isAlive) {
            _isAlive = false;
            GameInput.instance.DisableMovement();
            _knockBack.StopKnockBackMovement();
            OnPlayerDeath?.Invoke(this, EventArgs.Empty);
        }
    }

    private IEnumerator DamageRecoveryRoutine()
    {
        yield return new WaitForSeconds(_damageRecoveryTime);
        _canTakeDamage = true;
    }

    private void HandleMovement()
    {
        _rb.MovePosition(_rb.position + inputVector * (_movingSpeed * Time.fixedDeltaTime));

        if (Mathf.Abs(inputVector.x) > _minMovementSpeed || Mathf.Abs(inputVector.y) > _minMovementSpeed)
        {
            _isRunning = true;
        }
        else { 
            _isRunning = false;
        }
    }

    public bool IsRunning()
    {
        return _isRunning;
    }

    public Vector3 GetPlayerScreenPosition()
    {
        Vector3 playerScreenPosition = Camera.main.WorldToScreenPoint(transform.position);
        return playerScreenPosition;
    }
}
