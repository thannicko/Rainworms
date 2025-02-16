using System.Collections.Generic;
using System.Linq;
using Godot;

public partial class WaitRethrowState : State
{
    [Export]
    public Label PromptLabel { get; set; }
    
    [Export]
    public Button ThrowDiceButton { get; set; }
    
    [Export]
    public Button StealButton { get; set; }
    
    [Export]
    public Container ThrowDiceContainer { get; set; }
    
    [Export]
    public Container StealTargetsContainer { get; set; }

    [Export]
    public DeckController DeckController { get; set; }
	
	[Export]
	public AudioStreamPlayer ButtonClickEffect { get; set; }

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

        CheckAndRenderStealCondition();
        StealButton.ButtonDown += OnStealButtonDown;

        DeckController.IsBuyingEnabled = true;
        DeckController.TileBought += OnTileBought;
    }

    public override void Exit()
    {
        DeckController.IsBuyingEnabled = false;
        ThrowDiceButton.ButtonDown -= OnThrowDiceButtonDown;
        DeckController.TileBought -= OnTileBought;
        StealButton.ButtonDown -= OnStealButtonDown;
        StealButton.Hide();
        StealTargetsContainer.Hide();
    }

    private void CheckAndRenderStealCondition()
    {
        if (IsStealPossible())
        {
            ClearContainer(StealTargetsContainer);
            foreach (Player player in possibleSteals)
            {
                var button = new Button();
                button.Text = player.Name;
                button.ButtonDown += () => StealFrom(player);
                StealTargetsContainer.AddChild(button);
            }
            StealButton.Show();
        }
        else
        {
            StealButton.Hide();
            StealTargetsContainer.Hide();
        }
    }

    private void OnStealButtonDown()
    {
        ButtonClickEffect.Play();
        StealTargetsContainer.Show();
    }

    private void StealFrom(Player target)
    {
        ButtonClickEffect.Play();
        WormTile tile = target.PopLastTile();
        PromptLabel.Text = PlayerController.Instance.ActivePlayer.Name
            + " stole tile '" + tile.Cost + "' from "
            + target.Name;
        GD.Print("WaitRethrowState :: " + PromptLabel.Text);
        turnStateMachine.BuyTile(tile);
        DeckController.AddToBoughtTile(tile);
        turnStateMachine.ChangeToState("EndTurnState");
    }

    private bool IsStealPossible()
    {
        possibleSteals.Clear();
        possibleSteals = PlayerController.Instance.Players.Where(
            player => player != turnStateMachine.Player &&
                    player.TilesBought.LastOrDefault() != null &&
                    player.TilesBought.Last().Cost == turnStateMachine.PointsEarnedInTurn
        ).ToList();
        return possibleSteals.Count > 0 && !turnStateMachine.HasNoWorms();
    }

    private void OnTileBought(WormTile tile)
    {
        turnStateMachine.BuyTile(tile);
        turnStateMachine.ChangeToState("EndTurnState");
        PromptLabel.Text = "Successfully bought: " + tile.BoughtInfo();
    }

    private void OnThrowDiceButtonDown()
    {
        ButtonClickEffect.Play();
        turnStateMachine.ChangeToState("ThrowingState");
    }

    private void ClearContainer(Container container)
    {
        foreach (var child in container.GetChildren())
        {
            child.QueueFree();
        }
    }

    private List<Player> possibleSteals = new List<Player>();
}