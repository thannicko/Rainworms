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
    public Label TotalLabel { get; set; }
    
    [Export]
    public Label PromptLabel { get; set; }
    
    [Export]
    public Label PlayerNameLabel { get; set; }

    private TurnStateMachine turnStateMachine { get => (TurnStateMachine)stateMachine; }

    public override void Enter(object[] data)
    {
        turnStateMachine.SetPlayer(PlayerController.Instance.ActivePlayer);
        turnStateMachine.Reset();

        ThrowDiceButton.Disabled = false;
        ThrowDiceButton.ButtonDown += OnThrowDiceButtonDown;

        PromptLabel.Text = "Throw the dice!";
        PromptLabel.Show();

        PlayerNameLabel.Text = "Player: " + turnStateMachine.Player.Name;
        PlayerNameLabel.Show();

        ThrowDiceButton.Show();
        TotalLabel.Hide();
        ClearContainer(ThrowDiceContainer);
        ClearContainer(KeepDiceContainer);
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
}