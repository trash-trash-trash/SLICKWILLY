using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputHandler : MonoBehaviour
{
    public PlayerInputs playerControls;

    public bool initialized = false;

    public event Action<Vector2> AnnounceMouseMoved;

    public event Action<bool> AnnounceEscape;
    
    public event Action<Vector2> AnnounceMovement;
    
    public event Action<bool> AnnounceSpaceBar;

    public Vector2 moveInput;
    public Vector2 mousePosition;

    void Start()
    {
        playerControls = new PlayerInputs();
        
        playerControls.InGameActionMap.Aim.performed += OnAim;
        playerControls.InGameActionMap.Aim.canceled += OnAim;

        playerControls.InGameActionMap.Escape.performed += OnEscape;
        playerControls.InGameActionMap.Space.canceled += OnEscape;
        
        playerControls.InGameActionMap.Movement.performed += OnMove;
        playerControls.InGameActionMap.Movement.canceled += OnMove;
        
        playerControls.InGameActionMap.Space.performed += OnSpaceBar;
        playerControls.InGameActionMap.Space.canceled += OnSpaceBar;
        
        playerControls.Enable();
        
        initialized = true;
    }


    private void OnAim(InputAction.CallbackContext context)
    {
        mousePosition = context.ReadValue<Vector2>();
        AnnounceMouseMoved?.Invoke(mousePosition);
    }

    private void OnEscape(InputAction.CallbackContext context)
    {
        bool performed = context.performed;
        AnnounceEscape?.Invoke(performed);
    }

    private void OnMove(InputAction.CallbackContext context)
    {
        moveInput = context.ReadValue<Vector2>();
        AnnounceMovement?.Invoke(moveInput);
    }

    private void OnSpaceBar(InputAction.CallbackContext context)
    {
        bool performed = context.performed;
        AnnounceSpaceBar?.Invoke(performed);
    }
    

    void OnDisable()
    {
        
        playerControls.InGameActionMap.Aim.performed -= OnAim;
        playerControls.InGameActionMap.Aim.canceled -= OnAim;
        
        playerControls.InGameActionMap.Escape.performed -= OnEscape;
        playerControls.InGameActionMap.Space.canceled -= OnEscape;
        
        playerControls.InGameActionMap.Movement.performed -= OnMove;
        playerControls.InGameActionMap.Movement.canceled -= OnMove;
        
        playerControls.InGameActionMap.Space.performed -= OnSpaceBar;
        playerControls.InGameActionMap.Space.canceled -= OnSpaceBar;
    }
}