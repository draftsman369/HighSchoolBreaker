public class PlayerMoveState : State
{
    public PlayerMoveState(PlayerController player) : base(player) { }

    public override void Tick()
    {
        player.UpdateAnimations();

        if (player.IsGameOver)
        {
            player.ChangeState(new PlayerCaughtState(player));
        }

        if (!player.IsMoving)
        {
            player.ChangeState(new PlayerIdleState(player));
        }
        else if (player.IsSneaking)
        {
            player.ChangeState(new PlayerSneakState(player));
        }
    }

    public override void FixedTick()
    {
        player.Move();
        player.Rotate();
    }
}