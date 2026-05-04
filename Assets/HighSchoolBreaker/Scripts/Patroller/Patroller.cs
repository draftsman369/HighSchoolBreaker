using UnityEngine;
using UnityEngine.AI;
using System.Collections.Generic;

public class Patroller : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private NavMeshAgent agent;
    [SerializeField] private Animator animatorController;
    [SerializeField] private Transform player;
    [SerializeField] private Transform eyePoint;

    [Header("Patrol")]
    [SerializeField] private List<Transform> waypoints = new List<Transform>();
    [SerializeField] private float waypointReachDistance = 1f;

    [Header("Vision")]
    [SerializeField] private float viewRange = 8f;
    [SerializeField] private float viewAngle = 60f;
    [SerializeField] private LayerMask playerMask;
    [SerializeField] private LayerMask obstacleMask;

    [Header("Investigation")]
    [SerializeField] private float waitBeforeInvestigating = 1.5f;
    [SerializeField] private float investigationWaitTime = 2f;

    [Header("UI")]
    [SerializeField] private GameObject exclamationUI;

    private PatrollerState currentState;
    private int currentWaypointIndex;
    private Vector3 noisePosition;
    private bool hasCaughtPlayer;


    // ---- GETTERS ----
    public NavMeshAgent Agent => agent;
    public Animator AnimatorController => animatorController;
    public Transform Player => player;
    public Transform EyePoint => eyePoint != null ? eyePoint : transform;

    public float WaitBeforeInvestigating => waitBeforeInvestigating;
    public float InvestigationWaitTime => investigationWaitTime;
    public Vector3 NoisePosition => noisePosition;
    public bool HasCaughtPlayer => hasCaughtPlayer;

    public bool CanHearNoise => currentState is PatrollerPatrolState;

    // ---- UNITY ----

    private void Awake()
    {
        if (agent == null)
            agent = GetComponent<NavMeshAgent>();

        if (animatorController == null)
            animatorController = GetComponent<Animator>();

        if (exclamationUI != null)
            exclamationUI.SetActive(false);
    }

    private void Start()
    {
        ChangeState(new PatrollerPatrolState(this));
    }

    private void Update()
    {
        if (hasCaughtPlayer)
            return;

        DetectPlayer();

        currentState?.Tick();

        UpdateAnimations();
    }

    // ---- STATE MACHINE ----

    public void ChangeState(PatrollerState newState)
    {
        currentState?.Exit();

        currentState = newState;
        currentState.Enter();
    }

    // ---- PATROL ----

    public bool HasWaypoints()
    {
        return waypoints != null && waypoints.Count > 0;
    }

    public Vector3 GetCurrentWaypointPosition()
    {
        return waypoints[currentWaypointIndex].position;
    }

    public void MoveToCurrentWaypoint()
    {
        if (!HasWaypoints())
            return;

        agent.isStopped = false;
        agent.SetDestination(GetCurrentWaypointPosition());
    }

    public void MoveToNextWaypoint()
    {
        if (!HasWaypoints())
            return;

        currentWaypointIndex = (currentWaypointIndex + 1) % waypoints.Count;
        MoveToCurrentWaypoint();
    }

    public bool HasReachedDestination()
    {
        return !agent.pathPending && agent.remainingDistance <= waypointReachDistance;
    }

    // ---- NOISE ----

    public void HearNoise(Vector3 heardPosition)
    {
        if (hasCaughtPlayer)
            return;

        if (!CanHearNoise)
            return;

        noisePosition = heardPosition;

        ChangeState(new PatrollerWonderState(this));
    }

    // ---- VISION ----

    private void DetectPlayer()
    {
        if (player == null)
            return;

        Vector3 dir = player.position - EyePoint.position;
        float distance = dir.magnitude;

        if (distance > viewRange)
            return;

        float angle = Vector3.Angle(EyePoint.forward, dir);

        if (angle > viewAngle * 0.5f)
            return;

        if (Physics.Raycast(
            EyePoint.position,
            dir.normalized,
            out RaycastHit hit,
            viewRange,
            playerMask | obstacleMask))
        {
            bool hitPlayer = ((1 << hit.collider.gameObject.layer) & playerMask) != 0;

            if (hitPlayer)
            {
                ChangeState(new PatrollerCaughtState(this));
            }
        }
    }

    // ---- LOGIC HELPERS ----

    public void StopMoving()
    {
        agent.isStopped = true;
    }

    public void ResumeMoving()
    {
        agent.isStopped = false;
    }

    public void LookAtPosition(Vector3 position)
    {
        Vector3 dir = position - transform.position;
        dir.y = 0f;

        if (dir.sqrMagnitude > 0.01f)
            transform.forward = dir.normalized;
    }

    public bool IsNoiseBehind()
    {
        Vector3 dir = noisePosition - transform.position;
        dir.y = 0f;

        float dot = Vector3.Dot(transform.forward, dir.normalized);
        return dot < 0f;
    }

    // ---- UI ----


    public void ShowExclamationUI()
    {
        if (exclamationUI != null)
            exclamationUI.SetActive(true);
    }

    public void HideExclamationUI()
    {
        if (exclamationUI != null)
            exclamationUI.SetActive(false);
    }

    // ---- GAME OVER ----

    public void CatchPlayer()
    {
        if (hasCaughtPlayer)
            return;

        hasCaughtPlayer = true;

        StopMoving();

        if (animatorController != null)
            animatorController.SetTrigger("Disapointed");

        // Tell player
        PlayerController pc = player.GetComponent<PlayerController>();
        if (pc != null)
        {
            pc.ChangeState(new PlayerCaughtState(pc));
        }

        LevelLoader.Instance.ReloadLevel();
    }

    // ---- DEBUG ----

    private void OnDrawGizmosSelected()
    {
        Transform origin = eyePoint != null ? eyePoint : transform;

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(origin.position, viewRange);

        Vector3 left = Quaternion.Euler(0, -viewAngle * 0.5f, 0) * origin.forward;
        Vector3 right = Quaternion.Euler(0, viewAngle * 0.5f, 0) * origin.forward;

        Gizmos.color = Color.yellow;
        Gizmos.DrawRay(origin.position, left * viewRange);
        Gizmos.DrawRay(origin.position, right * viewRange);
    }

    private void UpdateAnimations()
    {
        if (animatorController == null || agent == null)
            return;

        animatorController.SetBool("IsMoving", agent.velocity.magnitude > 0.1f);
    }
}