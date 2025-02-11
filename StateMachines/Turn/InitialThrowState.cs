using System.Linq;
using Godot;

public partial class InitialThrowState : State
{
    [Export]
    public Button ThrowDiceButton { get; set; }

    [Export]
    public Container ThrowDiceContainer { get; set; }
    
    [Export]
    public Container KeepDiceContainer { get; set; }
    
    [Export]
    public Container BoughtTilesContainer { get; set; }
    
    [Export]
    public Label TotalLabel { get; set; }
    
    [Export]
    public Label PromptLabel { get; set; }
    
    [Export]
    public Label PlayerNameLabel { get; set; }

    [Export]
    public Label ScoreboardLabel { get; set; }

    [Export]
    public DeckController DeckController { get; set; }

    private TurnStateMachine turnStateMachine { get => (TurnStateMachine)stateMachine; }

    public override void Enter(object[] data)
    {
        turnStateMachine.Reset();

        UpdateScoreBoard();

        ThrowDiceButton.Disabled = false;
        ThrowDiceButton.ButtonDown += OnThrowDiceButtonDown;

        PromptLabel.Text = "Throw the dice!";
        PromptLabel.Show();
        
        PlayerNameLabel.Text = "Player: " + PlayerController.Instance.ActivePlayer.Name;
        PlayerNameLabel.Show();

        ThrowDiceButton.Show();
        TotalLabel.Hide();
        
        ClearContainer(ThrowDiceContainer);
        ClearContainer(KeepDiceContainer);

        BoughtTilesContainer.Hide();
        ClearContainer(BoughtTilesContainer);
        DeckController.RenderBoughtTiles(turnStateMachine.Player);
        BoughtTilesContainer.Show();
    }

    public override void Exit()
    {
        ThrowDiceButton.ButtonDown -= OnThrowDiceButtonDown;
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

    private void UpdateScoreBoard()
    {
        ScoreboardLabel.Text = "Wormboard";
        foreach (Player player in PlayerController.Instance.Players)
        {
            ScoreboardLabel.Text += "\n> " + player.Name + ": " + player.TotalScore.ToString();
            ScoreboardLabel.Text += "\n\tLast owned: ";
            if (player.TilesBought.LastOrDefault() != null)
            {
                ScoreboardLabel.Text += player.TilesBought.Last().Cost;
            }
            else
            {
                ScoreboardLabel.Text += "nothing";
            }
        }
    }
}