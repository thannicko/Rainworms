using Godot;

public partial class StartScene : Control
{
	[Export]
	public PackedScene StartGameScene { get; set; }

	private Button StartButton;

	public override void _Ready()
	{
		StartButton = GetNode<Button>("VBoxContainer/StartButton");
		StartButton.ButtonDown += ChangeToStartGameScene;
	}

	private void ChangeToStartGameScene()
	{
		GetTree().ChangeSceneToPacked(StartGameScene);
	}
}
