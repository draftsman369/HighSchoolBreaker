using UnityEngine;

public class PlayerScaringState : State
{
    public PlayerScaringState(PlayerController player) : base(player){}

    public float booDuration = 1f;
    public float booTimer = 0f;

    public override void Enter()
    {
        base.Enter();
        Debug.LogWarning("in boo state");
        player.Boo();
    }

    public override void Tick()
    {
        booTimer += Time.deltaTime;

        if(booTimer >= booDuration)
        {
            booTimer = 0f;
            player.ChangeState(new PlayerIdleState(player));    
        }
    }

    public override void Exit()
    {
        player.ResetbooTrigger();
        base.Exit();
    }
}
