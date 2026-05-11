public class PlayerIdleState : State
{
    public PlayerIdleState(PlayerController player) : base(player) { }

    public override void Enter()
    {
            player.StopMovement();
            //player.UpdateAnimations();
    }

    public override void Tick()
    {
        player.UpdateAnimations();

        if (player.IsGameOver)
            player.ChangeState(new PlayerCaughtState(player));

        if (player.IsMoving && player.IsSneaking)
        {
            player.ChangeState(new PlayerSneakState(player));
        }
        else if (player.IsMoving)
        {
            player.ChangeState(new PlayerMoveState(player));
        }
    }
}