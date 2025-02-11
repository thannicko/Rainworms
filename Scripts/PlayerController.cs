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

	public override void _Ready()
	{
        Instance  = this;
	}

    public Player Winner()
    {
        if (Players.Count > 0)
        {
            return Players.OrderByDescending(player => player.TotalScore).FirstOrDefault();
        }
        return null;
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
        GD.Print("PlayerController :: Current player: ", activePlayer.Name, "-> new player ", Players.ElementAt(nextIndex).Name);
        ActivePlayer = Players.ElementAt(nextIndex);
    }
}