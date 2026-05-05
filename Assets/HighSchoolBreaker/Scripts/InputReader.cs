using UnityEngine;
using UnityEngine.InputSystem;
using System;

public class InputReader : MonoBehaviour
{

    public event EventHandler OnInteractAction;
    public event EventHandler OnScreamAction;

    public static InputReader Instance;

    private PlayerInputs playerInputs;

    private Vector2 moveInput;
    public Vector2 MoveInput => moveInput;

    private bool isSneaking;
    public bool IsSneaking => isSneaking;
    private bool isScreaming;
    public bool IsScreaming => isScreaming;

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
        playerInputs.Gameplay.Scream.performed += OnScream;
    }

    private void OnInteract(InputAction.CallbackContext callback)
    {
        OnInteractAction?.Invoke(this, EventArgs.Empty);
        Debug.Log(callback);
    }

    public void OnScream(InputAction.CallbackContext callback)
    {
        OnScreamAction?.Invoke(this, EventArgs.Empty);
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