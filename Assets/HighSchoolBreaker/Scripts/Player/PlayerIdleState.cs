public class PlayerIdleState : State
{
    public PlayerIdleState(PlayerController player) : base(player) { }

    public override void Tick()
    {
        player.UpdateAnimations();

        if (player.IsGameOver)
            return;

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