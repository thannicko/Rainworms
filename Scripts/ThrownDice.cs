using Godot;
using System;

public partial class ThrownDice : TextureRect
{
	[Signal]
	public delegate void DiceClickedEventHandler(ThrownDice dice);

	public int DiceValue { get; private set; }

	public override void _Ready()
	{
	}

	public void Initialize(int diceValue, string textureFile)
	{
		DiceValue = diceValue;
        Texture = GD.Load<Texture2D>(textureFile);
        TextureFilter = CanvasItem.TextureFilterEnum.Nearest;
        ExpandMode = TextureRect.ExpandModeEnum.FitWidthProportional;
	}

	public void EnableClick(bool enabled)
	{
		if (enabled)
		{
			GuiInput += OnGuiInputDice;
		}
		else
		{
            Modulate = new Godot.Color("WEB_GRAY");
		}
	}

    private void OnGuiInputDice(InputEvent @event)
    {
        if (@event.IsPressed() && @event is InputEventMouseButton)
        {
			EmitSignal(SignalName.DiceClicked, this);
        }
    }
}
