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
	
	public override void _Ready()
	{
		WinnerLabel.Text = PlayerController.Instance.Winner() == null ? "No winner" : PlayerController.Instance.Winner().Name;
		RestartButton.ButtonDown += OnRestartButtonDown;
		ExitButton.ButtonDown += OnExitButtonDown;
		UpdateScoreBoard();
	}

	private void OnRestartButtonDown()
	{
		try
		{
			GetTree().ChangeSceneToFile("res://Scenes/CreatePlayers.tscn");
		}
		catch (Exception e)
		{
			GD.PrintErr(e);
		}
	}

	private void OnExitButtonDown()
	{
		GetTree().Quit();
	}

    private void UpdateScoreBoard()
    {
        ScoreboardLabel.Text = "Scoreboard";
        foreach (Player player in PlayerController.Instance.Players)
        {
            ScoreboardLabel.Text += "\n> " + player.Name + ": " + player.TotalScore.ToString();
        }
    }
}
