public class PatrollerPatrolState : PatrollerState
{
    public PatrollerPatrolState(Patroller patroller) : base(patroller) { }

    public override void Enter()
    {
        patroller.ResumeMoving();
        patroller.MoveToCurrentWaypoint();
    }

    public override void Tick()
    {
        if (!patroller.HasWaypoints())
            return;

        if (patroller.HasReachedDestination())
        {
            patroller.MoveToNextWaypoint();
        }
    }
}