public class PatrollerCaughtState : PatrollerState
{
    public PatrollerCaughtState(Patroller patroller) : base(patroller) { }

    public override void Enter()
    {
        patroller.CatchPlayer();
    }
}