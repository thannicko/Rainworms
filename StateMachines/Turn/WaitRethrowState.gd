class_name WaitRethrowState extends State

@export var prompt_label: Label
@export var throw_dice_button: Button
@export var stop_throw_button: Button
@export var throw_dice_container: Container

func enter(data := {}) -> void:
	prompt_label.text = "Throw the dice or stop to buy a tile"
	_clear_container(throw_dice_container)
	
	throw_dice_button.disabled = false
	throw_dice_button.button_down.connect(_on_throw_dice_button_down)
	throw_dice_button.show()
	
	stop_throw_button.disabled = false
	stop_throw_button.button_down.connect(_on_stop_throw_button_down)

func _on_throw_dice_button_down():
	self._state_machine._change_to_state("ThrowingState")

func _on_stop_throw_button_down():
	self._state_machine._change_to_state("StopThrowState")

func _clear_container(container: Container) -> void:
	for child in container.get_children():
		child.queue_free()

func exit() -> void:
	throw_dice_button.button_down.disconnect(_on_throw_dice_button_down)
	stop_throw_button.button_down.disconnect(_on_stop_throw_button_down)
