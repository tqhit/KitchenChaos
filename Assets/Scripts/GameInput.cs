using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameInput : MonoBehaviour
{
    public static GameInput Instance { get; private set; }

    public event EventHandler OnInteractAction;
    public event EventHandler OnInteractAlternateAction;
    public event EventHandler OnPauseAction;

    private CharacterInputAction _characterInputAction;

    private void Awake()
    {
        Instance = this;
        _characterInputAction = new CharacterInputAction();
        _characterInputAction.Player.Enable();
        _characterInputAction.Player.Interact.performed += Interact_performed;
        _characterInputAction.Player.InteractAlternate.performed += InteractAlternate_performed;
        _characterInputAction.Player.Pause.performed += Pause_performed;
    }

    private void OnDestroy()
    {
        _characterInputAction.Player.Interact.performed -= Interact_performed;
        _characterInputAction.Player.InteractAlternate.performed -= InteractAlternate_performed;
        _characterInputAction.Player.Pause.performed -= Pause_performed;

        _characterInputAction.Dispose();
    }

    private void Pause_performed(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        OnPauseAction?.Invoke(this, EventArgs.Empty);
    }

    private void InteractAlternate_performed(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        OnInteractAlternateAction?.Invoke(this, EventArgs.Empty);
    }

    private void Interact_performed(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        OnInteractAction?.Invoke(this, EventArgs.Empty);
    }

    public Vector2 GetMovementVectorNormalized()
    {
        Vector2 inputVector = _characterInputAction.Player.Move.ReadValue<Vector2>();

        inputVector = inputVector.normalized;

        return inputVector;
    }
}
