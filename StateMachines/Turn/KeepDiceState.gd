class_name KeepDiceState extends State

@export var stop_throw_button: Button
@export var throw_dice_container: Container
@export var keep_dice_container: Container

var _keep_dice: int = 0
var _dice_textures_to_value: Dictionary = {}
var _dice_side_to_points: Dictionary = {}

func enter(data := {}) -> void:
	var state_machine := (self._state_machine as TurnStateMachine)
	_keep_dice = data["dice_value"]
	_dice_textures_to_value = data["dice_textures"]
	_dice_side_to_points = state_machine._dice_side_to_points
	_move_textures_to_keep_container()
	state_machine._kept_dices[_keep_dice] = state_machine._dices_frequency[_keep_dice]
	state_machine._nr_dices_left = state_machine._nr_dices_left - state_machine._kept_dices[_keep_dice]
	state_machine.set_points_earned(_get_points_earned(state_machine._kept_dices))
	if state_machine._nr_dices_left <= 0:
		self._state_machine._change_to_state("StopThrowState")
	else:
		self._state_machine._change_to_state("WaitRethrowState")

func _move_textures_to_keep_container() -> void:
	var textures_to_remove: Array[Node] = []
	for texture in _dice_textures_to_value.keys():
		if _dice_textures_to_value[texture] == _keep_dice:
			textures_to_remove.append(texture)
			keep_dice_container.add_child((texture as Node).duplicate())
	for texture in textures_to_remove:
		texture.queue_free()

func _get_points_earned(dice_to_nr_of_throws: Dictionary) -> int:
	var sum: int = 0
	for dice in dice_to_nr_of_throws.keys():
		sum = sum + _dice_side_to_points[dice] * dice_to_nr_of_throws[dice]
	return sum
