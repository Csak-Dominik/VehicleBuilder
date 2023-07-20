using System;
using UnityEngine;

// Singleton
public class InputManager
{
    private static InputManager _instance;
    public static InputManager Instance => _instance ??= new InputManager();

    // Fields
    private InputActions _inputActions = new InputActions();

    #region Events

    public Action<Vector2> OnMove;

    public Action<Vector2> OnLook;
    public Action<bool> OnSpace;
    public Action<bool> OnShift;
    public Action<bool> OnControl;

    public Action<bool> OnLeftClick;
    public Action<bool> OnRightClick;
    public Action<bool> OnMiddleClick;
    public Action<float> OnScroll;

    public Action<bool> OnEscape;
    public Action<bool> OnAlt;

    public Action<bool> OnInteractE;
    public Action<bool> OnInteractQ;
    public Action<bool> OnInteractF;

    #endregion Events

    private InputManager()
    {
        InitMoveAndLook();
        InitModifiers();
        InitMouse();
        InitInteract();

        // Enable
        _inputActions.Player.Enable();
    }

    private void InitMoveAndLook()
    {
        // Move and Look
        _inputActions.Player.Move.performed += ctx => OnMove?.Invoke(ctx.ReadValue<Vector2>());
        _inputActions.Player.Move.canceled += ctx => OnMove?.Invoke(Vector2.zero);

        _inputActions.Player.Look.performed += ctx => OnLook?.Invoke(ctx.ReadValue<Vector2>());
        _inputActions.Player.Look.canceled += ctx => OnLook?.Invoke(Vector2.zero);
    }

    private void InitModifiers()
    {
        // Modifier Keys
        _inputActions.Player.Space.performed += ctx => OnSpace?.Invoke(true);
        _inputActions.Player.Space.canceled += ctx => OnSpace?.Invoke(false);

        _inputActions.Player.Shift.performed += ctx => OnShift?.Invoke(true);
        _inputActions.Player.Shift.canceled += ctx => OnShift?.Invoke(false);

        _inputActions.Player.Control.performed += ctx => OnControl?.Invoke(true);
        _inputActions.Player.Control.canceled += ctx => OnControl?.Invoke(false);

        _inputActions.Player.Escape.performed += ctx => OnEscape?.Invoke(true);
        _inputActions.Player.Escape.canceled += ctx => OnEscape?.Invoke(false);

        _inputActions.Player.Alt.performed += ctx => OnAlt?.Invoke(true);
        _inputActions.Player.Alt.canceled += ctx => OnAlt?.Invoke(false);
    }

    private void InitMouse()
    {
        // Mouse
        _inputActions.Player.LeftClick.performed += ctx => OnLeftClick?.Invoke(true);
        _inputActions.Player.LeftClick.canceled += ctx => OnLeftClick?.Invoke(false);

        _inputActions.Player.RightClick.performed += ctx => OnRightClick?.Invoke(true);
        _inputActions.Player.RightClick.canceled += ctx => OnRightClick?.Invoke(false);

        _inputActions.Player.MiddleClick.performed += ctx => OnMiddleClick?.Invoke(true);
        _inputActions.Player.MiddleClick.canceled += ctx => OnMiddleClick?.Invoke(false);

        _inputActions.Player.Scroll.performed += ctx => OnScroll?.Invoke(ctx.ReadValue<float>());
        _inputActions.Player.Scroll.canceled += ctx => OnScroll?.Invoke(0);
    }

    private void InitInteract()
    {
        // Interact
        _inputActions.Player.InteractE.performed += ctx => OnInteractE?.Invoke(true);
        _inputActions.Player.InteractE.canceled += ctx => OnInteractE?.Invoke(false);

        _inputActions.Player.InteractQ.performed += ctx => OnInteractQ?.Invoke(true);
        _inputActions.Player.InteractQ.canceled += ctx => OnInteractQ?.Invoke(false);

        _inputActions.Player.InteractF.performed += ctx => OnInteractF?.Invoke(true);
        _inputActions.Player.InteractF.canceled += ctx => OnInteractF?.Invoke(false);
    }
}