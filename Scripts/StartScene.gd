extends Control

@export var start_game_scene: PackedScene
@onready var _start_button: Button = $VBoxContainer/StartButton

func _ready():
	_start_button.button_down.connect(_start_game)


func _start_game():
	get_tree().change_scene_to_packed(start_game_scene)
