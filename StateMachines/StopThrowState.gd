class_name StopThrowState extends State

@export var throw_dice_button: Button
@export var stop_throw_button: Button
@export var result_label: Label

func enter(data := {}) -> void:
	var invalid_thrown: bool = data["invalid_thrown"]
	throw_dice_button.disabled = true
	stop_throw_button.disabled = true
	result_label.text = "Stopped throwing"
	if (invalid_thrown):
		result_label.text += " with an invalid throw"
	result_label.show()
