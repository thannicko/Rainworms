extends Control

@export var prompt_label: Label
@export var tile_container: Container
@export var total_label: Label

var _deck: TileDeck = load("res://Deck.tres")

func _ready() -> void:
	prompt_label.text = "Select tile to buy: "
	for tile in _deck.tiles:
		var button := Button.new()
		button.text = str(tile.cost) + "\n" + str(tile.worm) + " worm(s)"
		button.custom_minimum_size.y = 50
		button.disabled = not _is_tile_affordable(tile)
		if (_is_tile_affordable(tile)):
			button.button_down.connect(_tile_bought.bind(tile))
		tile_container.add_child(button)

func _tile_bought(tile: WormTile) -> void:
	for option in tile_container.get_children():
		option.disabled = true
	prompt_label.text = "Bought tile: " + str(tile.cost)

func _is_tile_affordable(tile: WormTile) -> bool:
	#TODO
	return true
