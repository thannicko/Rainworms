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
    public Label PlayerScoreLabel { get; set; }

    [Export]
    public Label ScoreboardLabel { get; set; }

    private TurnStateMachine turnStateMachine { get => (TurnStateMachine)stateMachine; }

    public override void Enter(object[] data)
    {
        turnStateMachine.SetPlayer(PlayerController.Instance.ActivePlayer);
        turnStateMachine.Reset();

        UpdateScoreBoard();

        ThrowDiceButton.Disabled = false;
        ThrowDiceButton.ButtonDown += OnThrowDiceButtonDown;

        PromptLabel.Text = "Throw the dice!";
        PromptLabel.Show();

        PlayerScoreLabel.Text = "Score: " + turnStateMachine.Player.NumberOfWormsBought().ToString();
        PlayerScoreLabel.Show();
        
        PlayerNameLabel.Text = "Player: " + turnStateMachine.Player.Name;
        PlayerNameLabel.Show();

        ThrowDiceButton.Show();
        TotalLabel.Hide();
        
        ClearContainer(ThrowDiceContainer);
        ClearContainer(KeepDiceContainer);
        ClearContainer(BoughtTilesContainer);
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
        ScoreboardLabel.Text = "Scoreboard";
        foreach (Player player in PlayerController.Instance.Players)
        {
            GD.Print("Player: ", player.Name, " + score: ", player.TotalScore.ToString());
            ScoreboardLabel.Text += "\n> " + player.Name + ": " + player.TotalScore.ToString();
        }
    }
}