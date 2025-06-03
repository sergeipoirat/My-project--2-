using UnityEngine;

public class PlayerVisual : MonoBehaviour
{
    private Animator animator;
    private SpriteRenderer spriteRenderer;

    private const string IS_RUNNING = "IsRunning";
    private const string IS_DIE = "IsDie";

    private void Awake()
    {
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void Start()
    {
        NewMonoBehaviourScript.Instance.OnPlayerDeath += Player_OnPlayerDeath;
    }

    private void Player_OnPlayerDeath(object sender, System.EventArgs e)
    {
        animator.SetBool(IS_DIE, true);
    }

    private void Update()
    {
        animator.SetBool(IS_RUNNING, NewMonoBehaviourScript.Instance.IsRunning());

        if (NewMonoBehaviourScript.Instance.IsAlive())
            AdjustPlayerFacingDirection();
    }

    private void AdjustPlayerFacingDirection()
    {
        Vector3 mousePos = GameInput.instance.GetMousePosition();
        Vector3 playerPos = NewMonoBehaviourScript.Instance.GetPlayerScreenPosition();

        if (mousePos.x < playerPos.x)
        {
            spriteRenderer.flipX = true;
        }
        else
        {
            spriteRenderer.flipX = false;
        }
    }
}
