using UnityEngine;

public class MisterInvestigateState : MisterState
{
    public MisterInvestigateState(MisterController controller) : base(controller){}

    public override void Enter()
    {
        Debug.LogWarning("Investigating Noise");
        base.Enter();
    }

    public override void Update()
    {
        if(controller.fieldOfView.canSeePlayer)
        {
            controller.ChangeState(controller.madState);
            return;
        }

        controller.InvestigateNoise();
    }

    public override void Exit()
    {
        base.Exit();
    }
}
