class_name StopThrowState extends State

@export var dice_container: Container
@export var buy_container: Container
@export var tile_container: Container
@export var dice_prompt_label: Label
@export var buy_prompt_label: Label

var _invalid_thrown: bool
var _deck: TileDeck

func enter(data := {}) -> void:
	_invalid_thrown = data["invalid_thrown"]
	_deck = (self._state_machine as TurnStateMachine)._deck
	dice_prompt_label.text = "Stopped throwing"
	if (_invalid_thrown):
		dice_prompt_label.text += " with an invalid throw"
	else:
		dice_container.hide()
		_offer_buy()

func _offer_buy() -> void:
	buy_prompt_label.text = "Select tile to buy: "
	buy_container.show()
	for tile in _deck.tiles:
		var button := Button.new()
		button.text = str(tile.cost) + "\n" + str(tile.worm) + " worm(s)"
		button.custom_minimum_size.y = 50
		button.disabled = not _is_tile_affordable(tile)
		if (_is_tile_affordable(tile)):
			button.button_down.connect(_tile_bought.bind(tile, button))
		tile_container.add_child(button)

func _tile_bought(tile: WormTile, button: Button) -> void:
	for option in tile_container.get_children():
		option.disabled = true
	(self._state_machine as TurnStateMachine)._current_points = 0
	buy_prompt_label.text = "Bought tile: " + str(tile.cost)

func _is_tile_affordable(tile: WormTile) -> bool:
	return tile.cost <= (self._state_machine as TurnStateMachine)._current_points
	
