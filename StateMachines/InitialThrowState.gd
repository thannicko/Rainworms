class_name InitialThrowState extends State

@export var throw_dice_button: Button
@export var stop_throw_button: Button
@export var throw_dice_container: Container
@export var keep_dice_container: Container
@export var message_label: Label
@export var total_label: Label

func enter(data := {}) -> void:
	throw_dice_button.show()
	throw_dice_button.button_down.connect(_on_throw_dice_button_down)
	stop_throw_button.disabled = true
	_clear_container(throw_dice_container)
	_clear_container(keep_dice_container)
	message_label.hide()
	total_label.hide()

func _on_throw_dice_button_down():
	self._state_machine._change_to_state("ThrowingState")

func _clear_container(container: Container) -> void:
	for child in container.get_children():
		child.queue_free()

func exit() -> void:
	throw_dice_button.button_down.disconnect(_on_throw_dice_button_down)
