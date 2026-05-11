public abstract class State
{
    protected PlayerController player;

    protected State(PlayerController player)
    {
        this.player = player;
    }

    public virtual void Enter()
    {
        if(GameManager.Instance.GameWon || player.IsGameOver)
        {
            return;
        }
    }
    public virtual void Tick()
    {
        if(GameManager.Instance.GameWon || player.IsGameOver)
        {
            return;
        }
    }
    public virtual void FixedTick()
    {
        if(GameManager.Instance.GameWon || player.IsGameOver)
        {
            return;
        }
    }
    public virtual void Exit()
    {
        if(GameManager.Instance.GameWon || player.IsGameOver)
        {
            return;
        }
    }
}