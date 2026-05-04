using UnityEngine;

public class PatrollerReturnState : PatrollerState
{
    private float timer;

    public PatrollerReturnState(Patroller patroller) : base(patroller) { }

    public override void Enter()
    {
        timer = patroller.InvestigationWaitTime;

        patroller.StopMoving();

        if (patroller.AnimatorController != null)
            patroller.AnimatorController.SetTrigger("LookAround");
    }

    public override void Tick()
    {
        timer -= Time.deltaTime;

        if (timer <= 0f)
        {
            patroller.ChangeState(new PatrollerPatrolState(patroller));
        }
    }
}