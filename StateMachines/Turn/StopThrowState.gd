class_name StopThrowState extends State

@export var dice_container: Container
@export var prompt_label: Label
const BuyingScene: String = "res://TileStore.tscn"

var _invalid_thrown: bool

func enter(data := {}) -> void:
	_invalid_thrown = data["invalid_thrown"]
	prompt_label.text = "Stopped throwing"
	if (_invalid_thrown):
		prompt_label.text += " with an invalid throw"
	else:
		dice_container.hide()
		get_tree().change_scene_to_file(BuyingScene)
	
