class_name StopThrowState extends State

@export var dice_container: Container
@export var prompt_label: Label
@export var tile_store_scene: PackedScene

var _invalid_thrown: bool

func enter(data := {}) -> void:
	_invalid_thrown = data["invalid_thrown"]
	prompt_label.text = "Stopped throwing"
	if (_invalid_thrown):
		prompt_label.text += " with an invalid throw"
	else:
		var state_machine := (self._state_machine as TurnStateMachine)
		state_machine._player.points_earned_in_turn = state_machine._current_points
		dice_container.hide()
		SceneChangerSingleton.change_to_scene(tile_store_scene, { 'player': state_machine._player })
	
