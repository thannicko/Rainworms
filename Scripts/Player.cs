using Godot;
using Godot.Collections;

public partial class Player : RefCounted
{
    public string Name { get; set; }
    public int PointsEarnedInTurn { get; set; }
    public Array<WormTile> TilesBought { get; set; } = new Array<WormTile>();
}