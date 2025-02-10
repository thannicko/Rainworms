using System.Collections.Generic;
using Godot;

public partial class TurnStateMachine : StateMachine
{
    [Signal]
    public delegate void PointsEarnedEventHandler(int newPoints, bool hasNoWorms);

    [Export]
    public Label TotalLabel { get; set; }

    public Player Player { get; private set; }

    public int PointsEarnedInTurn { get => Player.PointsEarnedInTurn; }
    public bool IsValidThrow { get; private set; }

    public int NrDicesLeft { get; set; } = MaxThrows;

    public enum InvalidThrowType { NO_DICES, NO_wORM, NO_TILES }

    private const int MaxThrows = 8;
    private const int Worm = 6;

    public readonly Dictionary<int, string> diceToTextures = new Dictionary<int, string>(){
        { 1, "res://Assets/dice-one.png" },
        { 2, "res://Assets/dice-two.png" },
        { 3, "res://Assets/dice-three.png" },
        { 4, "res://Assets/dice-four.png" },
        { 5, "res://Assets/dice-five.png" },
        { 6, "res://Assets/dice-worm.png" }
    };

    public readonly Dictionary<int, int> diceToPoints = new Dictionary<int, int>()
    {
        { 1, 1 },
        { 2, 2 },
        { 3, 3 },
        { 4, 4 },
        { 5, 5 },
        { 6, 5 }, // This is the worm
    };
    public bool HasNoWorms()
    {
        return !keptDices.ContainsKey(Worm);
    }

    public void BuyTile(WormTile tile)
    {
        SetPointsEarned(0);
        Player.TilesBought.Add(tile);
    }

    public void SetPlayer(Player newPlayer)
    {
        Player = newPlayer;
    }

    public void SetPointsEarned(int points)
    {
        Player.PointsEarnedInTurn = points;
        UpdateTotalLabel(Player.PointsEarnedInTurn);
        EmitSignal(SignalName.PointsEarned, Player.PointsEarnedInTurn, HasNoWorms());
    }
    
    public void Reset()
    {
        ResetThrowCounts();
        IsValidThrow = false;
        keptDices.Clear();
        NrDicesLeft = MaxThrows;
    }

    public void ResetThrowCounts()
    {
        diceFrequencies.Clear();
    }

    public void InvalidateThrow(InvalidThrowType reason)
    {
        IsValidThrow = false;
        invalidThrowType = reason;
    }

    public string InvalidThrowReason()
    {
        switch (invalidThrowType)
        {
            case InvalidThrowType.NO_DICES:
                return "Cannot keep any dices";
            case InvalidThrowType.NO_wORM:
                return "No worm thrown";
            case InvalidThrowType.NO_TILES:
                return "Cannot buy any tiles";
            default:
                return "";
        }
    }

    private void UpdateTotalLabel(int points)
    {
        TotalLabel.Text = "Total: " + points.ToString();
        TotalLabel.Show();
    }

    private InvalidThrowType invalidThrowType;
    public Dictionary<int, int> diceFrequencies = new Dictionary<int, int>();
    public Dictionary<int, int> keptDices = new Dictionary<int, int>();
}