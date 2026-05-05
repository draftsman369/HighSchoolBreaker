using UnityEngine;

public class MisterMadState : MisterState
{
    public MisterMadState(MisterController controller) : base(controller){}

    public override void Enter()
    {
        controller.agent.isStopped = true;
        PlayerController.Instance.SetGameOver();
        animator.SetTrigger("Mad");
    }

    public override void Update()
    {
        base.Update();
    }

    public override void Exit()
    {
        base.Exit();
    }

}
