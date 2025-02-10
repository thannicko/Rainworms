using Godot;

[GlobalClass]
public partial class WormTile : Resource
{
	[Export]
	public int Cost { get; set; }

	[Export]
	public int Worm { get; set; }

	[Export]
	public Texture2D TextureNormal { get; set; }

	[Export]
	public Texture2D TextureDisabled { get; set; }

	public WormTile() : this(0, 0) {}

	public WormTile(int cost, int worm)
	{
		Cost = cost;
		Worm = worm;
	}

    public string BuyInfo()
    {
        return Cost.ToString() + "\n" + WormString();
    }

    public string BoughtInfo()
    {
        return "Tile " + Cost.ToString() + " for " + WormString();
    }

    public string WormString()
    {
        return Worm.ToString() + " worm" + (Worm > 1 ? "s" : "");
    }
}