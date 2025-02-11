using Godot;

public partial class WaitRethrowState : State
{
    [Export]
    public Label PromptLabel { get; set; }
    
    [Export]
    public Button ThrowDiceButton { get; set; }
    
    [Export]
    public Container ThrowDiceContainer { get; set; }

    [Export]
    public DeckController DeckController { get; set; }
    
    [Export]
    public Label PlayerScoreLabel { get; set; }

    private TurnStateMachine turnStateMachine { get => (TurnStateMachine)stateMachine; }

    public override void Enter(object[] data)
    {
        PromptLabel.Text = "Throw the dice or click on a tile to buy";
        ClearContainer(ThrowDiceContainer);

        if (turnStateMachine.NrDicesLeft > 0)
        {
            ThrowDiceButton.Disabled = false;
            ThrowDiceButton.ButtonDown += OnThrowDiceButtonDown;
        }

        DeckController.IsBuyingEnabled = true;
        DeckController.TileBought += OnTileBought;
    }

    public override void Exit()
    {
        DeckController.IsBuyingEnabled = false;
        ThrowDiceButton.ButtonDown -= OnThrowDiceButtonDown;
        DeckController.TileBought -= OnTileBought;
    }

    private void OnTileBought(WormTile tile)
    {
        turnStateMachine.BuyTile(tile);
        PlayerScoreLabel.Text = "Score: " + turnStateMachine.Player.NumberOfWormsBought().ToString();
        turnStateMachine.ChangeToState("InitialThrowState");
        PromptLabel.Text = "Successfully bought: " + tile.BoughtInfo();
    }

    private void OnThrowDiceButtonDown()
    {
        turnStateMachine.ChangeToState("ThrowingState");
    }

    private void ClearContainer(Container container)
    {
        foreach (var child in container.GetChildren())
        {
            child.QueueFree();
        }
    }
}