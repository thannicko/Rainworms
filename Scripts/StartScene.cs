using Godot;

public partial class StartScene : Control
{
	public override void _Ready()
	{
		StartButton = GetNode<Button>("VBoxContainer/StartButton");
		ButtonClickEffect = GetNode<AudioStreamPlayer>("ButtonClickEffect");
		StartButton.ButtonDown += ChangeToStartGameScene;
	}

	private void ChangeToStartGameScene()
	{
		ButtonClickEffect.Play();
		ButtonClickEffect.Finished += () => 
			GetTree().ChangeSceneToFile("res://Scenes/CreatePlayers.tscn");
		
	}

	private Button StartButton;
	private AudioStreamPlayer ButtonClickEffect;
}
