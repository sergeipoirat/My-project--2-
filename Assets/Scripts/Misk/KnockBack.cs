using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KnockBack : MonoBehaviour {

    [SerializeField] private float _knockBackMovingTimerMax = 0.5f;
    [SerializeField] private float knockBackForce = 3f;

    public bool GettingKnockedBack { get; private set; }

    Rigidbody2D rb;

    private float _knockBackMovingTimer;

    private void Awake() {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update() {
        _knockBackMovingTimer -= Time.deltaTime;
        if (_knockBackMovingTimer < 0) {
            StopKnockBackMovement();
        }
    }

    public void GetKnockedBack(Transform damageSource) {
        GettingKnockedBack = true;
        _knockBackMovingTimer = _knockBackMovingTimerMax;
        Vector2 difference = (transform.position - damageSource.position).normalized * knockBackForce;
        rb.AddForce(difference, ForceMode2D.Impulse);
    }

    public void StopKnockBackMovement() {
        rb.linearVelocity = Vector2.zero;
        GettingKnockedBack = false;
    }
}
