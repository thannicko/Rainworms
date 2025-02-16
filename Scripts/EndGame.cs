using Godot;
using System;

public partial class EndGame : Control
{
    [Export]
    public Label WinnerLabel { get; set; }

    [Export]
    public Label ScoreboardLabel { get; set; }
	
    [Export]
    public Button RestartButton { get; set; }
	
    [Export]
    public Button ExitButton { get; set; }
	
	[Export]
	public AudioStreamPlayer ButtonClickEffect { get; set; }
	
	public override void _Ready()
	{
		WinnerLabel.Text = PlayerController.Instance.Winner() == null ? "No winner" : "'" + PlayerController.Instance.Winner().Name + "' won!";
		RestartButton.ButtonDown += OnRestartButtonDown;
		ExitButton.ButtonDown += OnExitButtonDown;
		UpdateScoreBoard();
	}

	private void OnRestartButtonDown()
	{
		ButtonClickEffect.Play();
		ButtonClickEffect.Finished += () => 
			GetTree().ChangeSceneToFile("res://Scenes/CreatePlayers.tscn");
	}

	private void OnExitButtonDown()
	{
		ButtonClickEffect.Play();
		ButtonClickEffect.Finished += () => 
			GetTree().Quit();
	}

    private void UpdateScoreBoard()
    {
        ScoreboardLabel.Text = "Wormboard";
        foreach (Player player in PlayerController.Instance.Players)
        {
            ScoreboardLabel.Text += "\n> " + player.Name + ": " + player.TotalScore.ToString();
        }
    }
}
