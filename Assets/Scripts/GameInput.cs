using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class GameInput : MonoBehaviour
{
    private const string PLAYER_PREFS_BINDINGS = "InputBindings";

    public static GameInput Instance { get; private set; }

    public event EventHandler OnInteractAction;
    public event EventHandler OnInteractAlternateAction;
    public event EventHandler OnPauseAction;

    public enum Binding
    {
        Move_Up,
        Move_Down,
        Move_Left,
        Move_Right,
        Interact,
        InteractAlternate,
        Pause
    }

    private CharacterInputAction _characterInputAction;

    private void Awake()
    {
        Instance = this;
        _characterInputAction = new CharacterInputAction();

        if (PlayerPrefs.HasKey(PLAYER_PREFS_BINDINGS))
        {
            _characterInputAction.LoadBindingOverridesFromJson(PlayerPrefs.GetString(PLAYER_PREFS_BINDINGS));
        }

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

    public string GetBindingText(Binding binding)
    {
        switch (binding)
        {
            default:
            case Binding.Move_Up:
                return _characterInputAction.Player.Move.bindings[1].ToDisplayString();
            case Binding.Move_Down:
                return _characterInputAction.Player.Move.bindings[2].ToDisplayString();
            case Binding.Move_Left:
                return _characterInputAction.Player.Move.bindings[3].ToDisplayString();
            case Binding.Move_Right:
                return _characterInputAction.Player.Move.bindings[4].ToDisplayString();
            case Binding.Interact:
                return _characterInputAction.Player.Interact.bindings[0].ToDisplayString();
            case Binding.InteractAlternate:
                return _characterInputAction.Player.InteractAlternate.bindings[0].ToDisplayString();
            case Binding.Pause:
                return _characterInputAction.Player.Pause.bindings[0].ToDisplayString();
        }
    }

    public void RebindBinding(Binding binding, Action onActionRebound)
    {
        _characterInputAction.Player.Disable();

        InputAction inputAction;
        int bindingIndex;

        switch (binding)
        {
            default:
            case Binding.Move_Up:
                inputAction = _characterInputAction.Player.Move;
                bindingIndex = 1;
                break;
            case Binding.Move_Down:
                inputAction = _characterInputAction.Player.Move;
                bindingIndex = 2;
                break;
            case Binding.Move_Left:
                inputAction = _characterInputAction.Player.Move;
                bindingIndex = 3;
                break;
            case Binding.Move_Right:
                inputAction = _characterInputAction.Player.Move;
                bindingIndex = 4;
                break;
            case Binding.Interact:
                inputAction = _characterInputAction.Player.Interact;
                bindingIndex = 0;
                break;
            case Binding.InteractAlternate:
                inputAction = _characterInputAction.Player.InteractAlternate;
                bindingIndex = 0;
                break;
            case Binding.Pause:
                inputAction = _characterInputAction.Player.Pause;
                bindingIndex = 0;
                break;
        }

        inputAction.PerformInteractiveRebinding(bindingIndex)
            .OnComplete(callback =>
            {
                callback.Dispose();
                _characterInputAction.Player.Enable();
                onActionRebound?.Invoke();

                PlayerPrefs.SetString(PLAYER_PREFS_BINDINGS, _characterInputAction.SaveBindingOverridesAsJson());
                PlayerPrefs.Save();
            });
    }
}
