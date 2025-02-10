using System.Collections.Generic;
using System.Linq;
using Godot;

public partial class DeckController : Node
{
    [Export]
    public Container TilesContainer { get; set; }
    
    [Export]
    public Container BoughtTilesContainer { get; set; }
    
    [Export]
    public TurnStateMachine TurnController { get; set; }
    
    [Signal]
    public delegate void TileBoughtEventHandler(WormTile tile);

    public bool IsBuyingEnabled { get; set; }

	public override void _Ready()
    {
        DrawDeck();
        TurnController.PointsEarned += Refresh;
    }
    
    public bool HasNothingToBuy()
    {
        var availableTiles = TilesContainer.GetChildren().Where(
            tile => !(tile as Button).Disabled).ToList();
        return availableTiles.Count == 0;
    }

    public void DisableNextHighestTile()
    {
        Node nextHighestTile = TilesContainer.GetChildren().Last();
        TilesContainer.GetChildren().Remove(nextHighestTile);
        nextHighestTile.QueueFree();
    }

    public void ReturnLastBoughtTilesToDeck(Player player)
    {
        WormTile lastBoughtTile = player.TilesBought.Last();
        if (lastBoughtTile == null)
            return;
        
        deck.Tiles.Add(lastBoughtTile);
        deck.Tiles.OrderBy(tile => tile.Cost);

        Button lastBoughtButton = boughtTileToButton[lastBoughtTile];
        Button toReturnToDeck = SpawnButton(lastBoughtTile);
        TilesContainer.AddChild(toReturnToDeck);
        TilesContainer.MoveChild(toReturnToDeck, deck.Tiles.IndexOf(lastBoughtTile));
        buttonToTile[toReturnToDeck] = lastBoughtTile;
        lastBoughtButton.QueueFree();
    }

    private void DrawDeck()
    {
        foreach (WormTile tile in deck.Tiles)
        {
            Button button = SpawnButton(tile);
            TilesContainer.AddChild(button);
            buttonToTile[button] = tile;
        }
    }

    private void Refresh(int newPoints, bool hasNoWorms)
    {
        foreach (Button button in TilesContainer.GetChildren())
        {
            button.Disabled = IsTileTooExpensive(buttonToTile[button], newPoints);
            button.Disabled |= hasNoWorms;
        }
    }

    private Button SpawnButton(WormTile tile)
    {
        var button = new Button();
        button.Text = tile.BuyInfo();
        button.CustomMinimumSize = new Vector2(75, 50);
        button.Disabled = true;
        button.ButtonDown += () => BuyTile(tile, button);
        return button;
    }

    private void BuyTile(WormTile tile, Button button)
    {
        if (!IsBuyingEnabled)
            return;
        deck.Tiles.Remove(tile);
        button.Disabled = true;
        TilesContainer.RemoveChild(button);
        BoughtTilesContainer.AddChild(button);
        EmitSignal(SignalName.TileBought, tile);
        GD.Print("DeckController :: Bought tile: ", tile.BoughtInfo());
    }
    
    private bool IsTileTooExpensive(WormTile tile, int points) => tile.Cost > points;
    
    private TileDeck deck = GD.Load<TileDeck>("res://Deck.tres");
    private Dictionary<Button, WormTile> buttonToTile = new Dictionary<Button, WormTile>();
    private Dictionary<WormTile, Button> boughtTileToButton = new Dictionary<WormTile, Button>();
}