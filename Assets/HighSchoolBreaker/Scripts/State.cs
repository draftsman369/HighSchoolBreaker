public abstract class State
{
    protected PlayerController player;

    protected State(PlayerController player)
    {
        this.player = player;
    }

    public virtual void Enter() { }
    public virtual void Tick() { }
    public virtual void FixedTick() { }
    public virtual void Exit() { }
}