public class PatrollerInvestigateState : PatrollerState
{
    public PatrollerInvestigateState(Patroller patroller) : base(patroller) { }

    public override void Enter()
    {
        patroller.ResumeMoving();

        if (patroller.AnimatorController != null)
        {
            patroller.AnimatorController.ResetTrigger("Wondering");
            patroller.AnimatorController.SetBool("IsMoving", true);
        }

        if (patroller.IsNoiseBehind())
        {
            patroller.MoveToCurrentWaypoint();
        }
        else
        {
            patroller.Agent.SetDestination(patroller.NoisePosition);
        }
    }

    public override void Tick()
    {
        if (patroller.HasReachedDestination())
        {
            patroller.ChangeState(new PatrollerReturnState(patroller));
        }
    }
}