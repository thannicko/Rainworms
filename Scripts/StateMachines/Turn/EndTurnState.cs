using Godot;

public partial class EndTurnState : State
{
    [Export]
    public Label PromptLabel { get; set; }

    [Export]
    public DeckController DeckController { get; set; }

	[Export]
	public PackedScene EndGameScene { get; set; }

    [Export]
    public Button ThrowDiceButton { get; set; }

    public override async void Enter(object[] data)
    {
        ThrowDiceButton.Disabled = true;
        var lastPlayer = PlayerController.Instance.ActivePlayer;
        PlayerController.Instance.ActivePlayerFinished();

        if (IsGameFinished())
        {
            PromptLabel.Text = "The game has reached an end!";
            await ToSignal(GetTree().CreateTimer(1.0), Timer.SignalName.Timeout);
            GetTree().ChangeSceneToPacked(EndGameScene);
        }
        else
        {
            PromptLabel.Text = "Turn finished for " + lastPlayer.Name;
            await ToSignal(GetTree().CreateTimer(1.0), Timer.SignalName.Timeout);

            PromptLabel.Text = "*** Next turn: " + PlayerController.Instance.ActivePlayer.Name + " ***";
            await ToSignal(GetTree().CreateTimer(1.0), Timer.SignalName.Timeout);
            stateMachine.ChangeToState("InitialThrowState");
        }
    }

    private bool IsGameFinished()
    {
        return DeckController.NumberOfTilesLeft == 0;
    }

    public override void Exit()
    {

    }
}