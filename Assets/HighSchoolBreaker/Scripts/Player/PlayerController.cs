using System;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

    public static PlayerController Instance { get; private set; }

    [Header("References")]
    [SerializeField] private Rigidbody playerRigidbody;
    [SerializeField] private Animator playerAnimator;
    private Collider playerCollider;

    [Header("Movement")]
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float sneakSpeed = 2f;
    [SerializeField] private float turnSpeed = 10f;

    [Header("Screaming parameters")]
    [SerializeField] private float noiseRadius = 5f;
    [SerializeField] private LayerMask patrollerMask;

    private Vector2 moveInput;
    private Vector3 moveDirection;

    private State currentState;

    public bool IsGameOver { get; private set; }
    public bool IsSneaking => InputReader.Instance.IsSneaking;
    public bool IsMoving => moveInput.sqrMagnitude > 0.01f;


    public bool IsHidden { get; private set; }

    [SerializeField] private GameObject playerVisual; // mesh or model
    private Locker currentLocker;

    public void EnterLocker(Locker locker, Transform hidePoint)
    {
        currentLocker = locker;

        IsHidden = true;

        playerVisual.SetActive(false);
        playerCollider.enabled = false;
        enabled = false;

        transform.position = hidePoint.position;
        transform.forward = locker.gameObject.transform.forward;
    }

    public void TryExitLocker()
    {
        if (currentLocker != null)
        {
            currentLocker.ExitLocker();
        }
    }

    public void ExitLocker()
    {
        IsHidden = false;
        playerCollider.enabled = true;

        playerVisual.SetActive(true);
        enabled = true;

        currentLocker = null;
    }

    private void Awake()
    {
        if(Instance != null)
        {
            Destroy(this.gameObject);
        }
        Instance = this;

        if (playerRigidbody == null)
            playerRigidbody = GetComponent<Rigidbody>();

        if (playerAnimator == null)
            playerAnimator = GetComponent<Animator>();

        if (playerCollider == null)
            playerCollider = GetComponent<Collider>();
    }

    private void Start()
    {
        ChangeState(new PlayerIdleState(this));

        InputReader.Instance.OnScreamAction += OnScreamAction;
    }

    private void OnEnable()
    {
        InputReader.Instance.OnScreamAction += OnScreamAction;
    }

    private void OnDisable()
    {
        InputReader.Instance.OnScreamAction -= OnScreamAction;
    }

    public void OnScreamAction(object sender, EventArgs e)
    {
        ChangeState(new PlayerScaringState(this));
    }


    private void Update()
    {
        moveInput = InputReader.Instance.MoveInput;

        currentState?.Tick();
    }

    private void FixedUpdate()
    {
        currentState?.FixedTick();
    }

    public void ChangeState(State newState)
    {
        currentState?.Exit();

        currentState = newState;
        currentState.Enter();
    }

    public void Move()
    {
        moveDirection.Set(moveInput.x, 0f, moveInput.y);

        float currentSpeed = IsSneaking ? sneakSpeed : moveSpeed;

        Vector3 velocity = moveDirection.normalized * currentSpeed * Time.fixedDeltaTime;

        playerRigidbody.MovePosition(playerRigidbody.position + velocity);
    }

    public void Rotate()
    {
        if (!IsMoving)
            return;

        Vector3 desiredDirection = Vector3.RotateTowards(
            transform.forward,
            moveDirection,
            turnSpeed * Time.fixedDeltaTime,
            0f
        );

        Quaternion targetRotation = Quaternion.LookRotation(desiredDirection);

        playerRigidbody.MoveRotation(targetRotation);
    }

    public void UpdateAnimations()
    {
        playerAnimator.SetBool("IsMoving", IsMoving);
        playerAnimator.SetBool("IsSneaking", IsSneaking);
    }

    public void PlayGameOverAnimation()
    {
        playerAnimator.SetTrigger("GameOver");
    }

    public void SetGameOver()
    {
        IsGameOver = true;
        LevelLoader.Instance.ReloadLevel();
        Debug.LogWarning("Game Over!");
    }

    public void Boo()
    {
        playerAnimator.SetTrigger("Boo");


        Collider[] hits = Physics.OverlapSphere(
            transform.position,
            noiseRadius,
            patrollerMask
        );

        foreach (Collider hit in hits)
        {
            MisterController mister = hit.GetComponentInParent<MisterController>();

            if (mister != null)
            {
                mister.HearNoise(transform.position);
            }
        }
    }

    public void ResetbooTrigger()
    {
        playerAnimator.ResetTrigger("Boo");
    }


}