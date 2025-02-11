using System.Linq;
using Godot;
using Godot.Collections;

public partial class Player : RefCounted
{
    public string Name { get; set; }
    public int PointsEarnedInTurn { get; set; }
    public int TotalScore { get => NumberOfWormsBought(); }
    public Array<WormTile> TilesBought { get; set; } = new Array<WormTile>();

    public int NumberOfWormsBought()
    {
        return TilesBought.Sum(tile => tile.Worm);
    }

    public WormTile PopLastTile()
    {
        if (TilesBought.Count > 0)
        {
            var tile = TilesBought.Last();
            TilesBought.Remove(tile);
            return tile;
        }
        return null;
    }
}