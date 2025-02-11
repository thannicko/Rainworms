using Godot;
using System;

public partial class CreatePlayers : Control
{
	[Export]
	public Label PlayersInGameLabel { get; set; }

	[Export]
	public LineEdit NameEntry { get; set; }
	
	[Export]
	public Button AddButton {get; set;}
	
	[Export]
	public Button StartButton {get; set;}

	public override void _Ready()
	{
		AddButton.ButtonDown += OnAddButtonDown;
		StartButton.ButtonDown += OnStartButtonDown;
		PlayersInGameLabel.Hide();
	}

	private void OnAddButtonDown()
	{
		PlayerController.Instance.AddPlayer(NameEntry.Text);
		NameEntry.Text = "";
		UpdatePlayersInGame();
	}

	private void OnStartButtonDown()
	{
		GetTree().ChangeSceneToFile("res://Scenes/Game.tscn");
	}
	
	private void UpdatePlayersInGame()
	{
		PlayersInGameLabel.Text = "Players in game";
		foreach (Player player in PlayerController.Instance.Players)
		{
			PlayersInGameLabel.Text += "\n> " + player.Name;
		}
		PlayersInGameLabel.Show();
	}
}
