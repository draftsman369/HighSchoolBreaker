using UnityEngine;
using UnityEngine.InputSystem;
using System;

public class InputReader : MonoBehaviour
{

    public event EventHandler OnInteractAction;

    public static InputReader Instance;

    private PlayerInputs playerInputs;

    private Vector2 moveInput;
    public Vector2 MoveInput => moveInput;

    private bool isSneaking;
    public bool IsSneaking => isSneaking;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        playerInputs = new PlayerInputs();

        playerInputs.Gameplay.Interact.performed += OnInteract;
    }

    private void OnInteract(InputAction.CallbackContext callback)
    {
        OnInteractAction?.Invoke(this, EventArgs.Empty);
        Debug.Log(callback);
    }

    private void OnEnable()
    {
        playerInputs.Gameplay.Enable();
    }

    private void OnDisable()
    {
        playerInputs.Gameplay.Disable();
    }

    private void Update()
    {
        moveInput = playerInputs.Gameplay.Move.ReadValue<Vector2>();
        isSneaking = playerInputs.Gameplay.Sneak.IsPressed();
    }
}