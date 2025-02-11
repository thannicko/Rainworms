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
            tile => !(tile as TextureButton).Disabled).ToList();
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

        WormTile lastBoughtTile = player.TilesBought.LastOrDefault();
        if (lastBoughtTile == null)
            return;
            
        deck.Tiles.Add(lastBoughtTile);
        var orderedTiles = deck.Tiles.OrderBy(tile => tile.Cost).ToList();
        deck.Tiles = new Godot.Collections.Array<WormTile>(orderedTiles);

        TextureButton lastBoughtButton = boughtTileToButton[lastBoughtTile];
        TextureButton toReturnToDeck = SpawnButton(lastBoughtTile);

        TilesContainer.AddChild(toReturnToDeck);
        TilesContainer.MoveChild(toReturnToDeck, deck.Tiles.IndexOf(lastBoughtTile));
        buttonToTile[toReturnToDeck] = lastBoughtTile;
        lastBoughtButton.QueueFree();
    }

    private void DrawDeck()
    {
        foreach (WormTile tile in deck.Tiles)
        {
            TextureButton button = SpawnButton(tile);
            TilesContainer.AddChild(button);
            buttonToTile[button] = tile;
        }
    }

    private void Refresh(int newPoints, bool hasNoWorms)
    {
        foreach (TextureButton button in TilesContainer.GetChildren())
        {
            button.Disabled = IsTileTooExpensive(buttonToTile[button], newPoints);
            button.Disabled |= hasNoWorms;
        }
    }

    private TextureButton SpawnButton(WormTile tile)
    {
        GD.Print("DeckController :: SpawnButton ", tile);
        var button = new TextureButton();
        button.StretchMode = TextureButton.StretchModeEnum.KeepAspectCentered;
        button.TextureNormal = tile.TextureNormal;
        button.TextureDisabled = tile.TextureDisabled;
        button.Disabled = true;
        button.ButtonDown += () => BuyTile(tile, button);
        return button;
    }

    private void BuyTile(WormTile tile, TextureButton button)
    {
        if (!IsBuyingEnabled || BoughtTilesContainer.GetChildren().Contains(button))
            return;
        deck.Tiles.Remove(tile);
        TilesContainer.RemoveChild(button);
        BoughtTilesContainer.AddChild(button);
        boughtTileToButton[tile] = button;
        EmitSignal(SignalName.TileBought, tile);
        GD.Print("DeckController :: Bought tile: ", tile.BoughtInfo());
    }
    
    private bool IsTileTooExpensive(WormTile tile, int points) => tile.Cost > points;
    
    private TileDeck deck = GD.Load<TileDeck>("res://Deck.tres");
    private Dictionary<TextureButton, WormTile> buttonToTile = new Dictionary<TextureButton, WormTile>();
    private Dictionary<WormTile, TextureButton> boughtTileToButton = new Dictionary<WormTile, TextureButton>();
}