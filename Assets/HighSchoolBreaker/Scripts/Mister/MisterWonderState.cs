using UnityEngine;

public class MisterWonderState : MisterState
{
    public MisterWonderState(MisterController controller) : base(controller){}

    float wonderDuration = 2f;
    float wonderTimer;

    public override void Enter()
    {
        controller.agent.isStopped = true;
        controller.agent.ResetPath();
        Debug.LogWarning("Wondering");
        controller.animator.SetTrigger("Wondering");
        AudioManager.Instance.PlaySFX("huhSFX");
        base.Enter();
    }

    public override void Update()
    {
        if(wonderTimer >= wonderDuration)
        {
            controller.ChangeState(controller.investigateState);
            return;
        }
        
        if(controller.fieldOfView.canSeePlayer)
        {
            controller.ChangeState(controller.madState);
            return;
        }
        wonderTimer += Time.deltaTime;


    }

    public override void Exit()
    {
        base.Exit();
        controller.agent.isStopped = false;
        wonderTimer = 0f;
        controller.animator.ResetTrigger("Wondering");
    }
}
