using Godot;

public partial class State : Node
{
    protected StateMachine stateMachine;

    public void SetStateMachine(StateMachine stateMachine)
    {
        this.stateMachine = stateMachine;
    }

    public virtual void Enter(object[] data)
    {
    }

    public virtual void Exit()
    {
    }
}