using System;
using UnityEngine;

public class Sword : MonoBehaviour
{
    [SerializeField] private int _damageAmount = 2;

    public event EventHandler OnSwordSwing;
    private PolygonCollider2D _polygonColider2D;

    private void Awake()
    {
        _polygonColider2D = GetComponent<PolygonCollider2D>();
    }

    private void Start()
    {
        AttackColiderTurnOff();
    }

    public void Attack()
    {
        AttackColiderTurnOffOn();
        OnSwordSwing?.Invoke(this, EventArgs.Empty);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.transform.TryGetComponent(out EnemyEntity enemyEntity))
        {
            enemyEntity.TakeDamage(_damageAmount);
        }
    }

    public void AttackColiderTurnOff()
    {
        _polygonColider2D.enabled = false;
    }

    private void AttackColiderTurnOn()
    {
        _polygonColider2D.enabled = true;
    }

    private void AttackColiderTurnOffOn()
    {
        AttackColiderTurnOff();
        AttackColiderTurnOn();
    }
}
