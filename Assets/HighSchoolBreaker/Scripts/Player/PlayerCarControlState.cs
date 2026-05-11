public class PlayerCarControlState : State
{
    private RemoteCarController remoteCar;

    public PlayerCarControlState(PlayerController player, RemoteCarController remoteCar) : base(player)
    {
        this.remoteCar = remoteCar;
    }

    public override void Enter()
    {
        if(!remoteCar.CanBeDeployed)
        {
            player.ChangeState(new PlayerIdleState(player));
            return;
        }
        remoteCar.StartControl();
        player.SwitchToCarCamera();
        player.SetRemoteCarTrigger();
        player.RemoveMovement();
        player.UpdateAnimations();
    }

    public override void Tick()
    {
        if (!remoteCar.HasSignal || !remoteCar.HasEnergy)
        {
            player.ChangeState(new PlayerIdleState(player));
        }
    }

    public override void FixedTick()
    {
        remoteCar.Move(InputReader.Instance.MoveInput);
    }

    public override void Exit()
    {
        player.ResetRemoteCarTrigger();
        remoteCar.StopControl();
        player.SwitchToPlayerCamera();
    }
}