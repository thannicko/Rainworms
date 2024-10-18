class_name KeepDiceState extends State

@export var stop_throw_button: Button
@export var throw_dice_container: Container
@export var keep_dice_container: Container
@export var total_label: Label
@export var prompt_label: Label

var _keep_dice: int = 0
var _dice_textures_to_value: Dictionary = {}

func enter(data := {}) -> void:
	var state_machine := (self._state_machine as TurnStateMachine)
	_keep_dice = data["dice_value"]
	_dice_textures_to_value = data["dice_textures"]
	_move_textures_to_keep_container()
	state_machine._kept_dices[_keep_dice] = state_machine._dices_frequency[_keep_dice]
	state_machine._nr_dices_left = state_machine._nr_dices_left - state_machine._kept_dices[_keep_dice]
	state_machine._current_points = _get_sum(state_machine._kept_dices)
	total_label.text = "Total: " + str(state_machine._current_points)
	total_label.show()
	if state_machine._nr_dices_left <= 0:
		prompt_label.text = "Out of dice"
		self._state_machine._change_to_state("StopThrowState", { 'invalid_thrown': false })
	else:
		prompt_label.text = "Throw the dice!"
		self._state_machine._change_to_state("WaitRethrowState")

func _move_textures_to_keep_container() -> void:
	var textures_to_remove: Array[Node] = []
	for texture in _dice_textures_to_value.keys():
		if _dice_textures_to_value[texture] == _keep_dice:
			textures_to_remove.append(texture)
			keep_dice_container.add_child((texture as Node).duplicate())
	for texture in textures_to_remove:
		texture.queue_free()

func _get_sum(dice_to_nr_of_throws: Dictionary) -> int:
	var sum: int = 0
	for dice in dice_to_nr_of_throws.keys():
		sum = sum + dice * dice_to_nr_of_throws[dice]
	return sum
