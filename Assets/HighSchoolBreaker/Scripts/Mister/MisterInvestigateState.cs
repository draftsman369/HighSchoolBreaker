using UnityEngine;
public class MisterInvestigateState : MisterState
{
    public MisterInvestigateState(MisterController controller) : base(controller){}

    private float timeSpentPatroling;
    private float patrolDuration = 7f;

    public override void Enter()
    {
        controller.agent.isStopped = false;
        controller.agent.SetDestination(controller.NoisePosition);

        Debug.LogWarning("Investigating Noise");
        controller.animator.SetTrigger("Investigating");

        base.Enter();
    }

    public override void Update()
    {
        timeSpentPatroling += Time.deltaTime;

        if(timeSpentPatroling >= patrolDuration)
        {
            controller.playerHeard = false;
            controller.ChangeState(controller.idleState);
            return;
        }

        if(controller.fieldOfView.canSeePlayer)
        {
            controller.ChangeState(controller.madState);
            return;
        }

        if (!controller.agent.pathPending &&
            controller.agent.hasPath &&
            controller.agent.remainingDistance <= controller.agent.stoppingDistance + 0.1f)
        {
            controller.playerHeard = false;
            controller.ChangeState(controller.idleState);
            return;
        }
    }

    public override void Exit()
    {
        controller.animator.ResetTrigger("Investigating");
        timeSpentPatroling = 0f;
        base.Exit();
    }
}