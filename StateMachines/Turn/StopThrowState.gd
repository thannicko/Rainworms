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
	
	if (state_machine.is_valid_throw()):
		self._state_machine._change_to_state("BuyingTileState")
	else:
		prompt_label.text = "Invalid throw: " 
		prompt_label.text += state_machine.invalid_throw_reason()
	
