using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class GameInput : MonoBehaviour {

    public static GameInput instance { get; private set; }

    private PlayerInputActions _playerInputActions;

    public event EventHandler OnPlayerAttack;

    private void Awake()
    {
        instance = this;
        _playerInputActions = new PlayerInputActions();
        _playerInputActions.Enable();

        _playerInputActions.Combat.Attack.started += PlayerAtack_Started;
    }

    public Vector2 GetMovementVector()
    {
        Vector2 inputVector = _playerInputActions.Player.Move.ReadValue<Vector2>();
        return inputVector;
    }

    public Vector3 GetMousePosition()
    {
        Vector3 mousePos = Mouse.current.position.ReadValue();
        return mousePos;
    }

    public void DisableMovement()
    {
        _playerInputActions.Disable();
    }
    private void PlayerAtack_Started(InputAction.CallbackContext obj)
    {
        OnPlayerAttack?.Invoke(this, EventArgs.Empty);
    }
}
