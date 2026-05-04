using UnityEngine;

public class PatrollerWonderState : PatrollerState
{
    private float timer;

    public PatrollerWonderState(Patroller patroller) : base(patroller) { }

    public override void Enter()
    {
        timer = patroller.WaitBeforeInvestigating;

        patroller.StopMoving();

        if (patroller.AnimatorController != null)
            patroller.AnimatorController.SetTrigger("Wondering");

        patroller.ShowExclamationUI();
    }

    public override void Tick()
    {
        timer -= Time.deltaTime;

        if (timer <= 0f)
        {
            patroller.ChangeState(new PatrollerInvestigateState(patroller));
        }
    }

    public override void Exit()
    {
        patroller.HideExclamationUI();
    }
}