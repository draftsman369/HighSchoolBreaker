using UnityEngine;

public abstract class MisterState 
{
    protected MisterController controller;
    protected Animator animator;

    public MisterState(MisterController controller)
    {
        this.controller = controller;
        this.animator = controller.animator;
    }

    public virtual void Enter(){}
    public virtual void Update(){}
    public virtual void Exit(){}
}
