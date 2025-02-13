using System;
using System.Collections.Generic;
using System.Linq;
using Godot;

public partial class ThrowingState : State
{
    [Export]
    public Button ThrowDiceButton { get; set; }

    [Export]
    public Container ThrowDiceContainer { get; set; }
    
    [Export]
    public AnimatedSprite2D DiceAnimation { get; set; }
    
    [Export]
    public Label PromptLabel { get; set; }
    
    [Export(PropertyHint.File, "*.tscn")]
    public string ThrownDiceScene { get; set; }

    private TurnStateMachine turnStateMachine { get => (TurnStateMachine)stateMachine; }

    private Dictionary<TextureRect, int> diceTexturesToValue = new Dictionary<TextureRect, int>();

    public override void Enter(object[] data)
    {
        diceTexturesToValue.Clear();
        turnStateMachine.ResetThrowCounts();
        ThrowDiceButton.Disabled = true;
        PromptLabel.Hide();
        StartThrow();
    }

    public override void Exit()
    {
        foreach (ThrownDice dice in diceTexturesToValue.Keys)
        {
            dice.DiceClicked -= OnDiceClicked;
        }
    }

    private void StartThrow()
    {
        DiceAnimation.Play();
        DiceAnimation.Show();
        var timer = GetTree().CreateTimer(1.0);
        timer.Timeout += EndThrow;
    }

    private void EndThrow()
    {
        DiceAnimation.Hide();

        var rng = new RandomNumberGenerator();
        for (int dice = 0; dice < turnStateMachine.NrDicesLeft; dice++)
        {
            var diceValue = rng.RandiRange(1, 6);
            StoreDiceFrequency(diceValue);
            SpawnDiceTexture(diceValue);
        }

        var allowedDices = turnStateMachine.diceFrequencies.Keys.Where(dice => IsAllowedToKeepDice(dice)).ToList();
        if (allowedDices.Count == 0)
        {
            turnStateMachine.InvalidateThrow(TurnStateMachine.InvalidThrowType.NO_DICES);
            stateMachine.ChangeToState("StopThrowState");
        }
        else
        {
            PromptLabel.Text = "Click on a dice to keep it";
        }
        PromptLabel.Show();
    }

    private void StoreDiceFrequency(int diceValue)
    {
        if (turnStateMachine.diceFrequencies.ContainsKey(diceValue))
        {
            turnStateMachine.diceFrequencies[diceValue] += 1;
        }
        else 
        {
            turnStateMachine.diceFrequencies[diceValue] = 1;
        }
    }

    private void SpawnDiceTexture(int diceValue)
    {
        var newDice = GD.Load<PackedScene>(ThrownDiceScene).Instantiate() as ThrownDice;
        newDice.Initialize(
            diceValue: diceValue,
            textureFile: turnStateMachine.diceToTextures[diceValue]
        );
        newDice.EnableClick(IsAllowedToKeepDice(diceValue));
        newDice.DiceClicked += OnDiceClicked;
        ThrowDiceContainer.AddChild(newDice);
        diceTexturesToValue[newDice] = diceValue;
    }

    private void OnDiceClicked(ThrownDice dice)
    {
        object[] stateChangeData = new object[1];
        stateChangeData[0] = new Dictionary<string, object> {
            { "dice_value", dice.DiceValue },
            { "dice_textures" , diceTexturesToValue },
        }; 
        turnStateMachine.ChangeToState(
            newStateName: "KeepDiceState",
            data: stateChangeData);
    }

    private bool IsAllowedToKeepDice(int diceValue)
    {
        return !turnStateMachine.keptDices.ContainsKey(diceValue);
    }
}