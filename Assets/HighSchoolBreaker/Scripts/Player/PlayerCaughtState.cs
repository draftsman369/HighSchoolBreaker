public class PlayerCaughtState : State
{
    public PlayerCaughtState(PlayerController player) : base(player) { }

    public override void Enter()
    {
        player.SetGameOver();
        player.UpdateAnimations();
        player.PlayGameOverAnimation();
    }
}