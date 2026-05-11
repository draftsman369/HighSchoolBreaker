using UnityEngine;

public class MisterIdleState : MisterState
{
    public MisterIdleState(MisterController controller) : base(controller){}

    public float idleDuration;
    public float idleTimer;

    public override void Enter()
    {
        //base.Enter();
        controller.agent.isStopped = true;
        controller.animator.SetTrigger("Idle");
        idleDuration = Random.Range(2f, 5f);
        idleTimer = 0f;
    }
    

    public override void Update()
    {
        base.Update();
        if (idleTimer >= idleDuration)
        {
            controller.ChangeState(controller.patrolState);
        }

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
        
        idleTimer += Time.deltaTime;

    }

    public override void Exit()
    {
        controller.animator.ResetTrigger("Idle");
        controller.agent.isStopped = false;
        base.Exit();
    }

}
