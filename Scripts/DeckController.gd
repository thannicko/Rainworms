extends Node

var _deck: TileDeck = load("res://Deck.tres")

func available_tiles() -> Array[WormTile]:
	return _deck.tiles

func remove_tile(tile: WormTile) -> void:
	_deck.tiles.erase(tile)

func is_enough_for_a_tile(points: int) -> bool:
	for tile in _deck.tiles:
		if (tile.cost <= points):
			return true
	return false
