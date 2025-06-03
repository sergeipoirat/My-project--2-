using TMPro;
using UnityEngine;

[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(SpriteRenderer))]
public class SkeletonVisual : MonoBehaviour
{
    [SerializeField] private EnemyAI _enemyAI;
    [SerializeField] private EnemyEntity _enemyEntity;
    [SerializeField] private GameObject _enemyShadow;

    private Animator animator;

    private const string _ISRUNNING = "IsRunning";
    private const string _TAKEHIT = "TakeHit";
    private const string _IS_DIE = "IsDie";
    private const string _CHASING_SPEED_MULTIPLIER = "ChasingSpeedMultiplier";
    private const string _ATTACK = "Attack";

    SpriteRenderer _spriteRenderer;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void Start()
    {
        _enemyAI.OnEnemyAttack += _enemyAI_OnEnemyAttack;
        _enemyEntity.OnTakeHit += _enemyEntity_OnTakeHit;
        _enemyEntity.OnDeath += _enemyEntity_OnDeath;
    }

    private void _enemyEntity_OnTakeHit(object sender, System.EventArgs e)
    {
        animator.SetTrigger(_TAKEHIT);
    }
    private void _enemyEntity_OnDeath(object sender, System.EventArgs e)
    {
        animator.SetBool(_IS_DIE, true);
        _spriteRenderer.sortingOrder = -1;
    }

    private void OnDestroy()
    {
        _enemyAI.OnEnemyAttack -= _enemyAI_OnEnemyAttack;
    }

    public void Update()
    {
        animator.SetBool(_ISRUNNING, _enemyAI.IsRunning());
        animator.SetFloat(_CHASING_SPEED_MULTIPLIER, _enemyAI.GetRoamingAnimationSpeed());
    }

    public void TriggerAttackAnimationTurnOff()
    {
        _enemyEntity.PolygonColliderTurnOff();
    }

    public void TriggerAttackAnimationTurnOn()
    {
        _enemyEntity.PolygonColliderTurnOn();
    }

    private void _enemyAI_OnEnemyAttack(object sender, System.EventArgs e)
    {
        animator.SetTrigger(_ATTACK);
    }
}
