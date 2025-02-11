using Godot;

public partial class EndTurnState : State
{
    [Export]
    public Label PromptLabel { get; set; }

    public override async void Enter(object[] data)
    {
        PlayerController.Instance.ActivePlayerFinished();
        
        PromptLabel.Text = "Turn finished for " + PlayerController.Instance.ActivePlayer.Name;
        await ToSignal(GetTree().CreateTimer(1.0), Timer.SignalName.Timeout);

        PromptLabel.Text = "*** Next turn: " + PlayerController.Instance.ActivePlayer.Name + " ***";
        await ToSignal(GetTree().CreateTimer(1.0), Timer.SignalName.Timeout);
        stateMachine.ChangeToState("InitialThrowState");
    }

    public override void Exit()
    {

    }
}