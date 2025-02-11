using System.Linq;
using Godot;
using Godot.Collections;

public partial class PlayerController : Node
{
    public static PlayerController Instance { get; private set; }
    public Array<Player> Players { get; private set; } = new Array<Player>();
    private Player activePlayer = null;
    public Player ActivePlayer
    {
        get
        {
            if (activePlayer == null)
                activePlayer = Players.FirstOrDefault();
            return activePlayer;
        }
        set
        {
            activePlayer = value;
        }
    }

    private TileDeck deck = GD.Load<TileDeck>("res://Deck.tres");

	public override void _Ready()
	{
        Instance  = this;
	}

    public void AddPlayer(string name)
    {
        Players.Add(new Player()
        {
            Name = name
        });
        GD.Print("PlayerController :: Added ", name);
    }

    public void GameStarted()
    {
        ActivePlayer = Players.FirstOrDefault();
    }

    public void ActivePlayerFinished()
    {
        var activeIndex = Players.IndexOf(ActivePlayer);
        var nextIndex = (activeIndex + 1) % Players.Count;
        ActivePlayer.PlayerTurnFinished();
        ActivePlayer = Players.ElementAt(nextIndex);
    }
}