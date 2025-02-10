using System.Linq;
using Godot;

public partial class StateMachine : Node
{
    public State currentState { get; private set; } = null;

    private bool StateMachineInitialized => currentState != null;

	public override void _Ready()
    {
        foreach (State state in GetChildren())
        {
            state.SetStateMachine(this);
        }
        Node initialState = GetChildren().First();
        ChangeToState(initialState.Name);
    }

    public void ChangeToState(string newStateName, object[] data = null)
    {
        GD.Print("StateMachine :: ", currentState == null ? "null" : currentState.Name, " --> ", newStateName, " with data: ", data);
        State state = (State)FindChild(newStateName);
        if (StateMachineInitialized)
        {
            currentState.Exit();
        }
        currentState = state;
        state.Enter(data);
    }
}