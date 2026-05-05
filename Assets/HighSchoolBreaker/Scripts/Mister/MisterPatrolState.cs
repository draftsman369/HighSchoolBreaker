using UnityEngine;

public class MisterPatrolState : MisterState
{
    public MisterPatrolState(MisterController controller) : base(controller)
    {
    }

    public override void Enter()
    {

        //Debug.LogWarning("Patrolling");
        animator.SetTrigger("Patrolling");
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
        animator.ResetTrigger("Patrolling");
    }
    
}
