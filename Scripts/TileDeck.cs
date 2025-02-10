using Godot;
using Godot.Collections;

[GlobalClass]
public partial class TileDeck : Resource
{
	[Export]
	public Array<WormTile> Tiles { get; set; }

	public TileDeck() : this(null) {}

	public TileDeck(Array<WormTile> tiles)
	{
		Tiles = tiles;
	}
}
