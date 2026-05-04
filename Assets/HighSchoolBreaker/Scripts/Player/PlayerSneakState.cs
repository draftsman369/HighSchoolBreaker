public class PlayerSneakState : State
{
    public PlayerSneakState(PlayerController player) : base(player) { }

    public override void Tick()
    {
        player.UpdateAnimations();

        if (player.IsGameOver)
            return;

        if (!player.IsMoving)
        {
            player.ChangeState(new PlayerIdleState(player));
        }
        else if (!player.IsSneaking)
        {
            player.ChangeState(new PlayerMoveState(player));
        }
    }

    public override void FixedTick()
    {
        player.Move();
        player.Rotate();
    }
}