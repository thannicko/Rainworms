using Godot;
using Godot.Collections;

public partial class PlayerController : Node
{
    public static PlayerController Instance { get; private set; }
    public Array<Player> Players { get; private set; } = new Array<Player>();
    public Player ActivePlayer { get; private set; }

    private TileDeck deck = GD.Load<TileDeck>("res://Deck.tres");

	public override void _Ready()
	{
        Instance  = this;
        Player testPlayer = new Player()
        {
            Name = "Thannicko"
        };
        Players.Add(testPlayer);
        ActivePlayer = testPlayer;
	}
}