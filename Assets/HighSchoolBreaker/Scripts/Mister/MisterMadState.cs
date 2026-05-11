using UnityEngine;

public class MisterMadState : MisterState
{
    public MisterMadState(MisterController controller) : base(controller){}

    public override void Enter()
    {
        controller.agent.isStopped = true;
        controller.agent.ResetPath();
        PlayerController.Instance.SetGameOver();
        controller.animator.SetTrigger("Mad");
        AudioManager.Instance.PlaySFX("caugthSFX");
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
