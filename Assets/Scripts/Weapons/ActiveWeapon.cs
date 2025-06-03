using UnityEngine;

public class ActiveWeapon : MonoBehaviour
{
    public static ActiveWeapon Instance { get; private set; }

    [SerializeField] private Sword sword;

    public void Awake()
    {
        Instance = this;
    }

    private void Update()
    {
        if (NewMonoBehaviourScript.Instance.IsAlive())
            FollowMousePosition();
    }

    public Sword GetActiveWeapon()
    {
        return sword;
    }
    private void FollowMousePosition()
    {
        Vector3 mousePos = GameInput.instance.GetMousePosition();
        Vector3 playerPos = NewMonoBehaviourScript.Instance.GetPlayerScreenPosition();

        if (mousePos.x < playerPos.x)
        {
            transform.rotation = Quaternion.Euler(0, 180, 0);
        }
        else
        {
            transform.rotation = Quaternion.Euler(0, 0, 0);
        }
    }
}
