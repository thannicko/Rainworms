class_name InitialThrowState extends State

@export var throw_dice_button: Button
@export var stop_throw_button: Button
@export var throw_dice_container: Container
@export var keep_dice_container: Container
@export var total_label: Label
@export var prompt_label: Label

func enter(data := {}) -> void:
	#TODO Someone else should set our player
	var state_machine := self._state_machine as TurnStateMachine
	state_machine._player = PlayerControllerSingleton._active_player
	_clear_container(throw_dice_container)
	_clear_container(keep_dice_container)
	throw_dice_button.button_down.connect(_on_throw_dice_button_down)
	stop_throw_button.disabled = true
	prompt_label.text = "Throw the dice!"
	prompt_label.show()
	throw_dice_button.show()
	total_label.hide()
	state_machine.points_earned.connect(_update_total_label)

func _update_total_label(new_points: int) -> void:
	total_label.text = "Total: " + str(new_points)
	total_label.show()

func _on_throw_dice_button_down():
	self._state_machine._change_to_state("ThrowingState")

func _clear_container(container: Container) -> void:
	for child in container.get_children():
		child.queue_free()

func exit() -> void:
	throw_dice_button.button_down.disconnect(_on_throw_dice_button_down)
