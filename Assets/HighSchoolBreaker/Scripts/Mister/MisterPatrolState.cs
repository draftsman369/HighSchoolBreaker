using UnityEngine;
using UnityEngine.AI;

public class MisterPatrolState : MisterState
{
    public MisterPatrolState(MisterController controller) : base(controller)
    {
    }

    public override void Enter()
    {

        //Debug.LogWarning("Patrolling");
        controller.agent.isStopped = false;
        controller.SetDestination();
        controller.animator.SetTrigger("Patrolling");
    }

    public override void Update()
    {
        if(controller.fieldOfView.canSeePlayer)
        {
            controller.ChangeState(controller.madState);
            return;
        }

        if(controller.playerHeard)
        {
            controller.ChangeState(controller.wonderState);
            return;
        }

        controller.Patrol();

    }

    public override void Exit()
    {
        //Debug.LogWarning("Exiting Patrol State");
        controller.agent.isStopped = true;
        controller.animator.ResetTrigger("Patrolling");
    }
    
}
