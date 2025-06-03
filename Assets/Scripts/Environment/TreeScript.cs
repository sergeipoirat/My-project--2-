using UnityEngine;

public class TreeScript : MonoBehaviour
{
    [SerializeField] public float _maxStreangthOfTree = 15f;
    private float _currentStreangthOfTree;

    private Animator _animator;

    private void Awake()
    {
        _animator = GetComponent<Animator>();
    }

    private void Start()
    {
        _currentStreangthOfTree = _maxStreangthOfTree;
    }

    private void GetDamage(float damage)
    {
        _currentStreangthOfTree -= damage;
    }

    private void IsBroken()
    {
        if (_currentStreangthOfTree <= 0) return;
    }
}
