extends Control

@export var prompt_label: Label
@export var tile_container: Container
@export var total_label: Label
@export var owned_tiles_button: Button
@export var owned_tiles_panel: Panel
@export var owned_tiles_container: Container

var _deck: TileDeck = load("res://Deck.tres")
var _player := Player.new()

func initialize(data = {}) -> void:
	_player = data['player']

func _ready() -> void:
	prompt_label.text = "Select tile to buy: "
	_update_total_label()
	owned_tiles_button.button_down.connect(_show_owned_tiles)
	for tile in _deck.tiles:
		_spawn_button(tile)

func _tile_bought(tile: WormTile) -> void:
	prompt_label.text = "Bought tile: " + str(tile.display_details())
	_player.buy_tile(tile)
	_update_total_label()
	_disable_all_buttons()

func _is_tile_affordable(tile: WormTile) -> bool:
	return tile.cost <= _player.points_earned_in_turn

func _update_total_label() -> void:
	total_label.text = "Total: " + str(_player.points_earned_in_turn)

func _show_owned_tiles() -> void:
	owned_tiles_panel.visible = not owned_tiles_panel.visible
	#TODO: Maybe not respawn every time?
	for tile in owned_tiles_container.get_children():
		tile.queue_free()
		await get_tree().process_frame
	for tile in _player.tiles_bought:
		var button := Button.new()
		button.text = str(tile.buy_details())
		button.custom_minimum_size = Vector2(75, 50)
		owned_tiles_container.add_child(button)

func _spawn_button(tile: WormTile) -> void:
	var button := Button.new()
	button.text = str(tile.buy_details())
	button.custom_minimum_size = Vector2(75, 50)
	button.disabled = not _is_tile_affordable(tile)
	if (_is_tile_affordable(tile)):
		button.button_down.connect(_tile_bought.bind(tile))
	tile_container.add_child(button)
	
func _disable_all_buttons() -> void:
	for tile in tile_container.get_children():
		tile.disabled = true
