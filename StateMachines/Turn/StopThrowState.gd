class_name StopThrowState extends State

@export var prompt_label: Label
@export var throw_dice_button: Button
@export var deck_controller: DeckController

func enter(data := {}) -> void:
	throw_dice_button.disabled = true
	
	var state_machine := (self._state_machine as TurnStateMachine)
	if state_machine._has_no_worms():
		state_machine.invalidate_throw(TurnStateMachine.InvalidThrowType.NO_WORM)
	if deck_controller.has_nothing_to_buy():
		state_machine.invalidate_throw(TurnStateMachine.InvalidThrowType.NO_TILES)
	
	if not state_machine.is_valid_throw():
		prompt_label.text = "Invalid throw: " 
		prompt_label.text += state_machine.invalid_throw_reason()
		await get_tree().create_timer(1.0).timeout
		_take_invalid_actions()


func _take_invalid_actions() -> void:
	prompt_label.text = "Disabling the next highest tile..." 
	await get_tree().create_timer(1.0).timeout
	deck_controller.disable_next_highest_tile()
	
	prompt_label.text = "Returning player's top tile to the deck..." 
	await get_tree().create_timer(1.0).timeout
	
	var player: Player = (self._state_machine as TurnStateMachine)._player
	deck_controller.return_last_bought_tile_to_deck(player)

	await get_tree().create_timer(1.0).timeout
	self._state_machine._change_to_state("InitialThrowState")
