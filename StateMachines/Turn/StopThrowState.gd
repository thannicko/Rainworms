class_name StopThrowState extends State

@export var dice_container: Container
@export var prompt_label: Label
@export var tile_store_scene: PackedScene

var _invalid_thrown: bool
var _invalid_throw_type

func enter(data := {}) -> void:
	var state_machine := (self._state_machine as TurnStateMachine)
	_store_incoming_invalid_data(data)
	_check_invalid_throw_by_no_worm()
	_check_invalid_throw_by_no_tile()
	prompt_label.text = "Stopped throwing"
	if (_invalid_thrown):
		prompt_label.text += " with an invalid throw: " 
		prompt_label.text += state_machine._invalid_throw_type_to_string(_invalid_throw_type)
	else:
		state_machine._player.points_earned_in_turn = state_machine._current_points
		dice_container.hide()
		SceneChangerSingleton.change_to_scene(tile_store_scene, { 
			'player': state_machine._player,
			'deck': PlayerControllerSingleton._deck })

func _store_incoming_invalid_data(data := {}) -> void:
	_invalid_thrown = data["invalid_thrown"]
	if (_invalid_thrown):
		_invalid_throw_type = data["invalid_throw_type"] as TurnStateMachine.InvalidThrowType

func _check_invalid_throw_by_no_worm() -> void:
	var state_machine := (self._state_machine as TurnStateMachine)
	if not _invalid_thrown and state_machine._has_no_worms():
		_invalid_thrown = true
		_invalid_throw_type = TurnStateMachine.InvalidThrowType.NO_WORM

func _check_invalid_throw_by_no_tile() -> void:
	var state_machine := (self._state_machine as TurnStateMachine)
	var can_buy_a_tile: bool = DeckControllerSingleton.is_enough_for_a_tile(state_machine._current_points)
	if not can_buy_a_tile:
		_invalid_thrown = true
		_invalid_throw_type = TurnStateMachine.InvalidThrowType.NO_TILES
	
