using Godot;

public partial class StartScene : Control
{

	private Button StartButton;

	public override void _Ready()
	{
		StartButton = GetNode<Button>("VBoxContainer/StartButton");
		StartButton.ButtonDown += ChangeToStartGameScene;
	}

	private void ChangeToStartGameScene()
	{
		GetTree().ChangeSceneToFile("res://Scenes/CreatePlayers.tscn");
	}
}
