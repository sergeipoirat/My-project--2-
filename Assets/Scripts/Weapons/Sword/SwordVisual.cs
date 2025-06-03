using UnityEngine;

public class SwordVisual : MonoBehaviour
{
    [SerializeField] private Sword sword;
    private Animator animator;
    private const string Attack = "Atack";

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    private void Start()
    {
        sword.OnSwordSwing += Sword_OnSwordSwing;
    }

    private void Sword_OnSwordSwing(object sender, System.EventArgs e)
    {
        animator.SetTrigger(Attack);
    }

    public void TriggerEndAttackAnimation()
    {
        sword.AttackColiderTurnOff();
    }
}
