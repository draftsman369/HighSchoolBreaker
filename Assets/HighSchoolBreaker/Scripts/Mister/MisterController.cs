using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public enum MisterType
{
    staticMister,
    dynamicMister
}

public class MisterController : MonoBehaviour
{
    //[SerializeFied] private MisterType misterType;

    public Animator animator{get; private set;}

    public FieldOfView fieldOfView{get; private set;}

    [Header("AI Navigation")]
    [SerializeField] private List<Transform> waypoinnts = new List<Transform>();
    public NavMeshAgent agent{get; private set;}
    int currentWaypointIndex = 0;

    //Investigation parameters
    private bool hasCaughtPlayer = false;
    private bool CanHearNoise => currentState is MisterPatrolState;
    public Vector3 NoisePosition { get; private set; }
    public bool playerHeard = false;

    //States
    public MisterPatrolState patrolState{get; private set;}
    public MisterMadState madState{get; private set;}
    public MisterWonderState wonderState{get; private set;}
    public MisterInvestigateState investigateState{get; private set;}
    public MisterIdleState idleState{get; private set;}
    MisterState currentState;
    private void Awake()
    {
        animator = this.GetComponent<Animator>();
        agent = this.GetComponent<NavMeshAgent>();
        fieldOfView = this.GetComponent<FieldOfView>();

        patrolState = new MisterPatrolState(this);
        madState = new MisterMadState(this);
        wonderState = new MisterWonderState(this);  
        investigateState = new MisterInvestigateState(this);
        idleState = new MisterIdleState(this);
        currentState = patrolState;
    }

    private void Start()
    {
        ChangeState(patrolState);
    }

    private void Update()
    {
        currentState.Update();
    }


    public void ChangeState(MisterState newState)
    {
        currentState?.Exit();
        currentState = newState;
        currentState?.Enter();
    }

    public void Patrol()
    {
        
        if(agent.remainingDistance < 0.5f)
        {
            currentWaypointIndex = (currentWaypointIndex + 1) % waypoinnts.Count;
            agent.SetDestination(waypoinnts[currentWaypointIndex].position);
        }
    }

    public void HearNoise(Vector3 heardPosition)
    {
        if (hasCaughtPlayer)
            return;

        if (!CanHearNoise)
            return;

        NoisePosition = heardPosition;
        playerHeard = true;
        ChangeState(wonderState);
    }

    public void InvestigateNoise()
    {
        if (NoisePosition != Vector3.zero)
        {
            agent.SetDestination(NoisePosition);
        }
        
        if(agent.remainingDistance < 0.5f)
        {
            playerHeard = false;
            //agent.isStopped = true;
            //ChangeState(idleState);
        }
    }

    public void SetDestination()
    {
        agent.SetDestination(waypoinnts[currentWaypointIndex].position);
    }
}
