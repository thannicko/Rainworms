class_name DeckController extends Node

@export var tiles_container: Container
@export var bought_tiles_container: Container
@export var turn_controller: TurnStateMachine

signal tile_bought(tile: WormTile)

var _deck: TileDeck = load("res://Deck.tres")
var _button_to_tile: Dictionary = {}
var _buying_enabled: bool = false

func _ready():
	_draw_deck()
	turn_controller.points_earned.connect(_refresh)

func has_nothing_to_buy() -> bool:
	var available_tiles: Array = tiles_container.get_children().filter(func(tile): return not tile.disabled)
	return available_tiles.is_empty()

func set_enable_buying(enabled: bool) -> void:
	_buying_enabled = enabled

func _refresh(new_points: int, has_no_worms: bool) -> void:
	for button in tiles_container.get_children():
		button.disabled = _is_tile_too_expensive(_button_to_tile[button], new_points)
		button.disabled = button.disabled or has_no_worms

func _is_tile_too_expensive(tile: WormTile, points: int) -> bool:
	return tile.cost > points

func _buy_tile(tile: WormTile, button: Button) -> void:
	if (_buying_enabled):
		_deck.tiles.erase(tile)
		button.disabled = true
		bought_tiles_container.add_child(button.duplicate())
		button.queue_free()
		tile_bought.emit(tile)
		print("DeckController :: Bought tile: ", tile.display_details())

func _draw_deck() -> void:
	for tile in _deck.tiles:
		var button := Button.new()
		button.text = str(tile.buy_details())
		button.custom_minimum_size = Vector2(75, 50)
		button.disabled = true
		button.button_down.connect(_buy_tile.bind(tile, button))
		tiles_container.add_child(button)
		_button_to_tile[button] = tile
		
