using System;
using Unity.VisualScripting;
using UnityEngine;
using Unity.Cinemachine;

public class PlayerController : MonoBehaviour
{

    public static PlayerController Instance { get; private set; }

    [Header("References")]
    [SerializeField] private Rigidbody playerRigidbody;
    [SerializeField] private Animator playerAnimator;
    [SerializeField] private FootStepPlayer footStepPlayer;
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


    [Header("Remote Car")]
    [SerializeField] private RemoteCarController remoteCar;

    [Header("Cameras")]
    [SerializeField] private CinemachineCamera playerCamera;
    [SerializeField] private CinemachineCamera carCamera;
    [SerializeField] private CinemachineCamera winCamera;


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
        InputReader.Instance.OnScreamAction += OnScreamAction;
        InputReader.Instance.OnControlCarAction += OnControlCarAction;
        footStepPlayer = this.GetComponent<FootStepPlayer>();
        ChangeState(new PlayerIdleState(this));
        SwitchToPlayerCamera();
    }

    private void OnEnable()
    {
        InputReader.Instance.OnScreamAction += OnScreamAction;
        InputReader.Instance.OnControlCarAction += OnControlCarAction;
    }

    private void OnDisable()
    {
        InputReader.Instance.OnScreamAction -= OnScreamAction;
        InputReader.Instance.OnControlCarAction -= OnControlCarAction;
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
        if(GameManager.Instance.GameWon || IsGameOver)
        {
            return;
        }
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
        float footStepInterval = IsSneaking ? .5f : .3f;
        playerRigidbody.MovePosition(playerRigidbody.position + velocity);

        if(IsSneaking)
        {
            footStepPlayer.PlayFootStep(.5f, 1.14f, .02f);
        }
        else
        {
            footStepPlayer.PlayFootStep(.3f, 1.12f, .3f);
        }
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
        bool shouldMove = moveDirection.sqrMagnitude > 0.01f;
        playerAnimator.SetBool("IsMoving", shouldMove);
        playerAnimator.SetBool("IsSneaking", IsSneaking);
    }

    public void PlayGameOverAnimation()
    {
        playerAnimator.SetTrigger("GameOver");
    }

    public void PlayGameWonAnimation()
    {
        playerAnimator.SetTrigger("GameWon");
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

    public void SetRemoteCarTrigger()
    {
        playerAnimator.SetTrigger("RemoteCar");
    }

    public void ResetRemoteCarTrigger()
    {
        playerAnimator.ResetTrigger("RemoteCar");
    }

    public void RemoveMovement()
    {
        playerRigidbody.linearVelocity = Vector3.zero;
        playerRigidbody.angularVelocity = Vector3.zero;
        moveDirection = Vector3.zero;

    }

    //Car control
    private void OnControlCarAction(object sender, EventArgs e)
    {

        if (currentState is PlayerCarControlState)
        {
            ChangeState(new PlayerIdleState(this));
            return;
        }

        if (remoteCar == null)
            return;

        if (!remoteCar.IsDeployed)
        {
            remoteCar.Deploy();
        }

        if (!remoteCar.HasSignal || !remoteCar.HasEnergy)
            return;

        
        ChangeState(new PlayerCarControlState(this, remoteCar));
    }

    public void SwitchToCarCamera()
    {
        playerCamera.Priority = 0;
        carCamera.Priority = 20;
    }

    public void SwitchToPlayerCamera()
    {
        playerCamera.Priority = 20;
        carCamera.Priority = 0;
    }

    public void SwitchToWinCamera()
    {
        winCamera.Priority = 20;
        playerCamera.Priority = 0;
        carCamera.Priority = 0;
    }

    public void StopMovement()
    {
        playerRigidbody.linearVelocity = Vector3.zero; // Unity 6
        playerRigidbody.angularVelocity = Vector3.zero;
        moveDirection = Vector3.zero;
    }

}