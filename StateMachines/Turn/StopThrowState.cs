using Godot;

public partial class StopThrowState : State
{
    [Export]
    public Label PromptLabel { get; set; }
    
    [Export]
    public Button ThrowDiceButton { get; set; }

    [Export]
    public DeckController DeckController { get; set; }

    private TurnStateMachine turnStateMachine { get => (TurnStateMachine)stateMachine; }

    public override async void Enter(object[] data)
    {
        ThrowDiceButton.Disabled = true;

        if (turnStateMachine.HasNoWorms())
        {
            turnStateMachine.InvalidateThrow(TurnStateMachine.InvalidThrowType.NO_wORM);
        }
        if (DeckController.HasNothingToBuy())
        {
            turnStateMachine.InvalidateThrow(TurnStateMachine.InvalidThrowType.NO_TILES);
        }
        if (!turnStateMachine.IsValidThrow)
        {
            PromptLabel.Text = "Invalid throw: " + turnStateMachine.InvalidThrowReason();
            PromptLabel.Show();
            TakeInvalidAction();
            await ToSignal(GetTree().CreateTimer(1.0), Timer.SignalName.Timeout);
        }
    }

    public override void Exit()
    {
    }

    private async void TakeInvalidAction()
    {
        PromptLabel.Text = "Disabling the next highest tile...";
        await ToSignal(GetTree().CreateTimer(1.0), Timer.SignalName.Timeout);
        DeckController.DisableNextHighestTile();

        PromptLabel.Text = "Returning player's top tile to the deck...";
        await ToSignal(GetTree().CreateTimer(1.0), Timer.SignalName.Timeout);
        DeckController.ReturnLastBoughtTilesToDeck(turnStateMachine.Player);
        
        await ToSignal(GetTree().CreateTimer(1.0), Timer.SignalName.Timeout);
        turnStateMachine.ChangeToState("EndTurnState");
    }
}