using System.Collections.Generic;
using System.Linq;
using Godot;

public partial class KeepDiceState : State
{
    [Export]
    public Container ThrowDiceContainer { get; set; }
    
    [Export]
    public Container KeepDiceContainer { get; set; }

    private TurnStateMachine turnStateMachine { get => (TurnStateMachine)stateMachine; }

    public override void Enter(object[] data)
    {
        var inputs = data[0] as Dictionary<string, object>;
        keepDice = (int)inputs["dice_value"];
        diceTexturesToValue = (Dictionary<TextureRect, int>)inputs["dice_textures"];
        MoveTexturesToKeepContainer();
        turnStateMachine.keptDices[keepDice] = turnStateMachine.diceFrequencies[keepDice];
        turnStateMachine.NrDicesLeft -= turnStateMachine.keptDices[keepDice];
        turnStateMachine.SetPointsEarned(GetPointsEarned(turnStateMachine.keptDices));
        if (turnStateMachine.NrDicesLeft <= 0)
        {
            turnStateMachine.ChangeToState("StopThrowState");
        }
        else
        {
            turnStateMachine.ChangeToState("WaitRethrowState");
        }
    }

    public override void Exit()
    {
    }

    private int GetPointsEarned(Dictionary<int, int> diceToNrOfThrows)
    {
        int sum = 0;
        foreach (int dice in diceToNrOfThrows.Keys)
            sum += turnStateMachine.diceToPoints[dice] * diceToNrOfThrows[dice];
        return sum;
    }

    private void MoveTexturesToKeepContainer()
    {
        var texturesToRemove = new Godot.Collections.Array<Node>();
        foreach (TextureRect texture in diceTexturesToValue.Keys)
        {
            if (diceTexturesToValue[texture] == keepDice)
            {
                texturesToRemove.Add(texture);
                KeepDiceContainer.AddChild(texture.Duplicate());
            }
        }
        foreach (Node texture in texturesToRemove)
        {
            texture.QueueFree();
        }
    }

    private int keepDice = 0;
    private Dictionary<TextureRect, int> diceTexturesToValue = new Dictionary<TextureRect, int>();
}