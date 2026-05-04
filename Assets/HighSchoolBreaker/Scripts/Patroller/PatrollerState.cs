public abstract class PatrollerState
{
    protected Patroller patroller;

    protected PatrollerState(Patroller patroller)
    {
        this.patroller = patroller;
    }

    public virtual void Enter() { }
    public virtual void Tick() { }
    public virtual void Exit() { }
}